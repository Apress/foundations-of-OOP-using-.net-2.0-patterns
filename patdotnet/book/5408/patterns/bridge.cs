using System;

// **********************************************************************
interface InterfaceAbstraction
{
	void Method();
}

class ImplementationOfInterface : InterfaceAbstraction
{
	public void Method()
	{
		Console.WriteLine( "ImplementationOfInterface::Method");
	}
}

// **********************************************************************
class Implementation 
{
	public virtual void DoStringOp(string str)
	{
		Console.WriteLine("Standard implementation - print string as is");
		Console.WriteLine("string = {0}", str);
	}		
}

abstract class DesignedBaseFunctionality
{
	protected Implementation _impToUse;

	public void SetImplementation(Implementation i)
	{
		_impToUse = i;
	}

	virtual public void DumpString(string str)
	{
		_impToUse.DoStringOp(str);				   
	}
}

class DerivedAbstraction : DesignedBaseFunctionality
{
	override public void DumpString(string str)
	{
		str += ".com";
		_impToUse.DoStringOp(str);			
	}		
}

class AnImplementation : Implementation 
{
	override public void DoStringOp(string str)
	{
		Console.WriteLine("AnImplementation - don't print string");
	}	
}

class AnotherImplementation : Implementation 
{
	override public void DoStringOp(string str)
	{
		Console.WriteLine("AnotherImplementation - print string twice");
		Console.WriteLine("string = {0}", str);
		Console.WriteLine("string = {0}", str);
	}	
}


public class TestBridge
{
	public void Execute()
	{
		InterfaceAbstraction impl = new ImplementationOfInterface();
		//InterfaceAbstraction impl2 = FactoryImplementationOfInstance.createInstance();
		impl.Method();

		DesignedBaseFunctionality baseFunc = new DerivedAbstraction();

		baseFunc.SetImplementation( new AnotherImplementation());

		baseFunc.DumpString( "hello world");

	}
}
