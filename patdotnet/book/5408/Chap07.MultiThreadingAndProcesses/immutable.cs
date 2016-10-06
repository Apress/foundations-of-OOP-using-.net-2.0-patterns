using System;

namespace ThreadingExamples {

	class MyData {
		private readonly int _value = 0;
		private readonly int _value2 = 0;

		public MyData() {
			_value = 0;
		}
		public MyData( int value) 
		{
			_value = value;
		}
		public MyData( int value, int value2) {
			_value = value;
			_value2 = value2;
		}
		public MyData( MyData oldValue) {
			_value = oldValue.Value;
		}

		public int Value {
			get {
				return _value;
			}
		}

		public int PowerOperation( int input) 
		{
			return input * _value * _value2;
		}

		public MyData AddValue( int val) {
			return new MyData( _value + val);
		}
	}

	public class Immutable {
		static void TestImmutable() {
			Console.WriteLine( "*** Start Test Immutable ***");
			MyData data = new MyData( 32);

			MyData data2 = new MyData( data);

			Console.WriteLine( "*** End Test Immutable ***");
		}
		public static void RunTests() {
			TestImmutable();
		}
	}

}