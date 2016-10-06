// one line to give the library's name and an idea of what it does.
// Copyright (C) 2005  Christian Gross devspace.com (christianhgross@yahoo.ca)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;

namespace Devspace.Commons.State {
    public interface IStateCallback {
        // FIXME: Remove Mono hack
#if MONO
        IState GetImplementation<type>( type descriptor);
#else
        IState GetImplementation<type>( type descriptor) where type: class;
#endif
        void AssignImplementation( IState state);
    }
    public interface IState {
        // FIXME: Remove Mono hack
        #if MONO
        type GetDescriptor< type>();
        bool IsMatch<type>( type searchidentifier);
        #else
        type GetDescriptor< type>() where type: class;
        bool IsMatch<type>( type searchidentifier) where type: class;
        #endif
        void AssignParent( IStateCallback mgr);
    }
    
    public class StateManager: IStateCallback {
        
        IState[] _states;
        IState _currentState;
        
        public StateManager( IState[] states, int initial) {
            _states = states;
            _currentState = _states[ initial];
            foreach( IState stateimpl in _states) {
                stateimpl.AssignParent( this);
            }
        }
        public IState CurrentState{
            get {
                return _currentState;
            }
            set {
                foreach( IState stateimpl in _states) {
                    if( Object.ReferenceEquals( _currentState, stateimpl)) {
                        _currentState = value;
                        return;
                    }
                }
                throw new NotSupportedException();
            }
        }
        // FIXME: Remove Mono hack
#if MONO
        public IState GetImplementation<type> (type descriptor) {
#else
        public IState GetImplementation<type> (type descriptor) where type: class{
#endif
            foreach( IState stateimpl in _states) {
                if( stateimpl.IsMatch( descriptor)) {
                    return stateimpl;
                }
            }
            throw new NullReferenceException();
        }
        public void AssignImplementation( IState state) {
            this.CurrentState = state;
        }
    }
}
