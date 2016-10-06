
using System;
using System.Text;

namespace Chap06Visitor.Definitions
{
    public interface IBase {
        void Accept( IVisitor visitor);
    }
    public interface IVisitor {
        //void Process( Chap06Visitor.Implementations.Implementation1 obj);
        //void Process( Chap06Visitor.Implementations.Implementation2 obj);
        void Process< type>( type parameter) where type : class ;
    }
}

