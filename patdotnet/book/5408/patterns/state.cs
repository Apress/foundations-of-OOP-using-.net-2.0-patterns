using System;

public interface State
{
	void DoSomeAction();
}


class GermanState : State
{
	public void DoSomeAction()
	{
		Console.WriteLine( "Moin");
	}
}

class EnglishState : State
{
	public void DoSomeAction()
	{
		Console.WriteLine( "Hello");
	}
}


class RunningApplication
{
	public enum LanguageState 
	{
		German,
		English
	};

	private State _german = new GermanState();
	private State _english = new EnglishState();
	private State _currState;

	public void SetState( LanguageState state)
	{
		if( state == LanguageState.German)
		{
			_currState = _german;
		}
		else if( state == LanguageState.English)
		{
			_currState = _english;
		}
	}

	public void DoSomeNeutralAction()
	{
		_currState.DoSomeAction();
	}
}

public class TestState
{
	public void Execute()
	{
		RunningApplication app = new RunningApplication();
		app.SetState( RunningApplication.LanguageState.German);
		app.DoSomeNeutralAction();
	}
}