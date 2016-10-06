using System;

public interface ILightState {
    bool IsLightOn {
        get;
    }
    void TouchButton();
}


public class StateManager {
    private ILightState _onLight;
    private ILightState _offLight;
    private ILightState _current;

    public ILightState LightState {
        get { return _current; }
    }
    public void TouchButton() {
        if(_current.IsLightOn) {
            _current = _offLight;
        }
        else {
            _current = _onLight;
        }
    }
}

public class StateManager2 {
    private ILightState _onLight;
    private ILightState _offLight;
    private ILightState _current;

    public StateManager2() {
        _onLight = new LightStateOn(this);
        _offLight = new LightStateOff(this);
        _current = null;
    }

    public ILightState LightState {
        get { return _current; }
        set { _current = value; }
    }

    public ILightState this[string state] {
        get {
            if(String.Compare("on", state) == 0) {
                return _onLight;
            }
            else if(String.Compare("off", state) == 0) {
                return _offLight;
            }
            throw new NotSupportedException();
        }
    }
}

public class LightStateOn : ILightState {
    StateManager2 _parent;

    public LightStateOn(StateManager2 parent) {
        _parent = parent;
    }

    public bool IsLightOn {
        get { return true; }
    }

    public void TouchButton() {
        _parent.LightState = _parent["off"];
    }
}

public class LightStateOff : ILightState {
    StateManager2 _parent;

    public LightStateOff(StateManager2 parent) {
        _parent = parent;
    }

    public bool IsLightOn {
        get { return false; }
    }

    public void TouchButton() {
        _parent.LightState = _parent["on"];
    }
}

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

	private GermanState _german = new GermanState();
	private EnglishState _english = new EnglishState();
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

	public void Translate()
	{
		_currState.DoSomeAction();
	}
}

interface IStrategy { }

class ConcreateStrategyA: IStrategy { }
class ConcreateStrategyB: IStrategy { }

class Algorithm {
    private IStrategy _strategy;

    public void SetStrategy(IStrategy strategy) {
        _strategy = strategy;
    }

    public void ApplicationLogic() {
        // do something with _strategy
    }
}

public class TestState
{
	public void Execute()
	{
		RunningApplication app = new RunningApplication();
		app.SetState( RunningApplication.LanguageState.German);
		//app.DoSomeNeutralAction();
	}
}