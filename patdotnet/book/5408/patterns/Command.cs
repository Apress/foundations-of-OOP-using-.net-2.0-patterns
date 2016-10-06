using System;
using System.Collections;

public interface Command
{
	void initialize();
	void execute();
}

public class Operation : Command
{
	public void initialize() 
	{
	}
	public void execute()
	{
		Console.WriteLine( "Execute the operation");
	}
}

class Macro 
{
	private ArrayList commands = new ArrayList();
	public void add(Command c) { commands.Add(c); }
	public void run() 
	{
		IEnumerator items = commands.GetEnumerator();

		while( items.MoveNext())
		{
			Command cmd = (Command)items.Current;

			cmd.execute();
		} 
	}
}

public class TestCommand
{
	public void Execute()
	{
		Macro macro = new Macro();

		macro.add( new Operation());
		macro.run();
	}
}