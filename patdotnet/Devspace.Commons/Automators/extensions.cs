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
using System.Collections;
using System.Collections.Generic;
using Devspace.Commons.TDD;
using System.Reflection;

namespace Devspace.Commons.Automaters {
    public interface ISubject {
        // FIXME: Remove Mono hack
#if MONO
        type GetExtension< type>();
#else
        type GetExtension< type>() where type: class;
#endif
    }
    public interface IExtendedSubject : ISubject {
        void AddExtension< type>( type extension) where type: class;
        void RemoveExtension< type>( type extension) where type: class;
    }

    public class SynchronizedSubject : IExtendedSubject {
        IExtendedSubject _parent;
        
        public SynchronizedSubject() {
            _parent = new Subject();
        }
        public SynchronizedSubject( IExtendedSubject parent) {
            _parent = parent;
        }
        
        // FIXME: Remove Mono hack
#if MONO
        public type GetExtension<type> () {
#else
        public type GetExtension<type> () where type: class {
#endif
            lock( this) {
                return _parent.GetExtension< type>();
            }
        }
        
        public void AddExtension<type> (type extension) where type: class{
            lock( this) {
                _parent.AddExtension< type>( extension);
            }
        }
        
        public void RemoveExtension<type> (type extension) where type: class{
            lock( this) {
                _parent.RemoveExtension<type>( extension);
            }
        }
        public override string ToString() {
            MemoryTracer.Start( this);
            MemoryTracer.Embedded( _parent.ToString());
            return MemoryTracer.End();
        }
    }
    
    
    public sealed class Subject : IExtendedSubject {
        bool InterfaceFilter(Type typeObj, Object criteriaObj) {
            return (typeObj.ToString() == criteriaObj.ToString());
        }
        bool DidImplement(string @interface, Type targetType) {
            TypeFilter interfaceFilter = new TypeFilter(InterfaceFilter);
            Type[] interfaces = targetType.FindInterfaces( interfaceFilter, @interface);
            return (interfaces.Length > 0);
        }
        
        
        ArrayList _list = new ArrayList();
        
        // FIXME: Remove Mono hack
#if MONO
        public type GetExtension< type>() {
#else
        public type GetExtension< type>() where type: class{
#endif
            foreach( Object obj in _list) {
                if( DidImplement( typeof( type).FullName, obj.GetType())) {
                    // FIXME: Remove Mono hack
#if MONO
                    return (type)obj;
#else
                    return obj as type;
#endif
                }
            }
            // FIXME: Remove Mono hack, as this one is bad because it changes the semantics
#if MONO
            throw new NotSupportedException();
#else
            return null;
#endif
        }
        public void AddExtension< type>( type extension) where type: class{
            if( extension == null) {
                throw new NotSupportedException();
            }
            _list.Add( extension);
        }
        public void RemoveExtension< type>( type extension) where type: class{
            for( int c1 = 0; c1 < _list.Count; c1 ++) {
                if( Object.ReferenceEquals( _list[ c1], extension)) {
                    _list.RemoveAt( c1);
                }
            }
        }
        public static IExtendedSubject Synchronized() {
            return new SynchronizedSubject();
        }
        public override string ToString() {
            MemoryTracer.Start( this);
            MemoryTracer.StartArray( "Extension Objects");
            foreach( Object obj in _list) {
                MemoryTracer.Variable( "type", obj.GetType().FullName);
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }
    
}
