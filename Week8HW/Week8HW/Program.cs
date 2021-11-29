using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

while (!File.Exists("todo.dat"))
{
    File.Create("todo.dat").Close();
}

List<string> todos = new List<string>();
string file = "todo.dat";
Deserialize();

if (args.Length == 0) ShowError("No command was provided");

else if (args[0] == "show")
{
    DisplayTodoList(); 
}
else if (args[0] == "add")
{
    try
    {
        AddTodo(args[1]);
    }
    catch (IndexOutOfRangeException)
    {
        ShowError("Nothing to add!");
    }
}
else if (args[0] == "done")
{
    try
    {
        MarkTodoDone(args[1]);
    }
    catch (IndexOutOfRangeException)
    {
        ShowError("Nothing to remove!");
    }
}
else if (args[0] == "clear") ClearTodo();
else if (args[0] == "stack")
{
    try
    {
        StackItems(args[1]);
    }
    catch (IndexOutOfRangeException) { ShowError("nothing to stack!"); }
    //catch (ArgumentNullException) { ShowError("nothing to stack!"); }
}
else ShowError("Unknown command: '" + args[0] + "'");

void DisplayTodoList()
{
    Deserialize();
    foreach (string item in todos)
	{
        Console.WriteLine(item);
	}
}

void AddTodo(string item)
{
    todos.Add(item);
    UpdateFile();
    Console.WriteLine("'" + item + "' Successfully added");

    var count = todos.Where(x => x == item).Count(c => c.Count() > 1);
    Convert.ToInt32(count);
    if (count >= 2)
    {
        if (File.Exists(item + ".dat"))
		{
            List<string> tempList = new List<string>();
            FileStream streamFile = null;
            try
            {
                streamFile = File.OpenRead(item + ".dat");
                BinaryFormatter bf = new BinaryFormatter();
                tempList = bf.Deserialize(streamFile) as List<string>;
                streamFile.Close();

                string oldStack = tempList.FirstOrDefault();
                int oldStackNum = Convert.ToInt32(oldStack.Split('x')[0]);
            }
            catch (SystemException)
            {
                streamFile.Close();
                Console.WriteLine("here");
                return;
            }
        }
        WritelineColour("NOTE: dublicates are stored seperately, if would you like to represent your item as: ", ConsoleColor.Yellow);
        Console.WriteLine(count + "x " + item);
        WritelineColour("\n" + AppDomain.CurrentDomain.FriendlyName + " stack " + item, ConsoleColor.Green);
    }
    else return;
}

void MarkTodoDone(string item)
{
    Console.WriteLine("Successfully removed " + item);
    todos.Remove(item);
    UpdateFile();
}

void ClearTodo()
{
    todos.Clear();
    UpdateFile();
    Console.WriteLine(file + " successfully cleared");
}

void UpdateFile()
{
    FileStream streamFile = File.OpenWrite(file);
    BinaryFormatter bf = new BinaryFormatter();
    bf.Serialize(streamFile, todos);
    streamFile.Close();
}

void StackItems(string item)
{
    var count = todos.Where(x => x == item).Count(c => c.Count() > 1);
    Convert.ToInt32(count);

    if (File.Exists(item + ".dat"))
    {
        List<string> tempList = new List<string>();
        FileStream streamFile = null;
        try
        {
            streamFile = File.OpenRead(file);
            BinaryFormatter nbf = new BinaryFormatter();
            tempList = nbf.Deserialize(streamFile) as List<string>;
            streamFile.Close();

            string oldStack = tempList.FirstOrDefault();
            todos.RemoveAll(i => i == oldStack);
            int oldStackNum = Convert.ToInt32(oldStack.Split('x')[0]);
            count = (count + oldStackNum);
            File.Delete(item + ".dat");
        }
        catch (SystemException)
        {
            streamFile.Close();
            Console.WriteLine("here");
            return;
        }
    }

    string newItem = (count + "x " + item);
    todos.Add(newItem);

    File.Create(item + ".dat").Close();
    FileStream newStreamFile = File.OpenWrite(item + ".dat");
    BinaryFormatter bf = new BinaryFormatter();
    bf.Serialize(newStreamFile, newItem);
    newStreamFile.Close();

    todos.RemoveAll(i => i == item);
    UpdateFile();
    Console.WriteLine(item + " succesfully stacked");
}
void WritelineColour(string message, ConsoleColor colour)
{
    var before = Console.ForegroundColor;
    Console.ForegroundColor = colour;
    Console.WriteLine(message);
    Console.ForegroundColor = before;
}
void ShowError(string message)
{
    string executable = AppDomain.CurrentDomain.FriendlyName;
    WritelineColour("Error: " + message, ConsoleColor.Red);
    Console.WriteLine("\nDisplay todo List file: ");
    WritelineColour(executable + " show", ConsoleColor.Green);
    Console.WriteLine("\nAdd item to, todo list file: ");
    WritelineColour(executable + " add 'item'", ConsoleColor.Green);
    Console.WriteLine("\nRemove item from todo list file: ");
    WritelineColour(executable + " done 'item'", ConsoleColor.Green);
    Console.WriteLine("\nClear todo list: ");
    WritelineColour(executable + " clear", ConsoleColor.Green);
}
void Deserialize()
{
    FileStream streamFile = null;
    try
    {
        streamFile = File.OpenRead(file);
        BinaryFormatter bf = new BinaryFormatter();
        todos = bf.Deserialize(streamFile) as List<string>;
        streamFile.Close();
    }
    catch (SystemException)
    {
        streamFile.Close();
        return;
    }
}