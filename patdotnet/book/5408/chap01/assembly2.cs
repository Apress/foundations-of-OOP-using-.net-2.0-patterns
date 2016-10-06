using System;
using Dbg;

public class InAssembly2Class {
    public void greetings() {
        DebugMgr.start( 10, "InAssembly2Class.greetings");
        InAssembly1Class cls = new InAssembly1Class();

        cls.greetings();
        // cls.greetingsProtected();      // This method cannot be access due to protected measures
        // cls.greetingsInternal();         // This method cannot be accessed outside the assembly
        // InAssembly1Class2 cls2 = new InAssembly1Class2();     // Cannot be accessed

        DebugMgr.end( 10);
    }
}

public class InAssembly2Class2 : InAssembly1Class {
    public new void greetings() {
        DebugMgr.start( 10, "InAssembly2Class2.greetings");
        DebugMgr.end( 10);
    }
    new void greetingsProtected() {
        DebugMgr.start( 10, "InAssembly2Clas2.greetingsProtected");
        DebugMgr.end( 10);
    }
    internal void greetingsInternal() {
        DebugMgr.start( 10, "InAssembly2Class2.greetingsInternal");
        DebugMgr.end( 10);
    }
    public virtual void greetingsInternalVirtual() {
        DebugMgr.start( 10, "InAssembly2Class2.greetingsInternalVirtual");
        DebugMgr.end( 10 );
    }
}

public class TestExternalReference {
    public static void RunIt() {
        DebugMgr.start( 10, "TestExternalreference.RunIt");

        InAssembly2Class2 cls = new InAssembly2Class2();
        cls.greetings();
        //cls.greetingsProtected();
        cls.greetingsInternal();
        cls.greetingsInternalVirtual();

        DebugMgr.output( 10, "-----------");
        InAssembly1Class cls2 = cls;
        cls2.greetings();
        //cls2.greetingsInternal();
        cls2.CallTheInternalMethod();
        DebugMgr.end( 10);
    }
}
