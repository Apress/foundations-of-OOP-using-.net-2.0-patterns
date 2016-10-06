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

using Devspace.Commons.Functors;
using Devspace.Commons.TDD;

namespace Devspace.Commons.Collections {
    public abstract class BaseProxy< type> : IList< type> {
        protected IList< type> _parent;

        public BaseProxy( IList<type> parent) {
            _parent = parent;
        }

        #region IList<type> Members
        public virtual int  IndexOf(type item) {
            return _parent.IndexOf( item);
        }

        public virtual void Insert(int index, type item) {
            _parent.Insert( index, item);
        }

        public virtual void  RemoveAt(int index) {
            _parent.RemoveAt( index);
        }
        public virtual type  this[int index] {
            get {
                return _parent[ index];
            }
            set {
                _parent[ index] = value;
            }
        }
        #endregion

        #region ICollection<type> Members
        public virtual void  Add(type item) {
            _parent.Add( item);
        }
        public virtual void  Clear() {
            _parent.Clear();
        }
        public virtual bool Contains(type item) {
            return _parent.Contains( item);
        }
        public virtual void  CopyTo(type[] array, int arrayIndex) {
            _parent.CopyTo( array, arrayIndex);
        }
        public virtual int  Count {
            get {
                return _parent.Count;
            }
        }
        public virtual bool  IsReadOnly {
            get {
                return _parent.IsReadOnly;
            }
        }
        public virtual bool  Remove(type item) {
            return _parent.Remove( item);
        }
        #endregion

        #region IEnumerable<type> Members
        public virtual IEnumerator<type>  GetEnumerator() {
            return _parent.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _parent.GetEnumerator();
        }
        #endregion
        
        #region ToString implementation
        public override string ToString() {
            MemoryTracer.Start(this);
            MemoryTracer.StartArray("collection");
            foreach(type value in _parent) {
                MemoryTracer.Embedded(value.ToString());
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
        #endregion
    }

    
    public class NullEnumerator< type> : IEnumerator< type> {
        public bool MoveNext() {
            return false;
        }
        public void Reset() {
        }
        public void Dispose() {
        }
        public type Current {
            get {
                return default( type);
            }
        }
        object System.Collections.IEnumerator.Current {
            get {
                return default( type);
            }
        }
        
    }
    
    public class NullIterator< type> : IEnumerable< type> {
        public IEnumerator<type> GetEnumerator() {
            return new NullEnumerator< type>();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return new NullEnumerator< type>();
        }
    }
}



