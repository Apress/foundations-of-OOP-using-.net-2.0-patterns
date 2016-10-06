
using System;
using System.Text;
using NUnit.Framework;

interface IBase {
    void Method< type>( type param);
}

class Implementation : IBase {
    public void Method< paramtype>( paramtype param) {
        
    }
}
namespace Chap06Visitor.Definitions {
    public interface IVisitor2 {
        //void Process( Chap06Visitor.Implementations.Implementation1 obj);
        //void Process( Chap06Visitor.Implementations.Implementation2 obj);
        void Process< type>( type parameter) where type : class ;
    }
}

class MyVisitor : Chap06Visitor.Definitions.IVisitor {
    public void Process<paramtype> (paramtype parameter) where paramtype : class {
    }
}

[TestFixture]
public class TestVisitor {
    [Test]
    public void TestSimple() {
        Chap06Visitor.Implementations.Implementations1 impl = new Chap06Visitor.Implementations.Implementations1();
        //impl.Accept(
    }
}
