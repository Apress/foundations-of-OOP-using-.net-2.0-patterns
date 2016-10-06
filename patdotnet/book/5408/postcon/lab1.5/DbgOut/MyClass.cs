using System;
using System.Collections;

namespace DbgMgr {
    public class DebugMgr {
        private static Stack _stack = new Stack();
        private static int _debugLevel = 0;

        public static void assignDebugFlags( int debugLevel) {
            _debugLevel = debugLevel;
        }

        public static void start( int debugLevel, string buffer) {
            if( debugLevel <= _debugLevel) {
                output( debugLevel, "**** start (" + buffer + ") ****");
                _stack.Push( buffer);
            }
        }
        public static void end( int debugLevel) {
            if( debugLevel <= _debugLevel) {
                string identifier = (string)_stack.Pop();
                output( debugLevel, "**** end (" + identifier + ") ****");
            }
        }
        public static void output( int debugLevel, string buffer) {
            if( debugLevel <= _debugLevel) {
                int counter = 0;
                while( counter < _stack.Count) {
                    Console.Write( "    ");
                    counter += 1;
                }
                Console.Write( buffer);
                Console.Write( "\n");
            }
        }
        public static void outputVar( int debugLevel, string identifier, object buffer) {
            if( debugLevel <= _debugLevel) {
                int counter = 0;
                while( counter < _stack.Count) {
                    Console.Write( "    ");
                    counter += 1;
                }
                Console.Write( identifier);
                Console.Write( "(");
                Console.Write( buffer);
                Console.Write( ")\n");
            }
        }
    }
}


// project created on 9/24/2004 at 8:23 PM
