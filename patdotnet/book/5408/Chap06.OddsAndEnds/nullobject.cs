using System;

namespace Devspace.NullObject {

    #region Null Strategy definition
    interface IStrategy {
        void Method();
    }

    class NullStrategy: IStrategy {
        public void Method() {
            return;
        }
    }

    class Algorithm {
        private IStrategy _strategy;

        public void SetStrategy(IStrategy strategy) {
            _strategy = strategy;
        }

        public void ApplicationLogic() {
            // do something with _strategy
        }
    }
    #endregion

    
    #region Null Adapter / Decorator definition
    #region Null Object definition
    interface IBaseClass {
        int Value { get; set; }
        void Assign();
    }
    /*
    class NullObject: IBaseClass {
        public int Value {
            get {
                return 0;
            }
            set {
                return;
            }
        }

        public void Assign() {
            return;
        }
        public override bool Equals(object obj) {
            NullObject obj = obj as NullObject;
            if(obj.Value == 0) {
                return true;
            }
            return false;
        }
        public override string ToString() {
            return "NullObject : IBaseClass";
        }
    }
     */
    #endregion

    /*
    interface IAdapter {
        void Method();
        IBaseClass ExpectReturn();
    }
    class NullAdapter: IAdapter {
        public void Method() {
        }
        public IBaseClass ExpectReturn() {
            return new NullObject();
        }
    }
     */
    #endregion

    #region Null State definition
    public interface ILightState {
        bool IsLightOn {
            get;
        }
    }

    /*
    class NullLightState : ILightState {
        bool IsLightOn {
            get {
                return false;
            }
        }
    }
    public class StateManager {
        private ILightState _state = new NullLightState();

        public ILightState LightState {
            get { return _state; }
        }
        public void TouchButton() {
        }
    }*/
    #endregion

    #region Null Proxy definition
    interface IList {
        void Add();
    }
    class NullProxy : IList {
        public void Add() {
            throw new NotImplementedException();
        }
    }
    #endregion
     
}