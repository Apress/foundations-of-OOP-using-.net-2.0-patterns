using System;
using System.Threading;

namespace ThreadingExamples {
	public interface SomeOldInterface {
		void Method();
	}

	public class NotThreadSafe : SomeOldInterface {
		public void Method() {
			Console.WriteLine( "I am not thread safe");
		}
	}

	public class ThreadSafe : SomeOldInterface {
		private SomeOldInterface _reference = null;

		public SomeOldInterface Reference {
			set {
				_reference = value;
			}
		}
		public void Method() {
			lock( this) {
				if( _reference != null) {
					_reference.Method();
				}
			}
		}
	}

}