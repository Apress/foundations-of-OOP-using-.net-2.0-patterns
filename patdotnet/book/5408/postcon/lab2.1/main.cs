using System;

namespace ConsoleCalculator
{
    interface IOperation {
        int process( int runningTotal, int value);
    }

    public class Add : IOperation {
        public int process(int runningTotal, int value) {
            return runningTotal + value;
        }
    }
    public class Subtract : IOperation {
        public int process(int runningTotal, int value) {
            return runningTotal - value;
        }
    }
    public class Multiply : IOperation {
        public int process(int runningTotal, int value) {
            return runningTotal * value;
        }
    }
    public class Divide : IOperation {
        public int process(int runningTotal, int value) {
            if( value == 0) {
                throw new ArithmeticException( "Divide by zero");
            }
            return runningTotal / value;
        }
    }
    public class Clear : IOperation {
        public int process(int runningTotal, int value) {
            return 0;
        }
    }
    public class Recall : IOperation {
        public int process(int runningTotal, int value) {
            Console.WriteLine( "Running Total is " + runningTotal);
            return runningTotal;
        }
    }

    sealed class OperationsFactory {
        static public IOperation createAdd() {
            return new Add();
        }
        static public IOperation createSubtract() {
            return new Subtract();
        }
        static public IOperation createMultiply() {
            return new Multiply();
        }
        static public IOperation createDivide() {
            return new Divide();
        }
        static public IOperation createClear() {
            return new Clear();
        }
        static public IOperation createRecall() {
            return new Recall();
        }
    }
    class CalculatorFullBridge {
        private int _runningTotal;
        private IOperation _op;

        public CalculatorFullBridge() {
        }
        public IOperation op {
            set {
                _op = value;
            }
            get {
                return _op;
            }
        }

        public void doIt( int val) {
            try {
                _runningTotal = _op.process( _runningTotal, val);
            }
            catch( ArithmeticException exception) {
                Console.WriteLine( "Ooops an error" + exception.Message);
            }
        }
    }

	class Class1
	{
		[STAThread]
		static void Main(string[] args)
		{
            CalculatorFullBridge proc = new CalculatorFullBridge();
            proc.op = OperationsFactory.createAdd();
            proc.doIt( 2);
            proc.doIt( 3);
            proc.doIt( 4);
            proc.op = OperationsFactory.createSubtract();
            proc.doIt( 7);
            proc.op = OperationsFactory.createRecall();
            proc.doIt( 0);
		}
	}
}

