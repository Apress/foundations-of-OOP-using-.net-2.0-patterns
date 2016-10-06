using System;
using System.Threading;

public class Singleton
{
	private static int _value;
	private static Singleton _instance;

	public static Singleton Instance()
	{
		Monitor.Enter( _value);
		if( _instance == null)
		{
			_instance = new Singleton();
		}
		Monitor.Exit( _value);
		return _instance;
	}
	public void Output()
	{
		Console.WriteLine( "hello world");
	}
}

public class TestSingleton
{
	public void Execute()
	{
		Singleton.Instance().Output();
	}
}
