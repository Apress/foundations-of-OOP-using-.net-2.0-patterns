using System;
using Dbg;

struct SimpleStruct {
}
class SimpleClass { 
    SimpleStruct _myStruct;
    public SimpleStruct getData() {
        return _myStruct;
    }
}

class BaseClass {
    public void Method() {
    }
}

class User1 {
    BaseClass _inst1;

    public void DoSomething() {
        _inst1.Method();
    }
}

class User2 {
    BaseClass _inst2;
}

class User3 {
    BaseClass _inst3;
}

class MainApp
{
    static void testAssembliesScope() {
        InAssembly1Class assm1 = new InAssembly1Class();

        assm1.greetings();

        InAssembly2Class assm2 = new InAssembly2Class();

        assm2.greetings();

        TestExternalReference.RunIt();
        //TestInheritance.TestClass.RunIt();
    }
    static void testMethod() {
        SimpleClass cls = new SimpleClass();
        SimpleStruct structure = cls.getData();
    }
    public static void Main(string[] args)
    {
        DebugMgr.assignDebugFlags( 10);
        Chap01SecAll.RunExamples.DoIt();
        Chap01SecInheritanceAndStructs.RunExamples.DoIt();
        Chap01SecGenerics.RunExamples.DoIt();
        Chap01SecAsynchronous.RunExamples.DoIt();
    }
}


