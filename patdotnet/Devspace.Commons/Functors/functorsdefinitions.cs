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

namespace Devspace.Commons.Functors {
    public class TypeConversionException: Exception {
        public TypeConversionException(string message) : base(message) { }
        public TypeConversionException() { }
    }
    public delegate outType DelegateTransformer<inpType, outType>(inpType input);

    public class PredicateEvaluationException: Exception {
        public PredicateEvaluationException(string message) : base(message) { }
        public PredicateEvaluationException() { }
    }
    public delegate bool DelegatePredicate< type>( type input);

    public class ClosureEvaluationException : Exception {
        public ClosureEvaluationException( string message) : base( message) { }
        public ClosureEvaluationException() { }
    }
    public delegate void DelegateClosure< type>( type input);
    
    public class ComparerEvaluationException : Exception {
        public ComparerEvaluationException( string message) : base( message) {}
        public ComparerEvaluationException() {}
    }
    public delegate int DelegateComparer< type1, type2>( type1 obj1, type2 obj2);
    
    public class PredicateAndFunctor< type> {
        DelegatePredicate<type> _predicates;
        public PredicateAndFunctor( DelegatePredicate<type>[] functors) {
            foreach( DelegatePredicate< type> functor in functors) {
                _predicates += functor;
            }
        }
        
        public bool PredicateFunction( type obj) {
            foreach( Delegate delg in _predicates.GetInvocationList()) {
                if( !(bool)(delg.DynamicInvoke( new Object[] { obj}))) {
                    return false;
                }
            }
            return true;
        }
        public static DelegatePredicate< type> CreateInstance( DelegatePredicate< type> functor1, DelegatePredicate< type> functor2) {
            return CreateInstance( new DelegatePredicate< type>[] { functor1, functor2});
        }
        public static DelegatePredicate< type> CreateInstance( DelegatePredicate< type>[] functors) {
            return new DelegatePredicate< type>( new PredicateAndFunctor< type>( functors).PredicateFunction);
        }
    }
    public class PredicateOrFunctor<type> {
        DelegatePredicate<type> _predicates;
        public PredicateOrFunctor(DelegatePredicate<type>[] functors) {
            foreach(DelegatePredicate<type> functor in functors) {
                _predicates += functor;
            }
        }

        public bool PredicateFunction(type obj) {
            foreach(Delegate delg in _predicates.GetInvocationList()) {
                if((bool)(delg.DynamicInvoke(new Object[] { obj }))) {
                    return true;
                }
            }
            return false;
        }
        public static DelegatePredicate<type> CreateInstance(DelegatePredicate<type> functor1, DelegatePredicate<type> functor2) {
            return CreateInstance(new DelegatePredicate<type>[] { functor1, functor2 });
        }
        public static DelegatePredicate<type> CreateInstance(DelegatePredicate<type>[] functors) {
            return new DelegatePredicate<type>(new PredicateAndFunctor<type>(functors).PredicateFunction);
        }
    }

}


