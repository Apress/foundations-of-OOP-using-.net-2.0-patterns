#region devspace.com Copyright (c) 2005, Christian Gross
// one line to give the library's name and an idea of what it does.
// Copyright (C) 2005  Christian Gross devspace.com (christianhgross@yahoo.ca)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;

namespace Devspace.Commons.TDD {
    public class CallTracer {
        private static Stack _stack = new Stack();
        private static bool _isActive = false;

        static CallTracer() {
            NameValueCollection pluginConfig = (NameValueCollection) ConfigurationSettings.GetConfig("settings/tracer");

            if( pluginConfig != null) {
                if( pluginConfig["isactive"] != null && pluginConfig[ "isactive"] == "true") {
                    Console.WriteLine( "Woohoo is active");
                }
            }
        }
        public static bool IsActive{
            get {
                return _isActive;
            }
            set {
                _isActive = value;
            }
        }
        public static void Start( string buffer) {
            if( _isActive) {
                Output( "**** start (" + buffer + ") ****");
                _stack.Push( buffer);
            }
        }
        public static void Start( MethodBase methodBase) {
            Start( methodBase.DeclaringType.FullName + "." + methodBase.Name);
        }
        public static void End( ) {
            if( _isActive) {
                string identifier = (string)_stack.Pop();
                Output( "**** end (" + identifier + ") ****");
            }
        }
        public static void Output( string buffer) {
            if( _isActive) {
                int counter = 0;
                string tempBuffer = "";
                while( counter < _stack.Count) {
                    tempBuffer += "   ";
                    counter += 1;
                }
                tempBuffer += buffer;
                Logging.Debug( tempBuffer);
            }
        }
        public static void OutputVar( int debugLevel, string identifier, object buffer) {
            if( _isActive) {
                int counter = 0;
                string tempBuffer = "";
                while( counter < _stack.Count) {
                    tempBuffer += "   ";
                    counter += 1;
                }
                tempBuffer += identifier + "(" + buffer + ")";
                Logging.Debug( tempBuffer);
            }
        }
        public static void Start( int offset, string buffer) {

        }
    }

    /// <summary>
    /// MemoryTracer: Used to generate an easy to read display of a ToString dump
    /// This class is not memory save, but will be made so in the future
    /// </summary>
    public class MemoryTracer {
        private static int _depthCounter;
        private static System.Collections.Generic.Stack<String> _buffers =
            new System.Collections.Generic.Stack<String>();

        static MemoryTracer() {
            _depthCounter = 0;
        }
        public static void StartArray( string buffer ) {
            _buffers.Push( "" );
            Output( "Array: " + buffer );
            _depthCounter++;
        }
        public static void EndArray() {
            _depthCounter--;
            string temp =  _buffers.Pop();
            string temp2 = _buffers.Pop();
            temp2 += temp;
            _buffers.Push( temp2);
        }
        public static void StartGeneric( string buffer) {
            _buffers.Push( "" );
            Output( "Location: " + buffer);
            _depthCounter++;
        }
        public static void Start( string buffer ) {
            _buffers.Push( "" );
            Output( "Type: " + buffer);
            _depthCounter++;
        }
        public static void Start( Object obj ) {
            Start( obj.GetType().FullName );
        }
        public static string End() {
            _depthCounter--;
            return _buffers.Pop();
        }
        public static void EndGeneric() {
            Console.WriteLine( End());
        }
        public static void Embedded( string value ) {
            string tempbuffer = _buffers.Pop();
            _buffers.Push( tempbuffer += value );
        }
        public static void Variable( string identifier, int value ) {
            Output( "Variable: " + identifier + " (" + value + ")");
        }
        public static void Variable(string identifier, double value) {
            Output("Variable: " + identifier + " (" + value.ToString() + ")");
        }
        public static void Variable(string identifier, ulong value) {
            Output( "Variable: " + identifier + " (" + value + ")" );
        }
        public static void Variable( string identifier, string value ) {
            Output( "Variable: " + identifier + " (" + value + ")");
        }
        public static void Variable( string identifier, bool value ) {
            if( value ) {
                Output( "Variable: " + identifier + " (true)" );
            }
            else {
                Output( "Variable: " + identifier + " (false)" );
            }
        }
        public static void Variable(string identifier, bool[] values) {
            StartArray(identifier);
            for( int c1 = 0; c1 < values.Length; c1 ++) {
                Output( "Element " + c1 + " (" + values[ c1] + ")");
            }
            EndArray();
        }
        public static void Variable(string identifier, double[] values) {
            StartArray(identifier);
            for( int c1 = 0; c1 < values.Length; c1 ++) {
                Output( "Element " + c1 + " (" + values[ c1] + ")");
            }
            EndArray();
        }
        public static void Variable(string identifier, int[] values) {
            StartArray(identifier);
            for( int c1 = 0; c1 < values.Length; c1 ++) {
                Output( "Element " + c1 + " (" + values[ c1] + ")");
            }
            EndArray();
        }
        public static void Variable( String identifier, IEnumerable list) {
            StartArray(identifier);
            foreach( Object obj in list) {
                Output( "Collection Element (" + obj.ToString() + ")");
            }
            EndArray();
        }
        public static void Output( string buffer ) {
            int counter = 0;
            string tempBuffer = _buffers.Pop();
            while( counter < _depthCounter ) {
                tempBuffer += "   ";
                counter += 1;
            }
            tempBuffer += buffer + "\n";
            _buffers.Push( tempBuffer);
        }
        public static void OutputVar( string identifier, object buffer ) {
            int counter = 0;
            string tempBuffer = _buffers.Pop();
            while( counter < _depthCounter ) {
                tempBuffer += "   ";
                counter += 1;
            }
            tempBuffer += identifier + "(" + buffer + ")\n";
            _buffers.Push( tempBuffer);
        }
    }

    public class GenerateOutput {
        public static void Write( String identifier, int[][] var) {
            Logging.Debug( "(int[][] " + identifier + "\n  (Length (" + var.Length + ")\n");
            foreach( int[] numbers in var) {
                Logging.Debug( "    Element (Length " + numbers.Length + ") (Numbers ");
                foreach( int value in numbers) {
                    Logging.Debug( "(" + value + ")");
                }
                Logging.Debug( "))\n");
            }
            Logging.Debug( "  ))\n");
        }
        public static void Write( String identifier, int[] var) {
            Logging.Debug( "(int[] " + identifier + "\n  (Length (" + var.Length + ") Numbers (");
            foreach( int value in var) {
                Logging.Debug( "(" + value + ")");
            }
            Logging.Debug( "))\n");
        }
        public static void Write( String identifier, double[] var) {
            Logging.Debug( "(double[] " + identifier + "\n  (Length (" + var.Length + ") Numbers (");
            foreach( double value in var) {
                Logging.Debug( "(" + value + ")");
            }
            Logging.Debug( "))\n");
        }
        public static void Write( String identifier, bool[] var) {
            Logging.Debug( "(int[] " + identifier + "\n  (Length (" + var.Length + ") Numbers (");
            foreach( bool value in var) {
                if( value) {
                    Logging.Debug( "(true)");
                }
                else {
                    Logging.Debug( "(false)");
                }
            }
            Logging.Debug( "))\n");
        }
        public static void Write( String identifier, IEnumerable list) {
            Logging.Debug( "(Type" + list.GetType().Name + " " + identifier + "(\n");
            foreach( Object obj in list) {
                Logging.Debug( "(" + obj.ToString() + ")");
            }
            Logging.Debug( "))\n");
        }
        public static void Write( String identifier, Object obj) {
            Logging.Debug( "(Type" + obj.GetType().Name + " " + identifier + "(\n"
                                      + obj.ToString() + "))\n");
        }
        public static void Write( String identifier, String value) {
            Logging.Debug( "(String " + identifier + " (" + value + "))\n");
        }
        public static void Write( String identifier, double value) {
            Logging.Debug( "(double " + identifier + " (" + value + "))\n");
        }
        public static void Write( String identifier, int value) {
            Logging.Debug( "(int " + identifier + " (" + value + "))\n");
        }
        public static void Write( String identifier, ulong value ) {
            Logging.Debug( "(ulong " + identifier + " (" + value + "))\n" );
        }
        
        public static void WriteSingle( String identifier, double[] var) {
            string buffer = "(double[] " + identifier + "\n  (Length (" + var.Length + ") Numbers (";
            foreach( double value in var) {
                buffer += "(" + value + ")";
            }
            buffer += "))\n";
            Logging.Debug( buffer);
        }
    }

}

