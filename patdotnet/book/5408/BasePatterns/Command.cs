using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CommandReceiver {
    public void Output(String buffer) {
        Console.WriteLine(buffer);
    }
}
public interface ICommand<Receiver> {
    void Execute( Receiver receiver);
}
[Serializable]
public class Operation : ICommand< CommandReceiver> {
    private string _data;
    public Operation(string data) {
        _data = data;
    }
    public void Execute(CommandReceiver output) {
        output.Output( "My Data (" + _data + ")");
    }
}

class Invoker< Receiver> {
    private List<ICommand< Receiver>> _commands = new List<ICommand<Receiver>>();
    public void Add( ICommand<Receiver> command) { _commands.Add(command); }
    public void Run( Receiver receiver) {
        foreach(ICommand<Receiver> command in _commands) {
            command.Execute(receiver);
        }
    }
}

public class TestCommand
{
	public void Execute()
	{
		Invoker< CommandReceiver> macro = new Invoker< CommandReceiver>();

		macro.Add( new Operation( "First"));
        macro.Add(new Operation("Second"));
        macro.Run( new CommandReceiver());
	}
}

