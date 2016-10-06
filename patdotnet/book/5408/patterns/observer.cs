using System;

public interface IParentDataSource
{
	object MyHandle { get; set; }
	int Value { get; set; }
}

public delegate void PluginHandler( IParentDataSource src, int extraInformation);


public class ParentDataSource : IParentDataSource
{
	private int _value;

	public int Value 
	{
		get { return _value; }
		set { _value = value; }
	}
}


public class Parent
{
	private PluginHandler _plugin;

	public void AddHandler( PluginHandler plugin)
	{
		if( plugin == null)
		{
			_plugin = plugin;
		}
		else
		{
			_plugin += plugin;
		}
	}

	public void SendMessage( IParentDataSource src)
	{
		_plugin( src, 10);
	}
}

public class PluginImplementation
{
	public void PluginHandler( IParentDataSource src, int extraInformation)
	{
		Console.WriteLine( "Caught a message");
	}
}

public class TestObserver
{
	public void Execute()
	{
		Parent parent = new Parent();
		PluginImplementation impl = new PluginImplementation();
		ParentDataSource src = new ParentDataSource();

		parent.AddHandler( new PluginHandler( impl.PluginHandler));
		parent.SendMessage( src);
	}
}
