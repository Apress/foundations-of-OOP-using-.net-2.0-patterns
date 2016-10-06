using System;
using Dbg;

namespace First {
    namespace Nested  {
        class Embedded  {
        }
    }
}
public class InAssembly1Class {
    public void greetings() {
        DebugMgr.start( 10, "InAssembly1Class.greetings");
        DebugMgr.end( 10);
    }
    protected void greetingsProtected() {
        DebugMgr.start( 10, "InAssembly1Class.greetingsProtected");
        DebugMgr.end( 10);
    }
    internal void greetingsInternal() {
        DebugMgr.start( 10, "InAssembly1Class.greetingsInternal");
        DebugMgr.end( 10);
    }
    internal virtual void greetingsInternalVirtual() {
        DebugMgr.start( 10, "InAssembly1Class.greetingsInternalVirtual");
        DebugMgr.end( 10);
    }
    public void CallTheInternalMethod() {
        greetingsInternalVirtual();
    }
}

class InAssembly1Class2 {
    public void testScope() {
        InAssembly1Class cls = new InAssembly1Class();

        cls.greetings();
        // cls.greetingsProtected();       // Cannot access
        cls.greetingsInternal();
    }
}

