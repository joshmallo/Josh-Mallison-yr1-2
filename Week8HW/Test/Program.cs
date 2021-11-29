using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
while (!File.Exists("todo.dat"))
{
    FileStream f = File.Create("todo.dat");
    f.Close();
}

List<string> todos = new List<string>();
string file = "todo.dat";
Deserialize();

Console.WriteLine("addsomit: ");
string ans = Console.ReadLine();
if (ans == "show")
{
    Deserialize();
    foreach (string item in todos)
    {
        Console.WriteLine(item);
    }
}
else
{
    todos.Add(ans);
    UpdateFile();
}

List<string> temp = new List<string>();
FileStream streamFile = null;
try
{
    streamFile = File.OpenRead(file);
    BinaryFormatter bf = new BinaryFormatter();
    temp = bf.Deserialize(streamFile) as List<string>;
    streamFile.Close();
    string oldStack = temp.First();
    Console.WriteLine(oldStack);
}
catch (SystemException)
{
    streamFile.Close();
    return;
}
	void UpdateFile()
{
    FileStream streamFile = File.OpenWrite(file);
    BinaryFormatter bf = new BinaryFormatter();
    bf.Serialize(streamFile, todos);
    streamFile.Close();
}
void Deserialize()
{
    FileStream streamFile = null;
    try
    {
        streamFile = File.OpenRead(file);
        BinaryFormatter bf = new BinaryFormatter();
        List<string> todos = bf.Deserialize(streamFile) as List<string>;
        streamFile.Close();
        //foreach(string item in todos1)
		//{
        //    todos.Add(item);
		//}
    }
    catch (SystemException)
    {
        streamFile.Close();
        return;
    }
}