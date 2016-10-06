#region Copyright (c) 2005, Christian Gross
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
// Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright © 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright © 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no
' event will the authors be held liable for any damages arising from the use of this
' software.
'
' Permission is granted to anyone to use this software for any purpose, including
' commercial applications, and to alter it and redistribute it freely, subject to the
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that
' you wrote the original software. If you use this software in a product, an
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright © 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright © 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

using System;
using System.Collections;
using System.ComponentModel;

//using Devspace.Commons.TDD;

namespace Devspace.Commons.TDD.Assert {
    /// <summary>
    /// A set of Assert methods
    /// </summary>
    public class Assert
    {
        /// <summary>
        /// A private constructor disallows any instances of this object.
        /// </summary>
        private Assert()
        {}

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void IsTrue(bool condition, InstantiateException code, params object[] args)
        {
            if (!condition)
                Assert.Fail(code, args);
        }
    
        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionInstantiateException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is false</param>
        static public void IsTrue(bool condition, InstantiateException code)
        {
            Assert.IsTrue(condition, code, null);
        }

        /// <summary>
        /// Asserts that a condition is true. If the condition is false the method throws
        /// an <see cref="AssertionInstantiateException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        static public void IsTrue(bool condition)
        {
            Assert.IsTrue(condition, new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionInstantiateException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is true</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void IsFalse(bool condition, InstantiateException code, params object[] args)
        {
            if (condition)
                Assert.Fail(code, args);
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionInstantiateException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        /// <param name="message">The message to display if the condition is true</param>
        static public void IsFalse(bool condition, InstantiateException code)
        {
            Assert.IsFalse( condition, code, null );
        }
        
        /// <summary>
        /// Asserts that a condition is false. If the condition is true the method throws
        /// an <see cref="AssertionInstantiateException"/>.
        /// </summary>
        /// <param name="condition">The evaluated condition</param>
        static public void IsFalse(bool condition)
        {
            Assert.IsFalse(condition, new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message that will be printed on failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreEqual(double expected,
            double actual, double delta, InstantiateException code, params object[] args)
        {
            // handle infinity specially since subtracting two infinite values gives
            // NaN and the following test fails. mono also needs NaN to be handled
            // specially although ms.net could use either method.
            if (double.IsInfinity(expected) || double.IsNaN(expected) || double.IsNaN(actual))
            {
                if (!(expected == actual))
                    Assert.FailNotEquals(expected, actual, code, args);
            }
            else if (!(Math.Abs(expected-actual) <= delta))
                Assert.FailNotEquals(expected, actual, code, args);
        }

        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message that will be printed on failure</param>
        static public void AreEqual(double expected,
            double actual, double delta, InstantiateException code)
        {
            Assert.AreEqual( expected, actual, delta, code, null );
        }

        /// <summary>
        /// Verifies that two doubles are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        static public void AreEqual(double expected, double actual, double delta)
        {
            Assert.AreEqual(expected, actual, delta,
                            new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// Verifies that two floats are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreEqual(float expected,
            float actual, float delta, InstantiateException code, params object[] args)
        {
            // handle infinity specially since subtracting two infinite values gives
            // NaN and the following test fails. mono also needs NaN to be handled
            // specially although ms.net could use either method.
            if (float.IsInfinity(expected) || float.IsNaN(expected) || float.IsNaN(actual))
            {
                if (!(expected == actual))
                    Assert.FailNotEquals(expected, actual, code, args);
            }
            else if (!(Math.Abs(expected-actual) <= delta))
                Assert.FailNotEquals(expected, actual, code, args);
        }

        /// <summary>
        /// Verifies that two floats are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        /// <param name="message">The message printed out upon failure</param>
        static public void AreEqual(float expected, float actual, float delta, InstantiateException code)
        {
            Assert.AreEqual(expected, actual, delta, code, null);
        }

        /// <summary>
        /// Verifies that two floats are equal considering a delta. If the
        /// expected value is infinity then the delta value is ignored. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="delta">The maximum acceptable difference between the
        /// the expected and the actual</param>
        static public void AreEqual(float expected, float actual, float delta)
        {
            Assert.AreEqual(expected, actual, delta,
                            new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// Verifies that two decimals are equal. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreEqual(decimal expected, decimal actual, InstantiateException code, params object[] args)
        {
            if(!(expected == actual))
                Assert.FailNotEquals(expected, actual, code, args);
        }

        /// <summary>
        /// Verifies that two decimals are equal. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        static public void AreEqual(decimal expected, decimal actual, InstantiateException code)
        {
            Assert.AreEqual(expected, actual, code, null);
        }
        
        /// <summary>
        /// Verifies that two decimals are equal. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        static public void AreEqual(decimal expected, decimal actual)
        {
            Assert.AreEqual(expected, actual,
                            new InstantiateException( UnknownException.Instantiate), null);
        }
        
        /// <summary>
        /// Verifies that two ints are equal. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreEqual(int expected, int actual, InstantiateException code, params object[] args)
        {
            if(!(expected == actual))
                Assert.FailNotEquals(expected, actual, code, args);
        }

        /// <summary>
        /// Verifies that two ints are equal. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        static public void AreEqual(int expected, int actual, InstantiateException code)
        {
            Assert.AreEqual(expected, actual, code, null);
        }

        /// <summary>
        /// Verifies that two ints are equal. If
        /// they are not equals then an <see cref="AssertionInstantiateException"/> is
        /// thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        static public void AreEqual(int expected, int actual)
        {
            Assert.AreEqual(expected, actual,
                            new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// Verifies that two arrays are equal. If they are not,
        /// then an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreEqual( System.Array expected, System.Array actual, InstantiateException code, params object[] args )
        {
            if ( expected == null && actual == null ) return;

            if ( expected == null || actual == null )
                Assert.FailNotEquals( expected, actual, code, args );

            if ( expected.Rank != actual.Rank )
                Assert.FailNotEquals( expected, actual, code, args );

            if ( expected.Rank != 1 )
                Assert.Fail( new InstantiateException( MultiDimensionNotSupportedException.Instantiate));

            int iLength = Math.Min( expected.Length, actual.Length );
            for( int i = 0; i < iLength; i++ )
                if ( !ObjectsEqual( expected.GetValue( i ), actual.GetValue( i ) ) )
                {
                    Assert.FailArraysNotEqual(i, expected, actual, code, args );
                }

            if ( expected.Length != actual.Length )
                Assert.FailArraysNotEqual( iLength, expected, actual, code, args );
                
            return;
        }
        
        /// <summary>
        /// Verifies that two arrays are equal. If they are not,
        /// then an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message printed out upon failure</param>
        static public void AreEqual( System.Array expected, System.Array actual, InstantiateException code )
        {
            Assert.AreEqual( expected, actual, code, null );
        }
        
        /// <summary>
        /// Verifies that two arrays are equal. If they are not,
        /// then an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        static public void AreEqual( System.Array expected, System.Array actual )
        {
            Assert.AreEqual( expected, actual,
                             new InstantiateException( UnknownException.Instantiate), null );
        }
        
        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value.  All
        /// non-numeric types are compared by using the <c>Equals</c> method.
        /// Arrays are compared by comparing each element using the same rules.
        /// If they are not equal an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display if objects are not equal</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreEqual(Object expected, Object actual, InstantiateException code, params object[] args)
        {
            if ( expected == null && actual == null ) return;
            if ( expected == null || actual == null )
                Assert.FailNotEquals( expected, actual, code, args );

            // FOr now, dynamically call array assertion if necessary. Try to move
            // this into the ObjectsEqual method later on.
            if ( expected.GetType().IsArray && actual.GetType().IsArray )
                Assert.AreEqual( (System.Array)expected, (System.Array)actual, code, args );
                    else
            {
                if ( !ObjectsEqual( expected, actual ) )
                    Assert.FailNotEquals( expected, actual, code, args );
            }
        }

        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value.  All
        /// non-numeric types are compared by using the <c>Equals</c> method.
        /// If they are not equal an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        /// <param name="message">The message to display if objects are not equal</param>
        static public void AreEqual(Object expected, Object actual, InstantiateException code)
        {
            Assert.AreEqual(expected, actual, code, null);
        }

        /// <summary>
        /// Verifies that two objects are equal.  Two objects are considered
        /// equal if both are null, or if both have the same value.  All
        /// non-numeric types are compared by using the <c>Equals</c> method.
        /// If they are not equal an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The value that is expected</param>
        /// <param name="actual">The actual value</param>
        static public void AreEqual(Object expected, Object actual)
        {
            Assert.AreEqual(expected, actual,
                            new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// The Equals method throws an AssertionInstantiateException. This is done
        /// to make sure there is no mistake by calling this function.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new bool Equals(object a, object b)
        {
            throw UnknownException.Instantiate();
        }

        /// <summary>
        /// override the default ReferenceEquals to throw an AssertionInstantiateException. This
        /// implementation makes sure there is no mistake in calling this function
        /// as part of Assert.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static new void ReferenceEquals(object a, object b)
        {
            throw DoNotUseException.Instantiate();
        }
                
        /// <summary>
        /// Checks the type of the object, returning true if
        /// the object is a numeric type.
        /// </summary>
        /// <param name="obj">The object to check</param>
        /// <returns>true if the object is a numeric type</returns>
        static protected bool IsNumericType( Object obj )
        {
            if( null != obj )
            {
                if( obj is byte    ) return true;
                if( obj is sbyte   ) return true;
                if( obj is decimal ) return true;
                if( obj is double  ) return true;
                if( obj is float   ) return true;
                if( obj is int     ) return true;
                if( obj is uint    ) return true;
                if( obj is long    ) return true;
                if( obj is short   ) return true;
                if( obj is ushort  ) return true;

                if( obj is System.Byte    ) return true;
                if( obj is System.SByte   ) return true;
                if( obj is System.Decimal ) return true;
                if( obj is System.Double  ) return true;
                if( obj is System.Single  ) return true;
                if( obj is System.Int32   ) return true;
                if( obj is System.UInt32  ) return true;
                if( obj is System.Int64   ) return true;
                if( obj is System.UInt64  ) return true;
                if( obj is System.Int16   ) return true;
                if( obj is System.UInt16  ) return true;
            }
            return false;
        }

        /// <summary>
        /// Used to compare two objects.  Two nulls are equal and null
        /// is not equal to non-null. Comparisons between the same
        /// numeric types are fine (Int32 to Int32, or Int64 to Int64),
        /// but the Equals method fails across different types so we
        /// use <c>ToString</c> and compare the results.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        static protected bool ObjectsEqual( Object expected, Object actual )
        {
            if ( expected == null && actual == null ) return true;
            if ( expected == null || actual == null ) return false;

            if( IsNumericType( expected )  &&
                IsNumericType( actual ) )
            {
                //
                // Convert to strings and compare result to avoid
                // issues with different types that have the same
                // value
                //
                string sExpected = expected.ToString();
                string sActual   = actual.ToString();
                return sExpected.Equals( sActual );
            }
            return expected.Equals(actual);
        }
    
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionInstantiateException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to be printed when the object is null</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void IsNotNull(Object anObject, InstantiateException code, params object[] args)
        {
            Assert.IsTrue(anObject != null, code, args);
        }

        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionInstantiateException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        static public void IsNotNull(Object anObject, InstantiateException code)
        {
            Assert.IsNotNull(anObject, code, null);
        }
    
        /// <summary>
        /// Verifies that the object that is passed in is not equal to <code>null</code>
        /// If the object is not <code>null</code> then an <see cref="AssertionInstantiateException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        static public void IsNotNull(Object anObject)
        {
            Assert.IsNotNull(anObject,
                             new InstantiateException( UnknownException.Instantiate), null);
        }
    
            
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionInstantiateException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        /// <param name="message">The message to be printed when the object is not null</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void IsNull(Object anObject, InstantiateException code, params object[] args)
        {
            Assert.IsTrue(anObject == null, code, args);
        }

        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionInstantiateException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        static public void IsNull(Object anObject, InstantiateException code)
        {
            Assert.IsNull(anObject, code, null);
        }
    
        /// <summary>
        /// Verifies that the object that is passed in is equal to <code>null</code>
        /// If the object is <code>null</code> then an <see cref="AssertionInstantiateException"/>
        /// is thrown.
        /// </summary>
        /// <param name="anObject">The object that is to be tested</param>
        static public void IsNull(Object anObject)
        {
            Assert.IsNull(anObject,
                          new InstantiateException( UnknownException.Instantiate), null);
        }
    
    
        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to be printed when the two objects are not the same object.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void AreSame(Object expected, Object actual, InstantiateException code, params object[] args)
        {
            if (object.ReferenceEquals(expected, actual)) return;

            Assert.FailNotSame(expected, actual, code, args);
        }

        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="message">The message to be printed when the object is null</param>
        static public void AreSame(Object expected, Object actual, InstantiateException code)
        {
            Assert.AreSame(expected, actual, code, null);
        }
   
        /// <summary>
        /// Asserts that two objects refer to the same object. If they
        /// are not the same an <see cref="AssertionInstantiateException"/> is thrown.
        /// </summary>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        static public void AreSame(Object expected, Object actual)
        {
            Assert.AreSame(expected, actual,
                           new InstantiateException( UnknownException.Instantiate), null);
        }
   
        /// <summary>
        /// Throws an <see cref="AssertionInstantiateException"/> with the message and arguments
        /// that are passed in. This is used by the other Assert functions.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionInstantiateException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Fail(InstantiateException code, params object[] args )
        {
            throw code();
        }

        /// <summary>
        /// Throws an <see cref="AssertionInstantiateException"/> with the message that is
        /// passed in. This is used by the other Assert functions.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionInstantiateException"/> with.</param>
        static public void Fail(InstantiateException code)
        {
            Assert.Fail( code, null);
        }
    
        /// <summary>
        /// Throws an <see cref="AssertionInstantiateException"/>.
        /// This is used by the other Assert functions.
        /// </summary>
        static public void Fail()
        {
            Assert.Fail(
                new InstantiateException( UnknownException.Instantiate), null);
        }

        /// <summary>
        /// Throws an <see cref="IgnoreInstantiateException"/> with the message and arguments
        /// that are passed in.  This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionInstantiateException"/> with.</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static public void Ignore( InstantiateException code, object[] args )
        {
            throw IgnoreException.Instantiate();
        }

        /// <summary>
        /// Throws an <see cref="IgnoreInstantiateException"/> with the message that is
        /// passed in. This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionInstantiateException"/> with.</param>
        static public void Ignore( InstantiateException code )
        {
            Assert.Ignore( code, null );
        }
    
        /// <summary>
        /// Throws an <see cref="IgnoreInstantiateException"/>.
        /// This causes the test to be reported as ignored.
        /// </summary>
        /// <param name="message">The message to initialize the <see cref="AssertionInstantiateException"/> with.</param>
        static public void Ignore()
        {
            Assert.Ignore(
                new InstantiateException( UnknownException.Instantiate), null);
        }
    
        /// <summary>
        /// This method is called when two objects have been compared and found to be
        /// different. This prints a nice message to the screen.
        /// </summary>
        /// <param name="message">The message that is to be printed prior to the comparison failure</param>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static protected void FailNotEquals(Object expected, Object actual, InstantiateException code,
            params object[] args)
        {
            Assert.Fail( new InstantiateException( UnknownException.Instantiate));
        }
    
        /// <summary>
        /// This method is called when two arrays have been compared and found to be
        /// different. This prints a nice message to the screen.
        /// </summary>
        /// <param name="index">The index at which the failure occured</param>
        /// <param name="expected">The expected array</param>
        /// <param name="actual">The actual array</param>
        /// <param name="message">The message that is to be printed prior to the comparison failure</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static protected void FailArraysNotEqual(int index, Array expected, Array actual,
            InstantiateException code, params object[] args)
        {
            Assert.Fail( new InstantiateException( UnknownException.Instantiate));
        }
    
        /// <summary>
        ///  This method is called when the two objects are not the same.
        /// </summary>
        /// <param name="message">The message to be printed on the screen</param>
        /// <param name="expected">The expected object</param>
        /// <param name="actual">The actual object</param>
        /// <param name="args">Arguments to be used in formatting the message</param>
        static protected void FailNotSame(Object expected, Object actual, InstantiateException code, params object[] args)
        {
            Assert.Fail( code);
        }
    }
}

