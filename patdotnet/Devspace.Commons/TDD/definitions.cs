#region devspace.com Copyright (c) 2005, Christian Gross
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
#endregion
using System;

namespace Devspace.Commons.TDD {
    public class TracerException : Exception {
        private string _message;

        public TracerException( string message) {
            _message = message;
        }

        public override string Message {
            get {
                return _message;
            }
        }
    }

    public class ApplicationFatalException : Exception {
        public ApplicationFatalException( string message) : base( message) {
            Logging.Fatal( message);
        }
    }

    public class ApplicationNonFatalException : ApplicationException {
        public ApplicationNonFatalException( string message) : base( message) {
            Logging.Error( message);
        }
    }

    public delegate Exception InstantiateException();


    internal class IgnoreException : ApplicationException {
         public IgnoreException() :
             base( "Ignore Exception") {
         }
         public static Exception Instantiate() {
             return new IgnoreException();
         }
     }
     internal class UnknownException : Exception {
         public UnknownException() :
             base( "Unknown Error") {

         }
         public static Exception Instantiate() {
             return new UnknownException();
         }
     }
     internal class DoNoUseAssertEqualsException : Exception {
         public DoNoUseAssertEqualsException() :
             base( "Assert.Equals should not be used for Assertions") {
         }
         public static Exception Instantiate() {
             return new DoNoUseAssertEqualsException();
         }
     }

     internal class MultiDimensionNotSupportedException : ApplicationException {
         public MultiDimensionNotSupportedException() :
             base( "Multi-dimension array comparison is not supported") {
         }
         public static Exception Instantiate() {
             return new MultiDimensionNotSupportedException();
         }
     }

     internal class DoNotUseException : Exception {
         public DoNotUseException() :
             base( "Functionality is not implemented and should not be used") {
         }
         public static Exception Instantiate() {
             return new MultiDimensionNotSupportedException();
         }
     }



     public class ExceptionImpl {
         public static Exception ExceptionIgnore {
             get {
                 return new IgnoreException();
             }
         }

         public static Exception ExceptionUnknown {
             get {
                 return new UnknownException();
             }
         }

         public static Exception ExceptionDoNotUseAssert {
             get {
                 return new DoNoUseAssertEqualsException();
             }
         }

         public static Exception ExceptionMultiDimensionNotSupported {
             get {
                 return new MultiDimensionNotSupportedException();
             }
         }

     }

}


