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
using System.Collections.Generic;

namespace Devspace.Commons.Pool {
    
    //
    // TODO: Add Dynamic Extension support for constructors
    //
    
    public class GenericObjectPool<type> : IObjectPool< type> {
        Stack< type> _availableObjects;
        
        int _counter;
        bool _allowRegistrations;
        
        public GenericObjectPool() {
            _availableObjects = new Stack< type>();
            _allowRegistrations = true;
        }
        ~GenericObjectPool() {
            lock( this) {
                _allowRegistrations = false;
            }
            Console.WriteLine( "Destroy GenericObjectPool");
        }
        IPoolableObjectFactory< type> _factory;
        /// <summary>
        /// Method GetNumIdle
        /// </summary>
        /// <returns>An int</returns>
        public virtual int NumIdle {
            get {
                lock( this) {
                    return _availableObjects.Count;
                }
            }
        }
        
        /// <summary>
        /// Method GetNumActive
        /// </summary>
        /// <returns>An int</returns>
        public virtual int NumActive {
            get {
                lock( this) {
                    return _counter - _availableObjects.Count;
                }
            }
        }
        
        /// <summary>
        /// Method GetObject
        /// </summary>
        /// <returns>A type</returns>
        public virtual type GetObject() {
            lock( this) {
                type retval;
                if( _availableObjects.Count == 0) {
                    retval = _factory.MakeObject( this);
                    _counter ++;
                }
                else {
                    retval = _availableObjects.Pop();
                }
                _factory.ActivateObject( retval);
                return retval;
            }
        }
        
        /// <summary>
        /// Method ReturnObject
        /// </summary>
        /// <param name="obj">A  type</param>
        public bool ReturnObject(type obj) {
            lock( this) {
                if( _allowRegistrations) {
                    Console.WriteLine( "GenericObjectPool.ReturnObject (" + obj.ToString() + ")");
                    _availableObjects.Push( obj);
                    _factory.PassivateObject( obj);
                    GC.ReRegisterForFinalize( obj);
                    return true;
                }
                else {
                    Console.WriteLine( "GenericObjectPool.ReturnObject is inactive");
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Method SetFactory
        /// </summary>
        /// <param name="factory">An IPoolableObjectFactory<type></param>
        public void SetFactory(IPoolableObjectFactory<type> factory) {
            _factory = factory;
        }
    }


    /*public class GenericObjectPool<type> : IObjectPool< type> {
        class Internal< type> {
            type _managed;
            
            public Internal( type managed) {
                
            }
            
            public type ManagedReference {
                get {
                    return _managed;
                }
            }
        }
        IList< Internal< type>> _pool;
        Stack< Internal< type>> _inactive;
        
        IPoolableObjectFactory< type> _factory;
        
        public GenericObjectPool( IPoolableObjectFactory< type> factory) {
            _factory = factory;
        }
        /// <summary>
        /// Method BorrowObject
        /// </summary>
        /// <returns>A type</returns>
        public virtual type BorrowObject() {
            lock( this) {
                if( _inactive.Count == 0) {
                    type obj = _factory.MakeObject();
                    _factory.ActivateObject( obj);
                    //_active.Push( new Internal< type>( obj));
                    return obj;
                }
                else {
                    Internal< type> obj = _inactive.Pop();
                    _factory.ActivateObject( obj.ManagedReference);
                    //_active.Push( obj);
                    return obj.ManagedReference;
                }
            }
        }
        
        /// <summary>
        /// Method ReturnObject
        /// </summary>
        /// <param name="obj">A  type</param>
        public void ReturnObject(type obj) {
            
            // TODO
        }
        
        /// <summary>
        /// Method InvalidateObject
        /// </summary>
        /// <param name="obj">A  type</param>
        public void InvalidateObject(type obj) {
            // TODO
        }
        
        /// <summary>
        /// Method PreCreateObject
        /// </summary>
        public void PreCreateObject() {
            // TODO
        }
        
        /// <summary>
        /// Method GetNumIdle
        /// </summary>
        /// <returns>An int</returns>
        public int GetNumIdle() {
            // TODO
            return 0;
        }
        
        /// <summary>
        /// Method GetNumActive
        /// </summary>
        /// <returns>An int</returns>
        public int GetNumActive() {
            // TODO
            return 0;
        }
        
        /// <summary>
        /// Method Clear
        /// </summary>
        public void Clear() {
            // TODO
        }
        
        /// <summary>
        /// Method Close
        /// </summary>
        public void Close() {
            // TODO
        }
        
        /// <summary>
        /// Method SetFactory
        /// </summary>
        /// <param name="factory">An IPoolableObjectFactory<type></param>
        public void SetFactory(IPoolableObjectFactory<type> factory) {
            // TODO
        }
        
        
    }
    */
    
}


