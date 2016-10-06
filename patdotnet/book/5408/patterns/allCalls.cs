using System;

namespace BestPractices
{

	public class AllCall
	{
		public void Test()
		{
			
		}
	}
}


/*
 using System;

public interface ISession
{
	String getDescription();
}

public class AppliedPatternsSession : ISession
{
	private String _description = "AppliedPatterns";

	public String getDescription()
	{
		return _description;
	}
}

public class AppliedPatternsSessionFactory 
{
	static public ISession createInstance()
	{
		return new AppliedPatternsSession();
	}
}

public interface SessionsFactory
{
	ISession createSession( String implementation);

	ISession createAppliedPatterns();
	ISession createJavaPatterns();
	ISession createCPPPatterns();
}


public class MainClass2
{
	public MainClass2()
	{
		//
		// TODO: Add Constructor Logic here
		//
	}

	///** @attribute System.STAThread() *
	public static void main(String[] args)
	{

		ISession sess = new AppliedPatternsSession();
		
		Meal meal = new Meal();

		meal.runIt();

		TestCommand cmd = new TestCommand();

		cmd.runIt();
	}
}
*/