using System;
using System.Text;

namespace Chap06Visitor.Definitions
{
    public interface IBase {
        void Accept( IVisitor visitor);
    }
    public interface IVisitor {
        void Process< type>( type parameter) where type : class ;
    }
}

