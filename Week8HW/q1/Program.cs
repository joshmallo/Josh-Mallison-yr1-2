using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
while (!File.Exists("todo.dat"))                                   //Creates file in case of delete
{
    File.Create("todo.dat").Close();
}
Dictionary<string, int> todos = new Dictionary<string, int>();     //Dictionary to easily count number of dublicates
string file = "todo.dat";
Deserialize();                                                     //Function adds serialized file to dictionary set

if (args.Length == 0) ShowError("No command was provided");        //ShowError formats a message with colours and set instructions for input

else if (args[0].ToLower() == "show")
{
    DisplayTodoList();
}
else if (args[0].ToLower() == "add")
{
    try
    {
        AddTodo(args[1].ToLower());
    }
    catch (IndexOutOfRangeException)
    {
        ShowError("Nothing to add!");
    }
}
else if (args[0].ToLower() == "done")
{
    try
    {
        MarkTodoDone(args[1].ToLower());
    }
    catch (IndexOutOfRangeException)
    {
        ShowError("Nothing to remove!");
    }
}
else if (args[0].ToLower() == "clear")
{
    try
    {
        ClearTodo();
    }
    catch (NullReferenceException) { ShowError("Nothing to clear!"); }
    catch (SystemException) { ShowError("Nothing to clear!"); }
}
else ShowError("Unknown command: '" + args[0].ToLower() + "'");
void DisplayTodoList()
{
    Deserialize();
    todos.ToList().ForEach(x => Console.WriteLine(x.Key + " x" + x.Value)); //DisplayTodoList outputs dictionary set in 'item' x'NumOfItems'
}
void AddTodo(string item)                                                   //Passes through user input
{
    bool exists = todos.ContainsKey(item);                                  //Checks if item already in the dictionary
	if (exists == true)
	{
        var value = todos.Where(x => x.Key == item).Count(c => c.Value >= 0); //Finds number of current items
        Convert.ToInt32(value);
        todos.Remove(item);
        todos.Add(item, value + 1);                                        //Adds 1 to the number of items already in set
    }
    else
	{
        todos.Add(item , 1);                                               //Otherwise just add 1 item
	}
    Console.WriteLine(item + " succesfully added to list");

    UpdateFile();
}
void MarkTodoDone(string item)
{
    foreach (var todo in todos.Where(x => x.Key == item))        //Find where the user input matches the key in dictionary set
    {
        todos.Remove(todo.Key);                                  //Removes set where user input matches key
    }
    UpdateFile();
    Console.WriteLine("Successfully removed " + item);
}
void ClearTodo()
{
    todos.Clear();                                               //Empties dictionary, then updates file
    UpdateFile();
    Console.WriteLine(file + " successfully cleared");
}
void UpdateFile()
{
    FileStream streamFile = File.OpenWrite(file);                //Serialises into todo file
    BinaryFormatter bf = new BinaryFormatter();
    bf.Serialize(streamFile, todos);
    streamFile.Close();
}
void WritelineColour(string message, ConsoleColor colour)       
{
    var before = Console.ForegroundColor;                       //When called allows a colour for text to be passed through and then reset to original
    Console.ForegroundColor = colour;
    Console.WriteLine(message);
    Console.ForegroundColor = before;
}
void ShowError(string message)
{
    string executable = AppDomain.CurrentDomain.FriendlyName;   //Format for error message
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
        streamFile = File.OpenRead(file);                              //Deserialises file into dictionary set
        BinaryFormatter bf = new BinaryFormatter();
        todos = bf.Deserialize(streamFile) as Dictionary<string, int>;
        streamFile.Close();
    }
    catch (SystemException)
    {
        streamFile.Close();
        return;
    }
}