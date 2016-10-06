using System;
using NUnit.Framework;
using Proxy;
using Devspace.Commons.Functors;

class Example {
    public int value;
    public string buffer;
    
    /*public override bool Equals( Object obj) {
        Example cls = obj as Example;
        if( cls.value == value &&
           cls.buffer.CompareTo( buffer) == 0) {
            return true;
        }
        else {
            return false;
        }
    }*/
}

[TestFixture]
public class TestEnhancedObject {
    
    IFlight ExampleBuilder() {
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
    }

    [Test]
    public void TestEquals() {
        Example cls1 = new Example();
        Example cls2 = new Example();
        
        cls1.buffer = "hello";
        cls1.value = 1;
        
        cls2.buffer = "hello";
        cls2.value = 1;
        
        Assert.IsTrue( cls1.Equals( cls1));
        Assert.IsFalse( cls1.Equals( cls2));
        
    }
}


