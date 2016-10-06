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

namespace Devspace.Commons.Mediator {

    public interface IMediator1ToN< objtype1, objtype2> : ICollection< objtype2> where objtype1 : class where objtype2: class {
        void AssignParent( objtype1 type);
    }
    
    public class ReferenceMediator1ToN< objtype1, objtype2> :
        IMediator1ToN< objtype1, objtype2> where objtype1 : class where objtype2: class {
        private objtype1 _parent;
        private ICollection< objtype2> _children = new List< objtype2>();
        
        public ReferenceMediator1ToN() {
        }

        public ReferenceMediator1ToN( objtype1 parent, objtype2[] children) {
            _parent = parent;
            foreach(objtype2 obj in children) {
                _children.Add(obj);
            }
        }

        public virtual void AssignParent(objtype1 parent) {
            _parent = parent;
        }

        #region IEnumerable implementations
        public IEnumerator<objtype2> GetEnumerator() {
            return _children.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _children.GetEnumerator();
        }

        public override int GetHashCode() {
            return _parent.GetHashCode();
        }
        #endregion

        public virtual void Add(objtype2 item) {
            if(!Contains(item)) {
                _children.Add(item);
            }
        }
        
        public virtual void Clear() {
            _children.Clear();
        }
        
        public virtual bool Contains(objtype2 item) {
            foreach(objtype2 child in _children) {
                if(Object.ReferenceEquals(child, item)) {
                    return true;
                }
            }
            return false;
        }
        
        public virtual void CopyTo(objtype2[] array, int arrayIndex) {
            _children.CopyTo( array, arrayIndex);
        }
        
        public virtual bool Remove(objtype2 item) {
            bool retval = false;
            // May seem silly to do copy like this, but using RemoveAt does the same thing
            IList< objtype2> newList = new List< objtype2>();
            foreach( objtype2 element in _children) {
                if( !Object.ReferenceEquals( item, element)) {
                    newList.Add( element);
                    retval = true;
                }
            }
            _children = newList;
            return retval;
        }
        
        public virtual bool IsReadOnly {
            get {
                return false;
            }
        }
        
        public virtual int Count {
            get {
                return _children.Count;
            }
        }
    }
    
    public interface IMediatorNtoN<objtype1, objtype2>
        where objtype1 : class where objtype2: class {
        IMediator1ToN<objtype1, objtype2> Get1ToNType1( objtype1 type1);
        IMediator1ToN<objtype2, objtype1> Get1ToNType2( objtype2 type2);
    }
        
    public class ReferenceMediatorNtoN< objtype1, objtype2> : IMediatorNtoN< objtype1, objtype2>
        where objtype1 : class where objtype2 : class {
        
        class Proxy< objtype1, objtype2> : ReferenceMediator1ToN< objtype1, objtype2>
            where objtype1 : class where objtype2 : class {
            private ReferenceMediator1ToN< objtype2, objtype1> _otherList;
            
            public Proxy( ) {
            }
            public void AssignOtherList( ReferenceMediator1ToN< objtype2, objtype1> otherList) {
                _otherList = otherList;
            }
            
            public override void AssignParent( objtype1 parent) {
                base.AssignParent( parent);
            }
        }
        
        private Proxy< objtype1, objtype2> _listType1;
        private Proxy< objtype2, objtype1> _listType2;

        public ReferenceMediatorNtoN() {
            _listType1 = new Proxy< objtype1, objtype2>();
            _listType2 = new Proxy< objtype2, objtype1>();
            _listType1.AssignOtherList( _listType2);
            _listType2.AssignOtherList( _listType1);
        }
        public virtual IMediator1ToN<objtype1, objtype2> Get1ToNType1(objtype1 type1) {
            return _listType1 as IMediator1ToN<objtype1, objtype2>;
        }
        public virtual IMediator1ToN<objtype2, objtype1> Get1ToNType2(objtype2 type2) {
            return _listType2 as IMediator1ToN<objtype2, objtype1>;
        }
        
    }
}

