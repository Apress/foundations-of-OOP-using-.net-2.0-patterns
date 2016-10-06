using System;
using System.Threading;

public class SimpleThread
{
	public void Process()
	{
		lock( typeof(SimpleThread))
		{
			Console.WriteLine( "Hello from thread");
		}
	}
}


public class TestSimpleThread
{
	public void Execute()
	{
		SimpleThread simple = new SimpleThread();
		Thread thrd1 = new Thread( new ThreadStart( simple.Process));
		thrd1.Start();

		Thread thrd2 = new Thread( new ThreadStart( simple.Process));
		thrd2.Start();
	}
}
