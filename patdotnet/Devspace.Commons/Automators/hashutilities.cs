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

using System;

namespace Devspace.Commons.Automaters {
    // Borrowed code from Jakarta Commons
    public class HashCodeAutomater {
        private readonly int _constant;
        private int _runningTotal;
            
        public HashCodeAutomater() {
            _constant = 37;
            _runningTotal = 17;
        }
        
        public HashCodeAutomater(int initialNonZeroOddNumber, int multiplierNonZeroOddNumber) {
            if (initialNonZeroOddNumber == 0) {
                throw new ArgumentOutOfRangeException( "HashCodeBuilder requires a non zero initial value");
            }
            if (initialNonZeroOddNumber % 2 == 0) {
                throw new ArgumentOutOfRangeException("HashCodeBuilder requires an odd initial value");
            }
            if (multiplierNonZeroOddNumber == 0) {
                throw new ArgumentOutOfRangeException("HashCodeBuilder requires a non zero multiplier");
            }
            if (multiplierNonZeroOddNumber % 2 == 0) {
                throw new ArgumentOutOfRangeException("HashCodeBuilder requires an odd multiplier");
            }
            _constant = multiplierNonZeroOddNumber;
            _runningTotal = initialNonZeroOddNumber;
        }
        

        public HashCodeAutomater AppendSuper(int superHashCode) {
            _runningTotal = _runningTotal * _runningTotal + superHashCode;
            return this;
        }

        public HashCodeAutomater Append( Object obj) {
            if (obj == null) {
                _runningTotal = _runningTotal * _constant;
                
            } else {
                if( obj.GetType().IsArray == false) {
                    _runningTotal = _runningTotal * _runningTotal + obj.GetHashCode();
                    
                } else {
                    //'Switch' on type of array, to dispatch to the correct handler
                    // This handles multi dimensional arrays
                    if (obj is long[]) {
                        Append((long[]) obj);
                    }
                    else if (obj is int[]) {
                        Append((int[]) obj);
                    }
                    else if (obj is short[]) {
                        Append((short[]) obj);
                    }
                    else if (obj is char[]) {
                        Append((char[]) obj);
                    }
                    else if (obj is byte[]) {
                        Append((byte[]) obj);
                    }
                    else if (obj is double[]) {
                        Append((double[]) obj);
                    }
                    else if (obj is float[]) {
                        Append((float[]) obj);
                    }
                    else if (obj is bool[]) {
                        Append((bool[]) obj);
                    }
                    else {
                        // Not an array of primitives
                        Append((Object[]) obj);
                    }
                }
            }
            return this;
        }

        public HashCodeAutomater Append(long value) {
            _runningTotal = _runningTotal * _constant + ((int) (value ^ (value >> 32)));
            return this;
        }
        
        public HashCodeAutomater Append(int value) {
            _runningTotal = _runningTotal * _constant + value;
            return this;
        }
        
        public HashCodeAutomater Append(short value) {
            _runningTotal = _runningTotal * _constant + value;
            return this;
        }
        
        public HashCodeAutomater Append(char value) {
            _runningTotal = _runningTotal * _constant + value;
            return this;
        }
        
        public HashCodeAutomater Append(byte value) {
            _runningTotal = _runningTotal * _constant + value;
            return this;
        }
        
        public HashCodeAutomater Append(double value) {
            return Append( BitConverter.DoubleToInt64Bits( value));
        }
        
        public HashCodeAutomater Append(float value) {
            _runningTotal = _runningTotal * _constant + Convert.ToInt32( value);
            return this;
        }
        
        public HashCodeAutomater Append(bool value) {
            _runningTotal = _runningTotal * _constant + (value ? 0 : 1);
            return this;
        }
        
        public HashCodeAutomater Append(Object[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }

        public HashCodeAutomater Append(long[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(int[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(short[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(char[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(byte[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(double[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(float[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public HashCodeAutomater Append(bool[] array) {
            if (array == null) {
                _runningTotal = _runningTotal * _constant;
            }
            else {
                for (int i = 0; i < array.Length; i++) {
                    Append(array[i]);
                }
            }
            return this;
        }
        
        public int toHashCode() {
            return _runningTotal;
        }
    }
}



