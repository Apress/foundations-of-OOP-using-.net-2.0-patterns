using System;
using System.Threading;

namespace ThreadingExamples {

	class TestMonitor {
		private Object _sync = new Object();

		public void Method1() {
			// Create Database Connections
			// Create Network Connections
			Monitor.Enter( _sync);
			Console.WriteLine( "Sequence 1");
			// If Network is to home do something, otherwise exit
			Thread.Sleep( 100);
			Monitor.Wait( _sync);
			Console.WriteLine( "Sequence 4");
			Monitor.Exit( _sync);
		}

		public void Method2() {
			Monitor.Enter( _sync);
			Console.WriteLine( "Sequence 2");
			Monitor.Pulse( _sync);
			Console.WriteLine( "Sequence 3");
			Monitor.Exit( _sync);
		}
	}

	public class MonitorTests {
		static void RunMonitorTest() {
			TestMonitor tst = new TestMonitor();
			Thread tid1 = new Thread( new ThreadStart( tst.Method1));
			Thread tid2 = new Thread( new ThreadStart( tst.Method2));

			tid2.Start();
			tid1.Start();
		}
		public static void RunTests() {
			RunMonitorTest();
		}
	}

}