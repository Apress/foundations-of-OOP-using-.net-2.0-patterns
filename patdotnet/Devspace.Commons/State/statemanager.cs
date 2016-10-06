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

using Devspace.Commons.Functors;
using Devspace.Commons.Automaters;
using System.Threading;
using System.Collections.Generic;

namespace Devspace.Commons.State {
    public interface ILock {
        void Acquire();
        void Release();
    }
    
    class StateManagerLock : ILock {
        public void Acquire() {
            Monitor.Enter( this);
        }
        public void Release() {
            Monitor.Exit( this);
        }
    }
    class Rung {
        public readonly DelegatePredicate< ISubject> _predicates;
        public readonly DelegateClosure< IExtendedSubject> _executor;
        
        public Rung( DelegatePredicate< ISubject> predicate, DelegateClosure< IExtendedSubject> executor) {
            _predicates = predicate;
            _executor = executor;
        }
    }
    
    public class StateEngine : StateManager {
        private ICollection< Rung> _rungs;
        private ReaderWriterLock _lockRung = new ReaderWriterLock();
        private DelegateClosure< IExtendedSubject> _clientExecutor;
        private ReaderWriterLock _lockExecutor = new ReaderWriterLock();
        private Queue< IExtendedSubject> _toBePredicated;
        private Queue< IExtendedSubject> _toBeExecuted;
        
        public StateEngine( IState[] states, int initial) : base( states, initial) {
        }

        public void AddContext( IExtendedSubject context) {
            IExtendedSubject newContext = new SynchronizedSubject( context);
            context.AddExtension<ILock>( new StateManagerLock());
            context.AddExtension<IStateCallback>( this);
            lock( _toBePredicated) {
                _toBePredicated.Enqueue( context);
            }
        }
        public void AddRung( DelegatePredicate< ISubject> predicates, DelegateClosure< IExtendedSubject> executor) {
            try {
                _lockRung.AcquireWriterLock( -1);
                _rungs.Add( new Rung( predicates, executor));
            }
            finally {
                _lockRung.ReleaseLock();
            }
        }
        public void AssignExecutor( DelegateClosure< IExtendedSubject> executor) {
            try {
                _lockExecutor.AcquireWriterLock( -1);
                _clientExecutor = executor;
            }
            finally {
                _lockExecutor.ReleaseLock();
            }
        }
        public bool KeepExecuting {
            get {
                return true;
            }
        }
        public int QueuedSleep {
            get {
                return 0;
            }
        }
        // **********************************************************************
        private void ProcessQueuedPredicate( Object obj) {
            IExtendedSubject context = (IExtendedSubject)obj;
            try {
                _lockRung.AcquireReaderLock( -1);
                foreach( Rung rung in _rungs) {
                    if( rung._predicates( context)) {
                        rung._executor( context);
                    }
                }
            }
            finally {
                _lockRung.ReleaseLock();
            }
        }
        private void RunPredicateQueue() {
            while( KeepExecuting) {
                lock( _toBePredicated) {
                    if( _toBePredicated.Count > 0) {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback( ProcessQueuedPredicate),
                            _toBePredicated.Dequeue());
                    }
                }
                Thread.Sleep( QueuedSleep);
            }
        }
        // **********************************************************************
        private void ProcessQueuedStates( Object stateInfo) {
            IExtendedSubject state = (IExtendedSubject)stateInfo;
            try {
                _lockExecutor.AcquireReaderLock( -1);
                _clientExecutor( state);
            }
            finally {
                _lockExecutor.ReleaseLock();
            }
        }
        private void RunExecutedQueue() {
            while( KeepExecuting) {
                lock( _toBeExecuted) {
                    if( _toBeExecuted.Count > 0) {
                        ThreadPool.QueueUserWorkItem(
                            new WaitCallback( ProcessQueuedPredicate),
                            _toBeExecuted.Dequeue());
                    }
                }
            }
        }
    }
    
}
