using System;

public class Example : ICloneable
{
	private int _value;
	private SampleClass _cls;

	public int Value
	{
		get 
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}
	public System.Object Clone()
	{
		Example retval = (Example)this.MemberwiseClone();
		return retval;
	}
}


public class CloneFactory
{
	public Example CreateObject( ICloneable obj)
	{
		return (Example)obj.Clone();
	}
}

public class TestPrototype
{
	public void Execute()
	{
		Example ex = new Example();
		ex.Value = 1023;

		CloneFactory cloner = new CloneFactory();

		Example ex2 = cloner.CreateObject( ex);
	}
}

