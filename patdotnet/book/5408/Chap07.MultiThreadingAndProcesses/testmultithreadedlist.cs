using System;
using NUnit.Framework;
using System.Collections;
using System.Threading;


public class StringClass {
    public string data;
    public StringClass( string buffer) {
        data = buffer;
    }
}
class TestArrayList {
    public ArrayList _list;
    
    public TestArrayList() {
        _list = ArrayList.Synchronized( new ArrayList());
        _list.Add( new StringClass( "hello"));
        _list.Add( new StringClass( "how"));
        _list.Add( new StringClass( "are"));
        _list.Add( new StringClass( "you"));
        _list.Add( new StringClass( "doing"));
    }
    
    public void InOtherThread() {
        foreach( StringClass element in _list) {
            Thread.Sleep( 1000);
            Console.WriteLine( "List (" + element.data + ")");
        }
    }
}


[TestFixture]
public class TestMultiThreadedList {
    [Test]
    public void TestSimple() {
        TestArrayList array = new TestArrayList();
        Thread tid = new Thread( new ThreadStart( array.InOtherThread));
        tid.Start();
        
        lock( array._list.SyncRoot) {
            //array._list.Add( "something else");
            ((StringClass)array._list[ 0]).data = "some other data";
            Console.WriteLine( "Before");
            Thread.Sleep( 2000);
            array._list.Add( new StringClass( "hello again"));
            Console.WriteLine( "After");
            Thread.Sleep( 4000);
        }
        foreach( StringClass element in array._list) {
            Console.WriteLine( "List (" + element.data + ")");
        }
    }
}

namespace HowNotToSolve {
    interface ISingletonBuilder { }
    partial class Singleton {
        private static ISingletonBuilder _builder;
        static Singleton() {
            _builder = Singleton.GetMyBuilder();
        }
    }
    
    partial class Singleton {
        class MySingletonBuilder : ISingletonBuilder { }
        static ISingletonBuilder GetMyBuilder() {
            return new MySingletonBuilder();
        }
    }
}
