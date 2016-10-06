using System;
using Dbg;

namespace Chap01SecInheritanceAndStructs {
    // *********************************
    interface RunningTotal {
        int GetValue();
        void IncrementValue();
    }

    struct ExStruct2 : RunningTotal {
        private int _value;

        public int GetValue() {
            return _value;
        }
        public void IncrementValue() {
            _value ++;
        }
    }

    class ExClass2 : RunningTotal {
        private int _value;

        public int GetValue() {
            return _value;
        }
        public void IncrementValue() {
            _value ++;
        }
    }

    class ExMethodCall2 {
        public static void SecondMethod( RunningTotal param) {
            DebugMgr.start( 10, "ExMethodCall.SecondMethod");
            param.IncrementValue();
            DebugMgr.output( 10, "During RunningTotal.value = " + param.GetValue());
            DebugMgr.end( 10);
        }
        public static void FirstMethod() {
            DebugMgr.start( 10, "ExMethodCall.FirstMethod");
            ExStruct2 cls1 = new ExStruct2();
            ExClass2 cls2 = new ExClass2();
            DebugMgr.output( 10, "Before ExStruct2.value = " + cls1.GetValue());
            DebugMgr.output( 10, "Before ExClass2.value = " + cls2.GetValue());
            SecondMethod( cls1);
            SecondMethod( cls2);
            DebugMgr.output( 10, "After ExStruct2.value = " + cls1.GetValue());
            DebugMgr.output( 10, "After ExClass2.value = " + cls2.GetValue());
            DebugMgr.end( 10);
        }
    }
    // *********************************
    struct ExStruct {
        public int value;
    }
    class ExClass {
        public int value;
    }

    class ExMethodCall {
        public static void SecondMethod( ExStruct param1, ExClass param2) {
            DebugMgr.start( 10, "ExMethodCall.SecondMethod");
            param1.value ++;
            param2.value ++;
            DebugMgr.output( 10, "During ExStruct.value = " + param1.value);
            DebugMgr.output( 10, "During ExClass.value = " + param2.value);
            DebugMgr.end( 10);
        }
        public static void FirstMethod() {
            DebugMgr.start( 10, "ExMethodCall.FirstMethod");
            ExStruct cls1 = new ExStruct();
            ExClass cls2 = new ExClass();
            DebugMgr.output( 10, "Before ExStruct.value = " + cls1.value);
            DebugMgr.output( 10, "Before ExClass.value = " + cls2.value);
            SecondMethod( cls1, cls2);
            DebugMgr.output( 10, "After ExStruct.value = " + cls1.value);
            DebugMgr.output( 10, "After ExClass.value = " + cls2.value);
            DebugMgr.end( 10);
        }
    }

    // *********************************
    public class RunExamples {
        public static void IllustrateStructAndClass() {
            DebugMgr.start( 10, "TestInheritance.IllustrateStructAndClass");
            ExMethodCall.FirstMethod();
            DebugMgr.end( 10);
        }
        public static void IllustrateStructAndClass2() {
            DebugMgr.start( 10, "TestInheritance.IllustrateStructAndClass");
            ExMethodCall2.FirstMethod();
            DebugMgr.end( 10);
        }
        public static void DoIt() {
            IllustrateStructAndClass();
            IllustrateStructAndClass2();
        }
    }
}

namespace Chap01SecAll {
    // *********************************
    class BaseClass {
        public void SimpleMethod() {
            DebugMgr.start( 10, "BaseClass.SimpleMethod");
            DebugMgr.end( 10);
        }
        public virtual void VirtualMethod() {
            DebugMgr.start( 10, "BaseClass.VirtualMethod");
            DebugMgr.end( 10);
        }
    }
    
    class Subclassed : BaseClass {
        public new void SimpleMethod() {
            DebugMgr.start( 10, "Subclassed.SimpleMethod");
            DebugMgr.end( 10);
        }
        public override void VirtualMethod() {
            DebugMgr.start( 10, "Subclassed.VirtualMethod");
            DebugMgr.end( 10);
        }
    }

    // ***********************************************
    class Animal {
        public virtual void WhatAmI() {
            DebugMgr.output( 10, "I don't know what you are");
        }
    }

    class Human : Animal {
        public override void WhatAmI() {
            DebugMgr.output( 10, "I am a human");
        }
    }

    class Dog : Animal {
        public override void WhatAmI() {
            DebugMgr.output( 10, "I am a dog");
        }
    }

    class EnglishBulldog : Dog {
        public new virtual void WhatAmI() {
            DebugMgr.output( 10, "I am an English Bulldog");
        }
    }

    class NewVersion : Subclassed {
        public virtual new void VirtualMethod() {
            DebugMgr.start( 10, "NewVersion.VirtualMethod");
            DebugMgr.end( 10);
        }
    }

    class NewVersionSubclassed : NewVersion {
        public override void VirtualMethod() {
            DebugMgr.start( 10, "NewVersionSubclassed.VirtualMethod");
            DebugMgr.end( 10);
        }
    }

    public class RunExamples {
        private static void SimpleInheritance() {
            DebugMgr.start( 10, "TestInheritance.SimpleInheritance");
            Subclassed subcls = new Subclassed();
            subcls.SimpleMethod();
            DebugMgr.output( 10, "Now assigning to type BaseClass");
            BaseClass basecls = subcls;
            basecls.SimpleMethod();
            DebugMgr.end( 10);
        }
        private static void PolymorphicInheritance() {
            DebugMgr.start( 10, "TestInheritance.PolymorphicInheritance");
            DebugMgr.output( 10, "Created Human and assigned to Animal");
            Animal animal1 = new Human();
            animal1.WhatAmI();
            DebugMgr.output( 10, "Created Dog and assigned to Animal");
            Animal animal2 = new Dog();
            animal2.WhatAmI();
            DebugMgr.end( 10);
        }
        private static Animal CreateCurrentDogInstance() {
            DebugMgr.start( 10, "TestInheritance.CreateCurrentDogInstance");
            DebugMgr.end( 10);
            return new EnglishBulldog();
            // return new Dog(); Original version
        }
        private static void TimeProofPolymorphicInheritance() {
            DebugMgr.start( 10, "TestInheritance.TimeProofPolymorphicInheritance");
            Animal animal = CreateCurrentDogInstance();
            animal.WhatAmI();
            DebugMgr.end( 10);
        }
        private static void MultipleInheritance() {
            DebugMgr.start( 10, "TestInheritance.MultipleInheritance");
            DebugMgr.output( 10, "Created NewVersionSubclassed, and assigned to type BaseClass");
            BaseClass baseCls = new NewVersionSubclassed();
            baseCls.VirtualMethod();
            DebugMgr.output( 10, "Now assigning to type NewVersion");
            NewVersion newVer = (NewVersion)baseCls;
            newVer.VirtualMethod();
            DebugMgr.end( 10);
        }
        public static void DoIt() {
            SimpleInheritance();
            PolymorphicInheritance();
            TimeProofPolymorphicInheritance();
        }
    }
}
