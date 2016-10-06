//<<<<<<< .mine
using System;
using System.Text;
using NUnit.Framework;

abstract class VisitorImplementations : Chap06Visitor.Definitions.IVisitor {
    public virtual void Process( Chap06Visitor.Implementations.Implementation1 obj) {
        
    }
    public virtual void Process( Chap06Visitor.Implementations.Implementation2 obj) {
        
    }
    public void Process<paramtype> (paramtype parameter) where paramtype : class {
        if( parameter is Chap06Visitor.Implementations.Implementation1) {
            Process( parameter as Chap06Visitor.Implementations.Implementation1);
        }
        else if( parameter is Chap06Visitor.Implementations.Implementation2) {
            Process( parameter as Chap06Visitor.Implementations.Implementation2);
        }
    }
}
class MyVisitor : VisitorImplementations {
    public override void Process( Chap06Visitor.Implementations.Implementation1 obj) {
        Console.WriteLine( "Implementation1");
    }
    public override void Process( Chap06Visitor.Implementations.Implementation2 obj) {
        Console.WriteLine( "Implementation2");
        
    }
}

[TestFixture]
public class TestVisitor {
    [Test]
    public void TestSimple() {
        //Chap06Visitor.Implementations.Implementations1 impl = new Chap06Visitor.Implementations.Implementations1();
        //MyVisitor visitor = new MyVisitor();
        //impl.Accept( visitor);
        //impl.Accept(
    }
}
/*
=======
using System;
using System.Text;
using NUnit.Framework;

abstract class VisitorImplementations : Chap06Visitor.Definitions.IVisitor {
    public virtual void Process( Chap06Visitor.Implementations.Implementation1 obj) {
        
    }
    public virtual void Process( Chap06Visitor.Implementations.Implementation2 obj) {
    }
    public void Process<paramtype> (paramtype parameter) where paramtype : class {
        if( parameter is Chap06Visitor.Implementations.Implementation1) {
            Process( parameter as Chap06Visitor.Implementations.Implementation1);
        }
        else if( parameter is Chap06Visitor.Implementations.Implementation2) {
            Process( parameter as Chap06Visitor.Implementations.Implementation2);
        }
    }
}
class MyVisitor : VisitorImplementations {
    public override void Process( Chap06Visitor.Implementations.Implementation1 obj) {
        Console.WriteLine( "Implementation1");
    }
    public override void Process( Chap06Visitor.Implementations.Implementation2 obj) {
        Console.WriteLine( "Implementation2");
        
    }
}

[TestFixture]
public class TestVisitor {
    [Test]
    public void TestSimple() {
        Chap06Visitor.Implementations.Implementation1 impl = new Chap06Visitor.Implementations.Implementation1();
        MyVisitor visitor = new MyVisitor();
        impl.Accept( visitor);
    }
}
>>>>>>> .r31
*/
