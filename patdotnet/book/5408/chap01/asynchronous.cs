using System;
using Dbg;

namespace Chap01SecAsynchronous {

    delegate void DoItNow( string param);

    class ReceiveMessage {
        public void MethodToDoItNow( string param) {
            DebugMgr.start( 10, "ReceiveMessage.MethodToDoItNow");
            DebugMgr.output( 10, "Received message " + param);
            DebugMgr.end( 10);
        }
    }

    public class RunExamples {
        public static void DoIt() {
            DebugMgr.start( 10, "RunExamples.DoIt");
            ReceiveMessage receiver = new ReceiveMessage();
            DoItNow myDelegate = new DoItNow( receiver.MethodToDoItNow);

            myDelegate( "do something");
            DebugMgr.end( 10);
       }
    }
}
