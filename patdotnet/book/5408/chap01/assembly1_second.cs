using System;
using Dbg;

class InAssembly1OtherFile {
    public void testScope() {
        InAssembly1Class cls = new InAssembly1Class();

        cls.greetings();
        // cls.greetingsProtected();          // Cannot access
        cls.greetingsInternal();

         InAssembly1Class2 cls2 = new InAssembly1Class2();
         cls2.testScope();
    }
}
