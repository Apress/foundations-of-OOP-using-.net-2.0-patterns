using System;
using NUnit.Framework;
using devcommons = com.devspace.commons.Tracer;

class Mathematics {
    public int Add( int param1, int param2) {
        checked {
            return param1 + param2;
        }
    }
}


[TestFixture] 
public class TestMath {
    private Mathematics _obj;

    [TestFixtureSetUp] public void Init(){ 
        _obj = new Mathematics();
    }
    [TestFixtureTearDown] public void Dispose() { 
    }
    [Test] public void Add() {
        Assert.AreEqual( 6, _obj.Add( 2, 4), 6, "Addition of simple numbers");
    }
    [Test]
    [ExpectedException(typeof(OverflowException))]
    public void OverflowAdd()
    {
        _obj.Add( 2000000000, 2000000000);
    }
}

public class MainApp
{
    public static void Main(string[] args) {
        devcommons.CallTracer.IsActive = true;
        TestMath obj = new TestMath();
        obj.OverflowAdd();
    }
}



