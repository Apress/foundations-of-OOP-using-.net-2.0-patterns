using System;
using System.Collections;

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

        // This is a back door that is not normally part of the state pattern
        // However it is added so that you have the ability to use a custom
        // implementation that extends the state pattern
        public void useCustomOperation( IOperation operation) {
            _op = operation;
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

    class Exec : IOperation {
        public ArrayList _list = new ArrayList();

        private class Step {
            public Step( CalculatorFullBridgeWithState.Ops op, int val) {
                _op = op;
                _val = val;
            }
            public CalculatorFullBridgeWithState.Ops _op;
            public int _val;
        }

        public void addOperator( CalculatorFullBridgeWithState.Ops op, int val) {
            _list.Add( new Step( op, val));
        }

        private class MyRecall : IOperation {
            public int _runningTotal;
            public int process(int runningTotal, int value) {
                _runningTotal = runningTotal;
                return runningTotal;
            }
        }
        public int process( int runningTotal, int value) {
            CalculatorFullBridgeWithState proc = new CalculatorFullBridgeWithState();
            MyRecall recall = new MyRecall();
            foreach( Step item in _list) {
                if( item._op == CalculatorFullBridgeWithState.Ops.Recall) {
                    // This is why plugins are SO IMPORTANT...
                    // We have a hack because the abstract factory is hard coded,
                    // meaning we have to update the abstract factory or the state class
                    // It is partially ok to update the factory or state, because it is a single change.  
                    // However, if this
                    // were implemented with configuration files, then only the configuration 
                    // file would need changing.  If you do not want to use the plugin
                    // architecture then ALWAYS have a safe back door in the state pattern
                    proc.useCustomOperation( recall);
                }
                else {
                    proc.op = item._op;
                }
                proc.doIt( item._val);
            }
            return recall._runningTotal;
        }
    }
    class Builder {
        static public IOperation createOperationType1( int val1, int val2, int val3, int val4) {
            Exec exec = new Exec();
            exec.addOperator( CalculatorFullBridgeWithState.Ops.Add, val1);
            exec.addOperator( CalculatorFullBridgeWithState.Ops.Add, val2);
            exec.addOperator( CalculatorFullBridgeWithState.Ops.Add, val3);
            exec.addOperator( CalculatorFullBridgeWithState.Ops.Subtract, val4);
            exec.addOperator( CalculatorFullBridgeWithState.Ops.Recall, 0);
            return exec;
        }
    }

    // *****************************************************************************************
    interface IFormula {
        int process( int runningTotal, int value);
    }

    abstract class BaseFormula : IFormula {
        private IFormula _next;

        public BaseFormula( IFormula next) {
            _next = next;
        }
        public BaseFormula() {
        }
        public IFormula next {
            set {
                _next = value;
            }
        }
        public virtual int process( int runningTotal, int value) {
            return _next.process( runningTotal, value);
        }
    }

    class ImplFormula : BaseFormula {
        protected int _a;
        protected int _c;

        public ImplFormula( IFormula next, int a, int c) : base( next) {
           _a = a;
           _c = c;
        }
        public override int process( int runningTotal, int value) {
            return base.process( runningTotal + _a * (int)Math.Pow((double)value, (double)_c), value);
        }
    }

    class SquareRoot : BaseFormula {
        public SquareRoot( IFormula next) : base( next) {
        }
        public override int process(int runningTotal, int value) {
            return base.process( runningTotal + (int)Math.Sqrt( value), value);
        }
    }

    class EndPoint : IFormula {
        public virtual int process(int runningTotal, int value) {
            Console.WriteLine( "Total is " + runningTotal);
            return runningTotal;
        }
    }

	class Class1
	{   
		[STAThread]
		static void Main(string[] args)
		{
            IFormula formula = new ImplFormula( new SquareRoot( new ImplFormula( new EndPoint(), 1, 1)), 2, 2);
            formula.process( 0, 4);
		}
	}
}

