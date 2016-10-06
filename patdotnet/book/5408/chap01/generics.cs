using System;
using Dbg;

namespace Chap01SecGenerics {
    class OldContainer {
        private Object _contained;

        public Object MyProperty {
            get {
                return _contained;
            }
            set {
                _contained = value;
            }
        }
    }
#if GenericsSupported
    class NewContainer< item> {
        private item _contained;

        public item MyProperty {
            get {
                return _contained;
            }
            set {
                _contained = value;
            }
        }
    }

    class BaseType< mytype> {
        public virtual void DoSomething( mytype value){
        }
    }
    class NewContainerConstrained< item> : NewContainer< item> where item : BaseType< item> {
        public void Method( item param) {
            param.DoSomething( MyProperty);
        }
    }
#endif

    public class RunExamples {
        public static void SimpleOldContainer() {
            DebugMgr.start( 10, "RunExamples.SimpleOldContainer");
            OldContainer container = new OldContainer();
            container.MyProperty = 2;
            int value = (int)container.MyProperty;
            DebugMgr.output( 10, "Value is " + value);
            DebugMgr.end( 10);
        }
        public static void SimpleNewContainer() {
            DebugMgr.start( 10, "RunExamples.SimpleNewContainer");
#if GenericsSupported
            NewContainer< int> container = new NewContainer< int>();
            container.MyProperty = 2;
            int value = container.MyProperty;
            DebugMgr.output( 10, "Value is " + value);
#else
			DebugMgr.output( 10, "Not Implemented");
#endif
            DebugMgr.end( 10);
        }
        public static void DoIt() {
            DebugMgr.start( 10, "RunExamples.DoIt");
            SimpleOldContainer();
            SimpleNewContainer();
            DebugMgr.end( 10);
            
        }
    }
}
