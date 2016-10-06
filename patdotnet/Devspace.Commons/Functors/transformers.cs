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

namespace Devspace.Commons.Functors.Transformers {
    public delegate bool Transform< inpType, outType>( inpType input, out outType output);

    public class ChainOfResponsibilityTransformer< inpType, outType>{
        private Transform<inpType, outType> _transformer;

        public ChainOfResponsibilityTransformer() {
        }

        public void Add( Transform< inpType, outType> transformer) {
            _transformer += transformer;
        }
        public outType Transform(inpType input) {
            /*Delegate[] handlers = _handlers.GetInvocationList();

            for(int c1 = 0; c1 < handlers.Length; c1++) {
                if((bool)handlers[c1].DynamicInvoke(new Object[] { context })) {
                    return true;
                }
            }
            return false;
            */
            return default( outType);
        }
    }
}
