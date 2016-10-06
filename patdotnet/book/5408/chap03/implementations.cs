using System;
using Devspace.Commons.TDD;

using Console = Chap03MockObjects.Console;

namespace Chap03 {

    public interface Intention {
        void Echo( string message);
    }

    internal class Implementation : Intention {
        public void Echo(string message) {
            Console.WriteLine( "From the console " + message);
        }
    }

    public class Factory {
        public static Intention Instantiate() {
            return new Implementation();
        }
    }


    // ******************
#if GenericsSupported
    public interface IMathematics< numbertype> {
        numbertype Add( numbertype param1, numbertype param2);
    }

    internal class IntMathematicsImpl : IMathematics< int> {
        public int Add(int param1, int param2) {
            checked {
                return param1 + param2;
            }
        }
    }

    public class FactoryIMathematics {
        public static IMathematics< int> Instantiate() {
            return new IntMathematicsImpl();
        }
    }
#endif

#if _NEVER_INCLUDE_
    internal class IntMathematicsImpl2< basetype> : IMathematics< basetype> {
        public basetype Add( basetype param1, basetype param2) {
            checked {
                return param1 + param2;
            }
        }
    }

    public class FactoryIMathematics2< basetype> {
        public static IMathematics< basetype> Instantiate() {
            return new IntMathematicsImpl2< basetype>();
        }
    }
#endif
    // ******************
    public interface IMathematicsObj {
        object Add( object param1, object param2);
    }

    public class IntMathematicsImplObj : IMathematicsObj {
        public object Add(object param1, object param2) {
            checked {
                return (int)param1 + (int)param2;
            }
        }
    }

    public class FactoryIMathematicsObj {
        public static IMathematicsObj Instantiate() {
            return new IntMathematicsImplObj();
        }
    }

    // ************************************************
}

internal class PropertyNotDefined : ApplicationNonFatalException {
    public PropertyNotDefined( string propIdentifier) : base( "Property " + propIdentifier + " is not defined") {
    }
}

namespace MathBridgeDotNet1 {
    public interface IMathematics {
        int Add( int param1, int param2);
        int Reset();
    }

    internal class ImplIMathematics : IMathematics {
        public int Add(int param1, int param2) {
            checked {
                return param1 + param2;
            }
        }
        public int Reset() {
            return 0;
        }
    }

    public class Factory {
        public static IMathematics Instantiate() {
            return new ImplIMathematics();
        }
    }
    public class Factory2 {
        public static IMathematics Instantiate<type>() {
            if( typeof( type) is int) {
                return new ImplIMathematics();
            }
        }
    }
    
    public class Operations {
        private IMathematics _math;

        public IMathematics Math {
            get {
                if( _math == null) {
                    throw new PropertyNotDefined( "Operations.Math");
                }
                return _math;
            }
            set {
                _math = value;
            }
        }
        public int AddArray( int[] numbers) {
            int total = 0;

            foreach( int number in numbers) {
                total = this.Math.Add( total, number);
            }
            return total;
        }
#if NEVER_INCLUDE
        public object AddArray( object[] numbers) {
            object total = this.Math.Reset();
            foreach( object number in numbers) {
                total = this.Math.Add( total, number);
            }
        }
#endif
    }
}

#if GenericsSupported
namespace MathBridgeDotNet2 {
    public interface IMathematics< type> {
        type Add( type param1, type param2);
        type Reset();
    }

    internal class ImplIMathematics : IMathematics< int> {
        public int Add(int param1, int param2) {
            checked {
                return param1 + param2;
            }
        }
        public int Reset() {
            return 0;
        }
    }

    public class Factory {
        public static IMathematics<int> Instantiate() {
            return new ImplIMathematics();
        }
    }

    public class Operations< type> {
        private IMathematics< type> _math;

        public IMathematics<type> Math {
            get {
                if( _math == null) {
                    throw new PropertyNotDefined( "Operations.Math");
                }
                return _math;
            }
            set {
                _math = value;
            }
        }
        public type AddArray( type[] numbers) {
            type total = this.Math.Reset();

            foreach( type number in numbers) {
                total = this.Math.Add( total, number);
            }
            return total;
        }
    }

}
#endif

namespace Taxation {
    public interface IIncomes {
    }
    public interface IDeductions {
    }
    public interface ITaxMath {
        Decimal IncomeTax( Decimal rate, Decimal value);
    }
    public abstract class TaxMath {
        public virtual Decimal IncomeTax( Decimal rate, Decimal value) {
            return new Decimal();
        }
    }
    internal class StubTaxMath : TaxMath {
    }
    public class TaxMathFactory {
        public static TaxMath Instantiate() {
            return new StubTaxMath();
        }
    }
    public interface ITaxation {
        IIncomes [] Incomes { get; set; }
        IDeductions [] Deductions { get; set; }

        Decimal CalculateTax();
    }
    internal class SwissTaxes : ITaxation {
        public IIncomes[] Incomes {
            get { return null; }
            set { ; }
        }
        public IDeductions[] Deductions {
            get { return null; }
            set { ; }
        }
        public Decimal CalculateTax() {
            return new Decimal();
        }
    }

    public abstract class BaseTaxation : ITaxation {
        private IIncomes[] _incomes;
        private IDeductions[] _deductions;

        public IIncomes[] Incomes {
            get {
                if( _incomes == null) {
                    throw new PropertyNotDefined( "BaseTaxation.Incomes");
                }
                return _incomes;
            }
            set { _incomes = value; }
        }
        public IDeductions[] Deductions {
            get {
                if( _deductions == null) {
                    throw new PropertyNotDefined( "BaseTaxation.Deductions");
                }
                return _deductions;
            }
            set { _deductions = value; }
        }
        public abstract Decimal CalculateTax();
    }
    internal class AmericanTaxes : BaseTaxation {
        public override Decimal CalculateTax() {
            return new Decimal();
        }
    }
}

namespace Instantiations {
    public interface SimpleInterface {
    }
    internal class MultipleInstances : SimpleInterface {
    }
    internal class SingleInstance : SimpleInterface {
    }
    public class SimpleInterfaceFactory {
        public static SimpleInterface FirstType() {
            return new MultipleInstances();
        }
        private static SingleInstance _instance;
        public static SimpleInterface SecondType() {
            if( _instance == null) {
                _instance = new SingleInstance();
            }
            return _instance;
        }
    }
}

namespace Test {
    public class Something {
        public static basetype func< basetype>() {
            basetype obj;

            return obj;
        }
    }

    public interface Specific {
    }

    public class Consumer {
        public static void Test() {
            Specific obj = Something.func< Specific>();
        }
    }
}


namespace Builder {
    public interface ITaxMath {
    }
    public interface ITaxation {
        ITaxMath TaxMath { get; set; }
    }
    class SwissTaxationImpl : ITaxation {
        public ITaxMath TaxMath { get {return null;} set {;} }
    }
    class SwissTaxMathImpl : ITaxMath {
    }
    public class Builder {
        public ITaxation InstantiateSwiss() {
            ITaxation taxation = new SwissTaxationImpl();
            taxation.TaxMath = new SwissTaxMathImpl();
            return taxation;
        }
    }
}



