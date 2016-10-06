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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Devspace.Commons.TDD;
using Devspace.Commons.PipesFilters;
using Devspace.Commons.Collections;
using Devspace.Commons.Functors;
using Devspace.Commons.Automaters;
using Devspace.Commons.Pool;
using Devspace.Commons.State;

using System.Threading;

class Inside {
    public override string ToString() {
        MemoryTracer.Start( this );
        MemoryTracer.Variable( "Some variable", "Some value" );
        MemoryTracer.StartArray( "Local Embedded" );
        MemoryTracer.Variable( "embedded", "value" );
        MemoryTracer.Variable( "embedded", "value" );
        MemoryTracer.EndArray();
        return MemoryTracer.End();
    }

}
class Sample {
    private Inside _inside = new Inside();

    public override string ToString() {
        MemoryTracer.Start( this );
        MemoryTracer.Variable( "Variable", "Value before" );
        MemoryTracer.Embedded( _inside.ToString() );
        MemoryTracer.Variable( "Variable", "Value after" );
        return MemoryTracer.End();
    }
}


[TestFixture]
public class TestTracer {
    [Test]
    public void TestToString() {
        Sample cls = new Sample();

        Console.WriteLine( "Generated\n" + cls.ToString() );
    }
}

[TestFixture]
public class TestPipesFilters {
    [Test]
    public void TestBufferReaderWriter() {

        StreamingControlImpl<int>.BridgedStreams obj = new StreamingControlImpl<int>.BridgedStreams();

        InputStream<int> input = (InputStream<int>)obj;
        OutputStream<int> output = (OutputStream<int>)obj;

        output.Write( 1 );
        output.Write( 2 );
        output.Write( 3 );
        output.Write( 4 );
        output.Write( 5 );
        output.Write( 6 );
        Console.WriteLine( "Value (" + output.ToString() + ")" );
        int counter = 0;
        foreach( int value in input ) {
            counter++;
        }
        NUnit.Framework.Assert.AreEqual( 6, counter );
        output.Flush();
        Console.WriteLine( "Value (" + output.ToString() + ")" );
        counter = 0;
        foreach( int value in input ) {
            counter++;
        }
        NUnit.Framework.Assert.AreEqual( 0, counter );

        output.Write( 1 );
        output.Write( 2 );
        counter = 0;
        foreach( int value in input ) {
            counter++;
        }
        Console.WriteLine( "Value (" + output.ToString() + ")" );
        NUnit.Framework.Assert.AreEqual( 2, counter );

        input.Reset();
        NUnit.Framework.Assert.IsTrue( input.Available(), "First read content is available\n" + input.ToString() );
        NUnit.Framework.Assert.AreEqual( 1, input.Read(), "Read value is not one\n" + input.ToString() );
        NUnit.Framework.Assert.IsTrue( input.Available(), "Second read content is available\n" + input.ToString() );
        NUnit.Framework.Assert.AreEqual( 2, input.Read(), "Read value is not second\n" + input.ToString() );
        NUnit.Framework.Assert.IsFalse( input.Available(), "Third read and content is not available\n" + input.ToString() );

        int[] buffer;
        input.Read( out buffer );
        NUnit.Framework.Assert.AreEqual( 0, buffer.Length, "Length should nothing as all has been read" );
        input.Reset();
        input.Read( out buffer );
        NUnit.Framework.Assert.AreEqual( 2, buffer.Length, "Length should 2" );

        input.Reset();
        input.Read();
        output.Write( new int[] { 5, 6 } );
        Console.WriteLine( "Value (" + output.ToString() + ")" );
        NUnit.Framework.Assert.IsFalse( input.Available(), "Wrote content so should be at the end");

        input.Reset();
        input.Read( out buffer );
        NUnit.Framework.Assert.AreEqual( 3, buffer.Length, "Length should three long" );
        NUnit.Framework.Assert.AreEqual( 1, buffer[ 0 ] );
        NUnit.Framework.Assert.AreEqual( 5, buffer[ 1 ] );
        NUnit.Framework.Assert.AreEqual( 6, buffer[ 2 ] );

    }
}


[TestFixture]
public class TestMediator {
    [Test]
    public void TestSimple() {
        Object obj1 = new Object();
        Sample sample = new Sample();
        Object obj2 = new Object();
        Inside inside = new Inside();

        /*Devspace.Commons.Mediator.MediatorSimple mediator = new Devspace.Commons.Mediator.MediatorSimple();

        mediator.Add( obj1, sample );
        mediator.Add( obj2, inside );

        NUnit.Framework.Assert.AreEqual( obj1, mediator.Get<Sample, Object>( sample ) );
        NUnit.Framework.Assert.AreEqual( sample, mediator.Get<Object, Sample>( obj1 ) );
        NUnit.Framework.Assert.IsNull( mediator.Get< Object, Object>( new Object()));
        NUnit.Framework.Assert.AreEqual( obj2, mediator.Get<Inside, Object>( inside ) );
        NUnit.Framework.Assert.AreEqual( inside, mediator.Get<Object, Inside>( obj2 ) );
         */

    }
}

class ObjectToUse {
    private int _value;

    public ObjectToUse(int value) {
        _value = value;
    }

    public override string ToString() {
        MemoryTracer.Start(this);
        MemoryTracer.Variable("_value", _value);
        return MemoryTracer.End();
    }
}

class TransformedObject {
    private ObjectToUse _object;

    public TransformedObject(ObjectToUse obj) {
        _object = obj;
    }
    public override string ToString() {
        MemoryTracer.Start(this);
        MemoryTracer.Embedded(_object.ToString());
        return MemoryTracer.End();
    }
}

class MyTransformer {
    public TransformedObject Transform(ObjectToUse input) {
        return new TransformedObject(input);
    }
}

[TestFixture]
public class TestCollections {
    [Test]
    public void TestSimple() {
        IList< TransformedObject> list = new List< TransformedObject>();
        TransformerProxy<ObjectToUse, TransformedObject> collection =
            new TransformerProxy<ObjectToUse, TransformedObject>(list,
            new DelegateTransformer<ObjectToUse, TransformedObject>(new MyTransformer().Transform));

        collection.Add(new ObjectToUse(1));
        collection.Add(new ObjectToUse(2));
        Console.WriteLine(collection.ToString());
    }
}

class HashcodeExample {
    public int value;
    public string buffer;
    
    public HashcodeExample( int val, string buf) {
        value = val;
        buffer = buf;
    }
    public override int GetHashCode() {
        return new HashCodeAutomater()
            .Append( value)
            .Append( buffer).toHashCode();
    }
    public override bool Equals( Object obj) {
        return (obj.GetHashCode() == this.GetHashCode());
    }
}

[TestFixture]
public class TestHashUtilities {
    [Test]
    public void TestSimple() {
        HashcodeExample cls1 = new HashcodeExample( 10, "hello");
        HashcodeExample cls2 = new HashcodeExample( 10, "hello");
        HashcodeExample cls3 = new HashcodeExample( 20, "hello");
        HashcodeExample cls4 = new HashcodeExample( 10, "hellos");
        
        NUnit.Framework.Assert.AreEqual( cls1.GetHashCode(), cls2.GetHashCode());
        NUnit.Framework.Assert.AreNotEqual( cls1.GetHashCode(), cls3.GetHashCode());
        NUnit.Framework.Assert.AreNotEqual( cls1.GetHashCode(), cls4.GetHashCode());
        
        string buf1 = "hello";
        string buf2 = "hello";
        string buf3 = "bye";
        
        NUnit.Framework.Assert.AreEqual( buf1.GetHashCode() , buf2.GetHashCode());
        NUnit.Framework.Assert.AreNotEqual( buf1.GetHashCode(), buf3.GetHashCode());
        
        NUnit.Framework.Assert.IsTrue( cls1.Equals( cls2));
    }
}


public class PoolReference< type> {
    WeakReference _parentReference;
    
    public PoolReference( IObjectPoolBase<type> parent) {
        _parentReference = new WeakReference( parent);
    }
    public PoolReference( WeakReference parent) {
        _parentReference = parent;
    }
    ~PoolReference() {
        Console.WriteLine( "Destroy PoolReference");
    }
    public bool ReturnObject( type inst) {
        if( _parentReference.Target != null) {
            ((IObjectPoolBase<type>)_parentReference.Target).ReturnObject( inst);
            GC.ReRegisterForFinalize( inst);
            return true;
        }
        else {
            Console.WriteLine( "Target is null");
        }
        return false;
    }
    public override string ToString() {
        MemoryTracer.Start( this);
        if( _parentReference.Target != null) {
            MemoryTracer.Variable( "Alive reference", _parentReference.Target.ToString());
        }
        else {
            MemoryTracer.Embedded( "Not alive");
        }
        return MemoryTracer.End();
    }
    public Object Target {
        get {
            return _parentReference.Target;
        }
    }
}

public class PoolHelper {
    public static void ReturnObject< type>( WeakReference reference, type inst) {
        if( reference.Target != null) {
            ((IObjectPoolBase<type>)reference.Target).ReturnObject( inst);
            Console.WriteLine( "I am being called");
            GC.ReRegisterForFinalize( inst);
            return;
        }
        else {
            Console.WriteLine( "Target is null");
        }
        return;
    }
}

class ReferencedClass {
    public ReferencedClass() {
        Console.WriteLine( "ReferencedClass Initialized");
    }
    ~ReferencedClass() {
        Console.WriteLine( "ReferencedClass finalized");
    }
    public void CallMethod() {
        Console.WriteLine( "---");
    }
}

class TestPoolAllocation {
    public int Identifier;
    IObjectPoolBase<TestPoolAllocation> _reference;
    
    public TestPoolAllocation( IObjectPoolBase<TestPoolAllocation> parent) {
        _reference = parent;
    }
    ~TestPoolAllocation() {
        Console.WriteLine( "Destroy TestPoolAllocation");
        //Console.WriteLine( "IsNullWeakReference (" + this.IsNullWeakReference() + ")");
        //PoolHelper.ReturnObject( _reference, this);
        _reference.ReturnObject( this);
    }
    public override string ToString() {
        MemoryTracer.Start( this);
        MemoryTracer.Variable( "Identifier", Identifier);
        return MemoryTracer.End();
    }
    /*    public bool IsNullWeakReference() {
        return (_reference.Target == null);
    }
    public Object Target {
        get {
            return _reference.Target;
        }
    }*/
}

class TestPoolAllocation2 : IDisposable {
    WeakReference _reference;
    
    public TestPoolAllocation2( IObjectPoolBase<TestPoolAllocation2> parent) {
        _reference = new WeakReference( parent);;
    }
    
    public void Dispose() {
        if( _reference.Target != null) {
            ((IObjectPoolBase<TestPoolAllocation2>)_reference.Target).
                ReturnObject( this);
        }
    }
    
    
}

class TestPoolAllocationFactory : IPoolableObjectFactory< TestPoolAllocation> {
    public TestPoolAllocation MakeObject(IObjectPoolBase<TestPoolAllocation> parent) {
        return new TestPoolAllocation( parent);
    }
    public void DestroyObject(TestPoolAllocation obj) {
        Console.WriteLine( "TestPoolAllocationFactory.DestroyObject (" + obj.ToString() + ")");
    }
    public void ActivateObject(TestPoolAllocation obj) {
        obj.Identifier = 0;
    }
    public void PassivateObject(TestPoolAllocation obj) {
    }
}


[TestFixture]
public class TestObjectPool {
    void Method1(GenericObjectPool< TestPoolAllocation> pool) {
        Console.WriteLine( "--- Start Method1 ---");
        TestPoolAllocation obj = pool.GetObject();
        obj.Identifier = 100;
        NUnit.Framework.Assert.AreEqual( 1, pool.NumActive);
        //Console.WriteLine( "Object (" + obj.ToString() + ")");
        //Console.WriteLine( "IsNullWeakReference (" + obj.IsNullWeakReference() + ")");
        //Console.WriteLine( "Equal (" + Object.ReferenceEquals( pool, obj.Target) + ")");
        Console.WriteLine( "--- End Method1 ---");
    }
    void Method2(GenericObjectPool< TestPoolAllocation> pool) {
        Console.WriteLine( "--- Start Method2 ---");
        NUnit.Framework.Assert.AreEqual( 0, pool.NumActive);
        TestPoolAllocation obj = pool.GetObject();
        NUnit.Framework.Assert.AreEqual( 100, obj.Identifier);
        NUnit.Framework.Assert.AreEqual( 1, pool.NumActive);
        //Console.WriteLine( "Object (" + obj.ToString() + ")");
        Console.WriteLine( "--- End Method2 ---");
    }
    [Test]
    public void TestAllocateFreeAndReallocate() {
        GenericObjectPool< TestPoolAllocation> pool = new GenericObjectPool< TestPoolAllocation>();
        pool.SetFactory( new TestPoolAllocationFactory());
 
        Method1( pool);
        GC.Collect();
        GC.Collect();
        // Delay is needed so that the Garbage compacter runs
        Thread.Sleep( 500);
        Console.WriteLine( "NumActive (" + pool.NumActive + ") NumIdle (" + pool.NumIdle + ")");
        NUnit.Framework.Assert.AreEqual( 0, pool.NumActive);
        NUnit.Framework.Assert.AreEqual( 1, pool.NumIdle);
        Method2( pool);
    }
}


interface ITestFlyweight {
    string Identifier {
        get;
    }
}

class TestFlyweightA : ITestFlyweight {
    public String Identifier {
        get {
            return "TestFlyweightA";
        }
    }
}

class TestFlyweightB : ITestFlyweight {
    public String Identifier {
        get {
            return "TestFlyweightB";
        }
    }
}

class FlyweightBuilder {
    public static ITestFlyweight Transformation( object desc) {
        if( String.Compare((string)desc, "TestFlyweightA") == 0) {
            return new TestFlyweightA();
        }
        else if( String.Compare((string)desc, "TestFlyweightB") == 0) {
            return new TestFlyweightB();
        }
        throw new NotSupportedException();
    }
    public static IFlyweightCollection< ITestFlyweight> Instantiate() {
        return new FlyweightCollection< ITestFlyweight>(
            new DelegateTransformer< object,ITestFlyweight>( FlyweightBuilder.Transformation));
    }
}

[TestFixture]
public class TestFlyweight {
    [Test]
    public void TestSimple() {
        IFlyweightCollection< ITestFlyweight> var = FlyweightBuilder.Instantiate();
        
        ITestFlyweight var1 = var.GetItem( "TestFlyweightA");
        ITestFlyweight var2 = var.GetItem( "TestFlyweightB");
        
        NUnit.Framework.Assert.AreEqual( "TestFlyweightA", var1.Identifier);
        NUnit.Framework.Assert.AreEqual( "TestFlyweightB", var2.Identifier);
        
        ITestFlyweight var1a = var.GetItem( "TestFlyweightA");
        NUnit.Framework.Assert.IsTrue( Object.ReferenceEquals( var1, var1a));

        ITestFlyweight var2a = var.GetItem( "TestFlyweightB");
        NUnit.Framework.Assert.IsTrue( Object.ReferenceEquals( var2, var2a));
        
    }
}

class SingletonStructure {
    public int Value;
    public string Buffer;
}

class MySingletonBuilder : ISingletonBuilder< SingletonStructure> {
    bool IsTestSingletonValid( SingletonStructure obj) {
        return true;
    }
    
    public DelegatePredicate<SingletonStructure> IsValid {
        get {
            return new DelegatePredicate< SingletonStructure>( IsTestSingletonValid);
        }
    }
    
    SingletonStructure NewTestSingleton( object obj) {
        return new SingletonStructure();
    }
    public DelegateTransformer<object, SingletonStructure> NewObject {
        get {
            return new DelegateTransformer< object, SingletonStructure>( NewTestSingleton);
        }
    }
    
    public int SleepTime {
        get {
            return 1000;
        }
    }
    
    public bool KeepPolling {
        get {
            return true;
        }
    }
}

class MySingletonDelegator : BaseSingletonDelegation< SingletonStructure> {
    protected override bool IsObjectValid( SingletonStructure obj) {
        return true;
    }
    protected override SingletonStructure InstantiateNewObject( object descriptor) {
        return new SingletonStructure();
    }
}

[TestFixture]
public class TestSingleton {
    [Test]
    public void TestSimpleSingleton() {
        Singleton< MySingletonBuilder, SingletonStructure>.Instance( null).Buffer = "hello";
        
        NUnit.Framework.Assert.AreEqual( "hello", Singleton< MySingletonBuilder, SingletonStructure>.Instance( "").Buffer);
        Thread.Sleep( 1000);
    }
    [Test]
    public void TestSimpleSingleton2() {
        NUnit.Framework.Assert.AreEqual( null, Singleton< MySingletonDelegator, SingletonStructure>.Instance( "").Buffer);

        Singleton< MySingletonDelegator, SingletonStructure>.Instance( null).Buffer = "hello";
        
        NUnit.Framework.Assert.AreEqual( "hello", Singleton< MySingletonDelegator, SingletonStructure>.Instance( "").Buffer);
        Thread.Sleep( 1000);
    }
}


