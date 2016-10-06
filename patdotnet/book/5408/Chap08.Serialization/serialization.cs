using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using System.Runtime.Serialization;

namespace BinarySerialization {
    [Serializable]
    class SampleClass {
        private int _value;
        private string _buffer;
        
        public SampleClass(int value, string buffer) {
            _value = value;
            _buffer = buffer;
        }
        
        public int Value {
            get {
                return _value;
            }
        }
        public string Buffer {
            get {
                return _buffer;
            }
        }
    }
}


[TestFixture]
public class TestBinarySerialization {
    [Test]
    public void TestSerialization() {
        BinarySerialization.SampleClass obj = new BinarySerialization.SampleClass(10, "hello");
        Stream a = File.OpenWrite("C:\\abc.bin");
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(a, obj);
        a.Close();
    }
    [Test]
    public void TestDeserialization() {
        FileStream file=new FileStream("C:\\abc.bin",
                                       FileMode.Open);
        
        BinaryFormatter bf = new BinaryFormatter();
        BinarySerialization.SampleClass obj = bf.Deserialize(file) as BinarySerialization.SampleClass;
        if( obj != null) {
            Console.WriteLine( "Object Value (" + obj.Value + ") Buffer (" + obj.Buffer + ")");
        }
        else {
            Console.WriteLine( "could not read");
        }
        file.Close();
    }
}


