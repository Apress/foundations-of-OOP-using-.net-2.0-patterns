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
    class CalculatorFullBridgeWithState {
        private int _runningTotal;

        private IOperation _add;
        private IOperation _subtract;
        private IOperation _multiply;
        private IOperation _divide;
        private IOperation _clear;
        private IOperation _recall;
        private IOperation _op;

        public enum Ops {
            Add, Subtract, Multiply, Divide, Recall, Clear
        }

        public CalculatorFullBridgeWithState() {
            _add = OperationsFactory.createAdd();
            _subtract = OperationsFactory.createSubtract();
            _multiply = OperationsFactory.createMultiply();
            _divide = OperationsFactory.createDivide();
            _clear = OperationsFactory.createClear();
            _recall = OperationsFactory.createRecall();
        }
        public Ops op {
            set {
                if( value == Ops.Add) {
                    _op = _add;
                }
                else if( value == Ops.Subtract) {
                    _op = _subtract;
                }
                else if( value == Ops.Multiply) {
                    _op = _multiply;
                }
                else if( value == Ops.Divide) {
                    _op = _divide;
                }
                else if( value == Ops.Clear) {
                    _op = _clear;
                }
                else if( value == Ops.Recall) {
                    _op = _recall;
                }
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
            CalculatorFullBridgeWithState proc = new CalculatorFullBridgeWithState();
            proc.op = CalculatorFullBridgeWithState.Ops.Add;
            proc.doIt( 2);
            proc.doIt( 3);
            proc.doIt( 4);
            proc.op = CalculatorFullBridgeWithState.Ops.Subtract;;
            proc.doIt( 7);
            proc.op = CalculatorFullBridgeWithState.Ops.Recall;
            proc.doIt( 0);
		}
	}
}

