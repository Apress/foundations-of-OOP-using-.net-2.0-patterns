using System;

// *****************************************************************
public interface NewInterface
{
	void NewOutput();
}


public class OldClass
{
	public void Output()
	{
		System.Console.WriteLine( "Old class implementation");
	}
}

public class AdaptorWithInheritance : OldClass, NewInterface
{
	public void NewOutput()
	{
		Output();
	}
}

public class AdaptorWithEncapsulation : NewInterface
{
	private OldClass _cls;

	public AdaptorWithEncapsulation()
	{
		_cls = new OldClass();
	}

	public void NewOutput()
	{
		_cls.Output();
	}
}

// *****************************************************************
public class TestAdaptor
{
	public void Execute()
	{
		AdaptorWithInheritance cls1 = new AdaptorWithInheritance();
		AdaptorWithEncapsulation cls2 = new AdaptorWithEncapsulation();

		cls1.NewOutput();
		cls2.NewOutput();
	}
}
