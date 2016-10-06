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

class ExampleClass : MarshalByRefObject, ExampleInterface {
    public void Message() {
        Console.WriteLine( "Hello from ExampleClass.Messagessssss");
    }
}

[Serializable]
public class AnotherClass {
    public void Method() {
        Console.WriteLine( "AnotherClass.Method");
    }
}

public class Factory : IFactory {
    public Object CreateInstance( string identifier) {
        if( String.Compare( identifier, "ExampleClass") == 0) {
            return (Object)(ExampleInterface)(new ExampleClass());
        }
        return null;
    }
}
