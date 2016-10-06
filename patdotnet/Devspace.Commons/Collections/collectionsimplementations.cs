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

using Devspace.Commons.Functors;
using Devspace.Commons.TDD;

namespace Devspace.Commons.Collections {
    public class TransformerProxy<inpType, outType>: BaseProxy<outType>, IList< inpType> {
        private DelegateTransformer<inpType, outType> _transformer;
        
        public TransformerProxy(IList<outType> parent, DelegateTransformer<inpType, outType> transformer)
        : base(parent) {
            _transformer = transformer;
        }
        
        #region IList<inpType> Members
        public int  IndexOf(inpType item) {
            return _parent.IndexOf( _transformer( item));
        }
        
        public void Insert(int index, inpType item) {
            _parent.Insert( index, _transformer( item));
        }
        #endregion
        
        #region ICollection<inpType> Members
        public void Add(inpType item) {
            _parent.Add( _transformer( item));
        }
        public bool Contains(inpType item) {
            return _parent.Contains(_transformer(item));
        }
        public bool Remove(inpType item) {
            return _parent.Contains(_transformer(item));
        }
        #endregion
        
        #region IList<inpType> Exception Members
        inpType IList<inpType>.this[int index] {
            get {
                throw new Exception("The method or operation is not implemented.");
            }
            set {
                throw new Exception("The method or operation is not implemented.");
            }
        }
        
        public void CopyTo(inpType[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }
        IEnumerator<inpType> IEnumerable<inpType>.GetEnumerator() {
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
        
        #region ToString implementation
        public override string ToString() {
            MemoryTracer.Start(this);
            //MemoryTracer.Variable("transformer", _transformer.ToString());
            MemoryTracer.Embedded(base.ToString());
            return MemoryTracer.End();
        }
        #endregion
    }
    
    public class PredicateProxy< type> : BaseProxy< type> {
        private DelegatePredicate<type> _predicate;
        
        public PredicateProxy(IList<type> parent, DelegatePredicate<type> predicate)
        : base(parent) {
            _predicate = predicate;
        }
        
        #region IList<type> Members
        public override void Insert(int index, type item) {
            if(_predicate(item)) {
                base.Insert(index, item);
            }
            else {
                throw new PredicateEvaluationException();
            }
        }
        
        public override type this[int index] {
            get {
                return base[index];
            }
            set {
                if(_predicate(value)) {
                    base[index] = value;
                }
                else {
                    throw new PredicateEvaluationException();
                }
            }
        }
        #endregion
        
        #region ICollection<type> Members
        public override void Add(type item) {
            if(_predicate(item)) {
                base.Add(item);
            }
            else {
                throw new PredicateEvaluationException();
            }
        }
        #endregion
        
        #region ToString implementation
        public override string ToString() {
            MemoryTracer.Start(this);
            //MemoryTracer.Variable("predicate", _predicate.ToString());
            MemoryTracer.Embedded( base.ToString());
            return MemoryTracer.End();
        }
        #endregion
    }
    
    public class ComparerProxy< type> : BaseProxy< type> {
        private DelegateComparer<type, type> _comparable;
        
        public ComparerProxy(IList<type> parent, DelegateComparer<type, type> comparable)
        : base(parent) {
            _comparable = comparable;
        }
        
        #region IList<type> Members
        public override void Insert(int index, type item) {
            foreach( type listItem in _parent) {
                if( _comparable( listItem, item) == 0) {
                    throw new ComparerEvaluationException();
                }
            }
            _parent.Insert(index, item);
        }
        
        public override type this[int index] {
            get {
                return base[index];
            }
            set {
                foreach( type listItem in _parent) {
                    if( _comparable( listItem, value) == 0) {
                        throw new ComparerEvaluationException();
                    }
                }
                _parent[ index] = value;
            }
        }
        #endregion
        
        #region ICollection<type> Members
        public override void Add(type item) {
            foreach( type listItem in _parent) {
                if( _comparable( listItem, item) == 0) {
                    throw new ComparerEvaluationException();
                }
            }
            _parent.Add( item);
        }
        #endregion
        
        #region ToString implementation
        public override string ToString() {
            MemoryTracer.Start( this);
            MemoryTracer.Embedded( base.ToString());
            return MemoryTracer.End();
        }
        #endregion
    }
    public class ClosureAddProxy<type>: BaseProxy<type> {
        private DelegateClosure<type> _closure;
        
        public ClosureAddProxy(IList<type> parent, DelegateClosure<type> closure) :
        base(parent) {
            _closure = closure;
        }
        
        #region IList<type> Members
        public override void Insert(int index, type item) {
            _closure(item);
            base.Insert(index, item);
        }
        
        public override type this[int index] {
            get {
                return base[index];
            }
            set {
                _closure(value);
                base[index] = value;
            }
        }
        #endregion
        
        #region ICollection<type> Members
        public override void Add(type item) {
            _closure(item);
            base.Add(item);
        }
        #endregion
        
        #region ToString implementation
        public override string ToString() {
            MemoryTracer.Start(this);
            MemoryTracer.Embedded(base.ToString());
            return MemoryTracer.End();
        }
        #endregion
    }
    public class ClosureRemoveProxy<type>: BaseProxy<type> {
        private DelegateClosure<type> _closure;
        
        public ClosureRemoveProxy(IList<type> parent, DelegateClosure<type> closure) :
        base(parent) {
            _closure = closure;
        }
        
        public override void  RemoveAt(int index) {
            _closure( base[ index]);
            base.RemoveAt( index);
        }
        public override bool  Remove(type item) {
            _closure( item);
            return base.Remove( item);
        }
        
        #region ToString implementation
        public override string ToString() {
            MemoryTracer.Start(this);
            MemoryTracer.Embedded(base.ToString());
            return MemoryTracer.End();
        }
        #endregion
    }
    
    public class ImplementedEqualsAndHashCode< type>: BaseProxy< type> {
        public ImplementedEqualsAndHashCode( IList<type> parent) : base(parent) {
            
        }
        // Another solution would be to get the Hashcode of the list, but
        // might not be entirely correct
        public override bool Equals( Object obj) {
            IList< type> list = (IList<type>)obj;
            if( list.Count != base.Count) {
                return false;
            }
            for( int c1 = 0; c1 < base.Count; c1 ++) {
                if( !list[ c1].Equals( base[ c1])) {
                    return false;
                }
            }
            return true;
        }
        public override int GetHashCode() {
            Devspace.Commons.Automaters.HashCodeAutomater hashcode = new Devspace.Commons.Automaters.HashCodeAutomater();
            foreach( type element in this) {
                hashcode.Append( element);
            }
            return hashcode.GetHashCode();
        }
    }
    
    public interface IFlyweightCollection< type> : IEnumerable {
        type GetItem( Object descriptor);
        void Flush();
        void FlushItem( Object descriptor);
    }
    
    public sealed class FlyweightCollection< type> : IFlyweightCollection< type> {
        
        DelegateTransformer< object, type> _transformer;
        Hashtable _collection = new Hashtable();

        public FlyweightCollection( DelegateTransformer< object, type> transformer) {
            _transformer = transformer;
        }
        
        public type GetItem( object descriptor) {
            type retval = (type)_collection[ descriptor];
            if( retval == null) {
                retval = _transformer( descriptor);
                _collection[ descriptor] = retval;
            }
            return retval;
        }
        
        public void Flush() {
            _collection = new Hashtable();
        }
        public void FlushItem( Object descriptor) {
            _collection[ descriptor] = null;
        }

        public IEnumerator GetEnumerator() {
            return _collection.GetEnumerator();
        }
    }
    
    public sealed class SynchronizedFlyweightCollection< type>:  IFlyweightCollection< type> {
        FlyweightCollection< type> _parent;
        
        public SynchronizedFlyweightCollection( DelegateTransformer< object, type> transformer) {
            _parent = new FlyweightCollection< type>( transformer);
        }
        
        public type GetItem( Object descriptor) {
            lock( this) {
                return _parent.GetItem( descriptor);
            }
        }
        
        public void Flush() {
            lock( this) {
                _parent.Flush();
            }
        }
        public void FlushItem( Object descriptor) {
            lock( this) {
                _parent.FlushItem( descriptor);
            }
        }
        public IEnumerator GetEnumerator() {
            return _parent.GetEnumerator();
        }
    }
}



/*        class FlyweightData< type, desctype> {
type _datatype;
desctype _descriptor;

public FlyweightData( type datatype, desctype descriptor) {
_datatype = datatype;
_descriptor = descriptor;
}
public type Data {
get {
return _datatype;
}
}
public desctype Descriptor {
get {
return _descriptor;
}
}
}
*/
/*ICollection< FlyweightData< type, desctype>> _collection;

public FlyweightCollection( DelegateTransformer< desctype, type> transformer) {
_transformer = transformer;
_collection = new List< FlyweightData< type, desctype>>();
}

public type GetItem( desctype descriptor) {
foreach( FlyweightData< type, desctype> item in _collection) {
if( Object.Equals( item.Descriptor, descriptor)) {
return item.Data;
}
}
type retval = _transformer( descriptor);
_collection.Add( new FlyweightData<type, desctype>( retval, descriptor));
return retval;
}*/


