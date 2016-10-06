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
using Devspace.Commons.Collections;
using Devspace.Commons.Functors;
using System.Collections;
using System.Threading;

namespace Devspace.Commons.State {
    
    public interface ISingletonBuilder< type> where type: class {
        DelegatePredicate< type> IsValid {
            get;
        }
        DelegateTransformer< object, type> NewObject {
            get ;
        }
        int SleepTime {
            get;
        }
        bool KeepPolling {
            get;
        }
    }

    public abstract class BaseSingletonDelegation< type> : ISingletonBuilder<type> where type: class {
        
        protected virtual bool IsObjectValid( type obj) {
            throw new NotImplementedException();
        }
        
        protected virtual type InstantiateNewObject( object obj) {
            throw new NotImplementedException();
        }
        
        public virtual DelegatePredicate<type> IsValid {
            get {
                return new DelegatePredicate< type>( IsObjectValid);
            }
        }
        
        public virtual DelegateTransformer<object, type> NewObject {
            get {
                return new DelegateTransformer< object, type>( InstantiateNewObject);
            }
        }
        
        public virtual int SleepTime {
            get {
                return 1000;
            }
        }
        
        public virtual bool KeepPolling {
            get {
                return true;
            }
        }
    }
    
    public class Singleton< singletonupdate, type>
        where singletonupdate: ISingletonBuilder< type>, new()
        where type : class {
        private static Singleton< singletonupdate, type> _instance;
        
        private ISingletonBuilder< type> _builder;
        private type _data;
        private ReaderWriterLock _lock;
        
        Thread _thread;
        
        private Singleton() {
            _builder = new singletonupdate();
            _lock = new ReaderWriterLock();
            _thread = new Thread( new ThreadStart( SingletonThread));
            _thread.Start();
        }
        
        private void SingletonThread() {
            while( _builder.KeepPolling) {
                try {
                    _lock.AcquireReaderLock( -1);
                    if( _data != null && !_builder.IsValid( _data)) {
                        _lock.UpgradeToWriterLock( -1);
                        _data = null;
                    }
                    _lock.ReleaseLock();
                }
                finally {
                    _lock.ReleaseLock();
                }
                Thread.Sleep( _builder.SleepTime);
            }
        }
        public static type Instance( object obj) {
            if( _instance == null) {
                lock( typeof( Singleton< singletonupdate,type>)) {
                    if( _instance == null) {
                        _instance = new Singleton< singletonupdate, type>();
                    }
                }
            }
            type retval = null;
            try {
                _instance._lock.AcquireReaderLock( -1);
                if( _instance._data == null) {
                    _instance._lock.UpgradeToWriterLock(-1);
                    if( _instance._data == null) {
                        _instance._data = _instance._builder.NewObject( obj);
                    }
                }
                retval = _instance._data;
                _instance._lock.ReleaseLock();
            }
            finally {
                _instance._lock.ReleaseLock();
            }
            return retval;
        }
    }
    
    public class SingletonCollection< singletonupdate, type>
        where singletonupdate: ISingletonBuilder< type>, new()
        where type: class {
        private static SingletonCollection< singletonupdate, type> _instance;
        
        private ISingletonBuilder< type> _builder;
        private IFlyweightCollection< type> _collection;

        private SingletonCollection() {
            _builder = new singletonupdate();
            _collection = new SynchronizedFlyweightCollection< type>( _builder.NewObject);
        }
        
        private void SingletonThread() {
            while( _builder.KeepPolling) {
                try {
                    foreach( DictionaryEntry element in _collection) {
                        if( !_builder.IsValid((type)element.Value)) {
                            _collection.FlushItem(element.Key);
                        }
                    }
                }
                catch( InvalidOperationException ex) {
                    ;
                }
                Thread.Sleep( _builder.SleepTime);
            }
        }
        public static type Instance( object descriptor) {
            if( _instance == null) {
                lock( typeof( SingletonCollection< singletonupdate,type>)) {
                    if( _instance == null) {
                        _instance = new SingletonCollection< singletonupdate, type>();
                    }
                }
            }
            return _instance._collection.GetItem( descriptor);
        }
    }
}



