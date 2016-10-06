using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

public interface IBase {
    void Accept( IVisitor visitor);
}

public interface IVisitor {
    void Process< type>( type parameter) where type : class ;
}


namespace VisitorSerialization {
    [XmlRoot("SampleClass")]
    public class SampleClass : IBase {
        private int _value;
        private string _buffer;
        
        public SampleClass() {
            
        }
        public SampleClass(int value, string buffer) {
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
        public void Accept( IVisitor visitor) {
            visitor.Process( this);
        }
    }
    public class VisitorImplementation : IVisitor {
        private string _path;
        
        public VisitorImplementation( string path) {
            _path = path;
        }
        public void Process< type>(type parameter) where type: class{
            if( parameter is VisitorSerialization.SampleClass) {
                XmlSerializer s = new XmlSerializer( typeof( VisitorSerialization.SampleClass ) );
                TextWriter w = new StreamWriter( _path);
                s.Serialize( w, parameter as VisitorSerialization.SampleClass );
                w.Close();
            }
        }
    }
}


namespace VisitorSerialization2 {
    class SampleClass : IBase {
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
        public void Accept( IVisitor visitor) {
            visitor.Process( this);
        }
    }
    public class VisitorImplementation : IVisitor {
        private string _path;
        
        public VisitorImplementation( string path) {
            _path = path;
        }
        public void Process< type>(type parameter) where type: class{
            if( parameter is VisitorSerialization2.SampleClass) {
                XmlTextWriter writer = new XmlTextWriter( _path, null);
                SampleClass cls = parameter as SampleClass;
                writer.WriteStartDocument();
                writer.WriteStartElement( "SampleClass");
                writer.WriteStartElement( "Value");
                writer.WriteString( XmlConvert.ToString( cls.Value));
                writer.WriteEndElement();
                writer.WriteStartElement( "Buffer");
                writer.WriteString( cls.Buffer);
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
        }
    }
}

namespace Hierachy1 {
    class SingleClass : IBase {
        private string _buffer;
        
        public SingleClass( string buffer) {
            _buffer = buffer;
        }
        public string Buffer {
            get {
                return _buffer;
            }
        }
        public void Accept( IVisitor visitor) {
            visitor.Process( this);
        }
    }
    
    class CollectionClass : IEnumerable, IBase {
        private IList< SingleClass> _elements = new List< SingleClass>();
        
        public CollectionClass() { }
        public void AddElement( SingleClass cls) {
            _elements.Add( cls);
        }
        public IEnumerator GetEnumerator() {
            return _elements.GetEnumerator();
        }
        public void Accept( IVisitor visitor) {
            visitor.Process( this);
        }
    }
    
    public class VisitorImplementation : IVisitor {
        public void Process< type>(type parameter) where type: class{
            if( parameter is CollectionClass) {
                Console.WriteLine( "CollectionClass");
                CollectionClass cls = parameter as CollectionClass;
                foreach( SingleClass item in cls) {
                    item.Accept( this);
                }
            }
            else if( parameter is SingleClass) {
                SingleClass cls = parameter as SingleClass;
                Console.WriteLine( "SingleClass (" + cls.Buffer + ")");
            }
        }
    }
}



namespace Hierachy2 {
    class SingleClass : IBase {
        private string _buffer;
        
        public SingleClass( string buffer) {
            _buffer = buffer;
        }
        public string Buffer {
            get {
                return _buffer;
            }
        }
        public void Accept( IVisitor visitor) {
            visitor.Process( this);
        }
    }
    
    class CollectionClass : IEnumerable, IBase {
        private IList< SingleClass> _elements = new List< SingleClass>();
        
        public CollectionClass() { }
        public void AddElement( SingleClass cls) {
            _elements.Add( cls);
        }
        public IEnumerator GetEnumerator() {
            return _elements.GetEnumerator();
        }
        public void Accept( IVisitor visitor) {
            visitor.Process( this);
            foreach( SingleClass element in _elements) {
                element.Accept( visitor);
            }
        }
    }
    
    public class VisitorImplementation : IVisitor {
        public void Process< type>(type parameter) where type: class{
            if( parameter is CollectionClass) {
                Console.WriteLine( "CollectionClass");
            }
            else if( parameter is SingleClass) {
                SingleClass cls = parameter as SingleClass;
                Console.WriteLine( "SingleClass (" + cls.Buffer + ")");
            }
        }
    }
}



[TestFixture]
public class TestVisitorSerialization {
    [Test]
    public void TestVisitorImplementation() {
        VisitorSerialization.VisitorImplementation visitor = new VisitorSerialization.VisitorImplementation( @"c:\sampleclass.xml");
        
        IBase @base = new VisitorSerialization.SampleClass( 20, "VisitorSerialization.SampleClass");
        @base.Accept( visitor);
    }
    [Test]
    public void TestVisitorImplementation2() {
        VisitorSerialization2.VisitorImplementation visitor = new VisitorSerialization2.VisitorImplementation( @"c:\sampleclass2.xml");
        
        IBase @base = new VisitorSerialization2.SampleClass( 20, "VisitorSerialization2.SampleClass");
        @base.Accept( visitor);
    }
    [Test]
    public void TestHierachy1() {
        Hierachy1.VisitorImplementation visitor = new Hierachy1.VisitorImplementation();

        Hierachy1.CollectionClass collection = new Hierachy1.CollectionClass();
        collection.AddElement( new Hierachy1.SingleClass( "value1"));
        collection.AddElement( new Hierachy1.SingleClass( "value2"));
        collection.AddElement( new Hierachy1.SingleClass( "value3"));
        collection.AddElement( new Hierachy1.SingleClass( "value4"));
        collection.Accept( visitor);
    }
    [Test]
    public void TestHierachy2() {
        Hierachy2.VisitorImplementation visitor = new Hierachy2.VisitorImplementation();
        
        Hierachy2.CollectionClass collection = new Hierachy2.CollectionClass();
        collection.AddElement( new Hierachy2.SingleClass( "value1"));
        collection.AddElement( new Hierachy2.SingleClass( "value2"));
        collection.AddElement( new Hierachy2.SingleClass( "value3"));
        collection.AddElement( new Hierachy2.SingleClass( "value4"));
        collection.Accept( visitor);
    }
}


