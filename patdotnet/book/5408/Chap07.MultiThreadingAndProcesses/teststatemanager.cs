using System;
using Devspace.Commons.State;
using NUnit.Framework;
using Devspace.Commons.Automaters;
/*
public interface ILightState {
    bool IsLightOn {
        get;
    }
    void TouchButton();
}

abstract class BaseLightState : IState {
    protected IStateCallback _manager;
    protected string _descriptor;
    
    public type GetDescriptor<type> () {
        return _descriptor as type;
    }
    
    public bool IsMatch<type> (type searchidentifier) {
        string match = searchidentifier as string;
        if( String.Compare( _descriptor, match) == 0) {
            return true;
        }
        return false;
    }
    public void AssignParent(IStateCallback mgr) {
        _manager = mgr;
    }
}

public class LightStateOn : BaseLightState, ILightState {
    public bool IsLightOn {
        get { return true; }
    }
    public void TouchButton() {
    }
}

public class LightStateOff : BaseLightState, ILightState {
    public bool IsLightOn {
        get { return false; }
    }
    public void TouchButton() {
    }
}

class Room {
    ILightState _light;
    
}

class Person {
    //public void EnterRoom
}

class PredicatesAndExecutors {
    public static bool DidEnterRoom( IExtendedSubject obj) {
        return false;
    }
    public static void ExecutorEnterRoom( IExtendedSubject obj) {
        
    }
    public static bool DidLeaveRoom( IExtendedSubject obj) {
        
    }
    public void ExecutorLeaveRoom( IExtendedSubject obj) {
        
    }
}

[TestFixture]
public class TestStateManager {
    [Test]
    public void TestSimple() {
        StateEngine engine = new StateEngine( new IState[] {
            new LightStateOn(), new LightStateOff()}, 0);
        engine.AddContext(
        
    }
}

*/


