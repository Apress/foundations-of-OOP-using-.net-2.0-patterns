using System;
using com.devspace.commons.Tracer;

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

    internal class IntMathematicsImpl2< basetype> : IMathematics< basetype> {
        public basetype Add( basetype param1, basetype param2) {
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

    public class FactoryIMathematics2< basetype> {
        public static IMathematics< basetype> Instantiate() {
            return new IntMathematicsImpl2< basetype>();
        }
    }
}




