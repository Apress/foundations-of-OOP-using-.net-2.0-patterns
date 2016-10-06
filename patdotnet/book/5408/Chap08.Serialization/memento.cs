using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace ClassicalMemento {
    class Memento {
        private string _buffer;
        public Memento( string buffer) {
            _buffer = buffer;
        }
        public string Buffer {
            get {
                return _buffer;
            }
            set {
                _buffer = value;
            }
        }
    }
    interface IOrginator {
        Memento CreateMemento();
        void SetState( Memento state);
    }
    class Simple : IOrginator {
        private string _buffer;
        public Simple( string buffer) {
            _buffer = buffer;
        }
        public Memento CreateMemento() {
            return new Memento( _buffer);
        }
        public void SetState( Memento state) {
            _buffer = state.Buffer;
        }
    }
}


namespace Memento {
    interface IOriginator< type> {
        void GetState( type state);
        void SetState( type state);
    }
    interface IMemento {
        string Buffer { get; set; }
        int Value { get; set; }
    }
    class SampleClass : IOriginator< IMemento> {
        private int _value;
        private string _buffer;
        
        public SampleClass() { }
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
        public void GetState( IMemento state) {
            state.Buffer = _buffer;
            state.Value = _value;
        }
        public void SetState( IMemento state) {
            _buffer = state.Buffer;
            _value = state.Value;
        }
    }
    
    [XmlRoot("XMLSampleClass")]
    public class XMLSampleClass : IMemento {
        private int _value;
        private string _buffer;
        
        public XMLSampleClass() {
            
        }
        public XMLSampleClass(int value, string buffer) {
            _value = value;
            _buffer = buffer;
        }
        [XmlElement( "Value")]
        public int Value {
            get {
                return _value;
            }
            set {
                _value = value;
            }
        }
        [XmlElement( "Buffer")]
        public string Buffer {
            get {
                return _buffer;
            }
            set {
                _buffer = value;
            }
        }
    }
}


namespace ImmutableMementoSolution {
    class SampleClass : Memento.IOriginator< Memento.IMemento> {
        private int _value;
        private string _buffer;
        
        public SampleClass(Memento.IMemento state) {
            _buffer = state.Buffer;
            _value = state.Value;
        }
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
        public void GetState( Memento.IMemento state) {
            state.Buffer = _buffer;
            state.Value = _value;
        }
        public void SetState( Memento.IMemento state) {
            throw new NotSupportedException();
        }
    }
    
}

[TestFixture]
public class TestMemento {
    
    [Test]
    public void TestSerialize() {
        Memento.SampleClass cls = new Memento.SampleClass( 40, "Memento");
        
        Memento.XMLSampleClass state = new Memento.XMLSampleClass();
        cls.GetState( state);
        
        XmlSerializer s = new XmlSerializer( typeof( Memento.XMLSampleClass));
        TextWriter w = new StreamWriter( @"c:\sampleclass.xml" );
        s.Serialize( w, state);
        w.Close();
        
    }
    [Test]
    public void TestDeserialize() {
        Memento.SampleClass cls = new Memento.SampleClass();
        
        Memento.XMLSampleClass state = new Memento.XMLSampleClass();

        XmlSerializer s = new XmlSerializer( typeof( Memento.XMLSampleClass ) );
        TextReader r = new StreamReader( @"c:\sampleclass.xml" );
        state = (Memento.XMLSampleClass)s.Deserialize( r );
        r.Close();
        cls.SetState( state);
        Assert.AreEqual( 40, cls.Value);
    }
}
