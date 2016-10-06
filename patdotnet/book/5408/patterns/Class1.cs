using System;

class Person 
{
	string name;
	public int age;
}

interface IMySerialize 
{
	Person getPerson( string identifier);
	void updateAddress( Person person);
	void marryPersons( Person person, Person person2);
}

class DoPersonDatabaseSerialization : IMySerialize
{
	public Person getPerson( string identifier) { return null; }
	public void updateAddress( Person person) { return; }
	public void marryPersons( Person person, Person person2) { return; }
}

class DoOperation
{
	void DoInitialMortage( IMySerialize serialize, Person person)
	{
		if( person.age > 18)
		{
			serialize.updateAddress( person);
		}
	}
}

namespace Patterns
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			TestBridge cls1 = new TestBridge();
			cls1.Execute();

			TestFactory cls5 = new TestFactory();
			cls5.Execute();

			TestBuilder cls15 = new TestBuilder();
			cls15.Execute();
	
			TestPrototype cls9 = new TestPrototype();
			cls9.Execute();
	
			TestObserver cls7 = new TestObserver();
			cls7.Execute();
	
			TestAdaptor cls = new TestAdaptor();
			cls.Execute();

			TestDecorater cls4 = new TestDecorater();
			cls4.Execute();
	
			TestComposite cls3 = new TestComposite();
			cls3.Execute();

			TestProxy cls10 = new TestProxy();
			cls10.Execute();
	
			TestIterator cls6 = new TestIterator();
			cls6.Execute();
	
			TestState cls14 = new TestState();
			cls14.Execute();

			TestCommand cls2 = new TestCommand();
			cls2.Execute();

			// ************
			TestProducerConsumer cls8 = new TestProducerConsumer();
			cls8.Execute();
	
			TestReaderWriter cls11 = new TestReaderWriter();
			cls11.Execute();
	
			TestSimpleThread cls12 = new TestSimpleThread();
			cls12.Execute();
	
			TestSingleton cls13 = new TestSingleton();
			cls13.Execute();

		}
	}
}
