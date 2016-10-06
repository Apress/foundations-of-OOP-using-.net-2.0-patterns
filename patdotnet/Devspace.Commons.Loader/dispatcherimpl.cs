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
using System.Text;

namespace Devspace {
namespace Commons {
namespace Loader {
    public class Identifier {
        public const string ID_assembly = "assembly";
        public const string ID_type = "type";

        private Hashtable _propBag = new Hashtable();

        public Identifier( string identifier ) {
            _propBag[ ID_type ] = identifier;
        }
        public Identifier( string assembly, string identifier ) {
            _propBag[ ID_assembly ] = assembly;
            _propBag[ ID_type ] = identifier;
        }
        public Identifier() {
        }
        public string this[ string key ] {
            get {
                return (string)_propBag[ key ];
            }
            set {
                _propBag[ key ] = value;
            }
        }
        public bool DoesExist( string key ) {
            return _propBag.ContainsKey( key );
        }
    }
}
}
}

