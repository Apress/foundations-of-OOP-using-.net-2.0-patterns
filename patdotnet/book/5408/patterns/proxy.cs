using System;


public interface IOperation
{
	void DoSomething();
}

public class SampleImplementation : IOperation
{
	public SampleImplementation()
	{
	}

	public void DoSomething()
	{
		Console.WriteLine( "Do Something");
	}
}


public class ProxyImplementation : IOperation
{
	SampleImplementation _impl;

	public ProxyImplementation()
	{
		_impl = new SampleImplementation();
	}

	public void DoSomething()
	{
		_impl.DoSomething();
	}
}

public class TestProxy
{
	public void Execute()
	{
		ProxyImplementation impl = new ProxyImplementation();

		impl.DoSomething();
	}
}