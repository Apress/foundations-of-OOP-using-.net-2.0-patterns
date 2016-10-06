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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Runtime.Remoting;

namespace Devspace {
namespace Commons {
namespace Loader {
    public interface IResolver< Identifier> {
        void Load();
        void Unload();
        bool CanCreate( Identifier identifier);
        ObjectType CreateInstance<ObjectType>( Identifier identifier);
    }

    public abstract class Dispatcher< Identifier>{
        protected List<IResolver< Identifier>> _resolvers = new List<IResolver<Identifier>>();

        public abstract void Initialize();
        public abstract void Destroy();

        public void Load() {
            Initialize();
            foreach( IResolver<Identifier> resolver in _resolvers ) {
                resolver.Load();
            }
        }
        public void Unload() {
            foreach( IResolver<Identifier> resolver in _resolvers ) {
                resolver.Unload();
            }
            Destroy();
        }
        public ObjectType CreateInstance<ObjectType>( Identifier identifier) {
            foreach( IResolver<Identifier> resolver in _resolvers ) {
                if( resolver.CanCreate( identifier)) {
                    return resolver.CreateInstance<ObjectType>( identifier);
                }
            }
            return default(ObjectType);
        }
    }

}
}
}

