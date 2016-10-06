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

namespace Devspace {
namespace Commons {
namespace Loader {
    public class TypeList : MarshalByRefObject, IEnumerable {
        private ArrayList _types = new ArrayList();

        public TypeList() {

        }

    #region IEnumerable Members

        public IEnumerator GetEnumerator() {
            return _types.GetEnumerator();
        }

    #endregion

        public Type this[ int index] {
            get {
                return (Type)_types[ index];
            }
        }
        public void Add(Type element) {
            _types.Add(element);
        }

        public int Count {
            get {
                return _types.Count;
            }
        }
        public void Remove( Type obj) {
            _types.Remove( obj);
        }
        public void Clear() {
            _types.Clear();
        }
        public override String ToString() {
            String buffer;

            buffer = "Element Count (" + _types.Count + ")\n";
            foreach( Type type in _types) {
                buffer += "(\n";
                buffer += "  (Name " + type.Name + ")\n";
                buffer += "  (Assembly Qualified Name " + type.AssemblyQualifiedName + ")\n";
                buffer += "  (Assembly Full Name " + type.Assembly.FullName + ")\n";
                buffer += "  (Assembly Code Base" + type.Assembly.CodeBase + ")\n";
                buffer += ")\n";
            }
            return buffer;
        }
    }
}
}
}
