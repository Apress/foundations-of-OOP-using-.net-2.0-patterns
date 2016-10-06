using System;
using System.Collections;
using System.Collections.Generic;
using Devspace.Commons.Functors;

class ExampleIterator : IEnumerable {
    public IEnumerator GetEnumerator() {
        yield return 1;
        yield return 2;
        yield return 3;
    }
}

class IntegerData : IEnumerable {
    private IList< int> _list = new List< int>();
    private DelegatePredicate< int> _predicate;
    
    public IntegerData( DelegatePredicate< int> predicate) {
        _predicate = predicate;
    }
    public void Add( int value) {
        _list.Add( value);
    }
    public IEnumerator GetEnumerator() {
        foreach( int value in _list) {
            if( _predicate( value)) {
                yield return value;
            }
        }
    }
}
