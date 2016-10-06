
using System;
using System.Text;
using Chap06Visitor.Definitions;

namespace Chap06Visitor.Implementations
{
    public class Implementations1 : Chap06Visitor.Definitions.IBase
    {
        /// <summary>
        /// Method Accept
        /// </summary>
        /// <param name="visitor">An IVisitor</param>
        public void Accept(IVisitor visitor)
        {
            visitor.Process( this);
        }
        
        
    }
    public class Implementations2 : Chap06Visitor.Definitions.IBase
    {
        /// <summary>
        /// Method Accept
        /// </summary>
        /// <param name="visitor">An IVisitor</param>
        public void Accept(IVisitor visitor)
        {
            visitor.Process( this);
        }
    }
}


