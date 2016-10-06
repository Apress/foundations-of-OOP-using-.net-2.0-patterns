using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Devspace.Commons.Functors;

class ObjectEqualsReference {
    private int _value;
    private string _buffer;
    
    public ObjectEqualsReference( int value, string buffer) {
        _value = value;
        _buffer = buffer;
    }
}

class ObjectEqualsContent {
    private int _value;
    private string _buffer;
    
    public ObjectEqualsContent( int value, string buffer) {
        _value = value;
        _buffer = buffer;
    }
    public override bool Equals( Object obj) {
        ObjectEqualsContent cls = obj as ObjectEqualsContent;
        if( cls._value == _value &&
            cls._buffer.CompareTo( _buffer) == 0) {
            return true;
        }
        else {
            return false;
        }
    }
}

[TestFixture]
public class TestFlight {
    
    /*IFlight ExampleBuilder() {
        return new FlightComparer( new Flight(), delegate( IFlight flight1, IFlight flight2) {
                                      if( flight1.Equals( flight2)) {
                                          return 0;
                                      }
                                      else {
                                          return -1;
                                      }
                                  });
        
    }
    [Test]
    [ExpectedException( typeof(Devspace.Commons.Functors.ComparerEvaluationException))]
    public void SimpleInteraction() {
        IFlight flight = null;
        flight = ExampleBuilder();
        flight.NextLeg = flight;
    }*/
    
    // Illustrates how the base classes do not implement Equals correctly
    [Test]
    public void TestEqualsAndReference() {
        Console.WriteLine( "-- TestEqualsAndReference --");
        ObjectEqualsReference robj1 = new ObjectEqualsReference( 10, "hello");
        ObjectEqualsReference robj2 = new ObjectEqualsReference( 10, "hello");
        
        // Gotcha even though the elements are equal, it returns false
        Assert.IsFalse( robj1.Equals( robj2));
        Assert.IsTrue( robj1.Equals( robj1));
        
        ObjectEqualsContent obj1 = new ObjectEqualsContent( 10, "hello");
        ObjectEqualsContent obj2 = new ObjectEqualsContent( 10, "hello");
        
        Assert.IsTrue( obj1.Equals( obj2));
        Assert.IsTrue( obj1.Equals( obj1));
        
        IList< ObjectEqualsContent> list1 = new List< ObjectEqualsContent>();
        list1.Add( obj1);
        list1.Add( obj2);

        IList< ObjectEqualsContent> list2 = new List< ObjectEqualsContent>();
        list2.Add( obj1);
        list2.Add( obj2);
        // Gotcha even though the list contains equal elements, it returns false
        //Assert.IsFalse( list1.Equals( list2));
        
        Devspace.Commons.Collections.ImplementedEqualsAndHashCode<ObjectEqualsContent>  hlist1 =
            new Devspace.Commons.Collections.ImplementedEqualsAndHashCode<ObjectEqualsContent>( list1);
        Devspace.Commons.Collections.ImplementedEqualsAndHashCode<ObjectEqualsContent>  hlist2 =
            new Devspace.Commons.Collections.ImplementedEqualsAndHashCode<ObjectEqualsContent>( list2);
        
        Assert.IsTrue( hlist1.Equals( hlist2));
        
        // Gotcha as the GetHashCode is not implemented in ObjectEqualsContent and will return unequal values
        // there should also be a warning indicating that Equals has been implemented and not GetHashCode.
        Assert.AreNotEqual( hlist1.GetHashCode(), hlist2.GetHashCode());
    }
}


