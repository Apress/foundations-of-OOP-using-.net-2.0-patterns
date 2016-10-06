using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

internal static class DeveloperIds {
    public static string AmazonID {
        get {
            return "19A14D9F2DF74T6FFS02";
        }
    }
}

namespace Blackboxes {
    interface Math<type> {
        type Add( type param1, type param2 );
    }

    class Manipulations {
        public type SeriesAdd<type>( Math<type> adder, type[] values ) {
            type runningtotal = default( type );
            foreach( type value in values ) {
                runningtotal = adder.Add( runningtotal, value );
            }
            return runningtotal;
        }
    }
}

namespace MoreEfficientArchitecture {
    public class ToCreate {
    }

    public static class Factory {
        private static ToCreate _singleton = new ToCreate();

        public static ToCreate FactoryVer1() {
            return new ToCreate();
        }
        public static ToCreate FactoryVer2() {
            return _singleton;
        }
    }
}

namespace ComplicatedClass {
    class Math {
        public Decimal Addition( Decimal val1, Decimal val2 ) {
            return Decimal.Add( val1, val2 );
        }
    }

    class SimplerMath {
        public int Add( int val1, int val2 ) {
            Math math = new Math();

            return (int)(math.Addition( new Decimal( val1 ), new Decimal( val2 ) ));
        }
    }
}

namespace SimpleMicroKernel {
    interface IExternal {
        void DoSomething();
    }
    class ExternalServer : IExternal {
        public virtual void DoSomething() {
        }
    }

    class MicroKernel {
        public IExternal FindExternal() {
            return new ExternalServer();
        }
    }

    class SimpleLoader {
        public type CreateInstance<type>( string assemblyPath, string @class) {
            Assembly assembly;
            assembly = Assembly.Load( AssemblyName.GetAssemblyName( assemblyPath ) );
            return (type)assembly.CreateInstance( @class);        }
    }

    class MicroKernel2 : SimpleLoader {
        public IExternal FindExternal() {
            return CreateInstance<IExternal>( "somepath", "ExternalServer" );
        }
    }

    class Adapter {
        public void EasyDoSomething() {
            MicroKernel mk = new MicroKernel();
            IExternal external = new ExternalServer();
            external.DoSomething();
        }
    }

}

namespace Chap04.Application {
    class Program {
        static void Main(string[] args) {
            ComplicatedClass.SimplerMath math = new ComplicatedClass.SimplerMath();

            Console.WriteLine( "Value " + math.Add( 1, 2 ) );
        }
    }
}
