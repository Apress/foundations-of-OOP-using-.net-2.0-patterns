using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;


namespace XMLSerialization {
    [XmlRoot("SampleClass")]
    public class SampleClass {
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
            internal set {
                _value = value;
            }
        }
        [XmlElement( "Buffer")]
        public string Buffer {
            get {
                return _buffer;
            }
            internal set {
                _buffer = value;
            }
        }
    }
   
}

// Shopping list class which will be serialized
[XmlRoot("shoppingList")]
public class ShoppingList {
    private ArrayList listShopping;
    
    public ShoppingList() {
        listShopping = new ArrayList();
    }
    
    [XmlElement("item")]
    public Item[] Items {
        get {
            Item[] items = new Item[ listShopping.Count ];
            listShopping.CopyTo( items );
            return items;
        }
        set {
            if( value == null ) return;
            Item[] items = (Item[])value;
            listShopping.Clear();
            foreach( Item item in items )
                listShopping.Add( item );
        }
    }
    
    public int AddItem( Item item ) {
        return listShopping.Add( item );
    }
}

// Items in the shopping list
public class Item {
    [XmlAttribute("name")] public string name;
    [XmlAttribute("price")] public double price;
    
    public Item() {
    }
    
    public Item( string Name, double Price ) {
        name = Name;
        price = Price;
    }
}

[TestFixture]
public class TestXMLSerialization {
    [Test]
    public void TestSerialization() {
        ShoppingList myList = new ShoppingList();
        myList.AddItem( new Item( "eggs",1.49 ) );
        myList.AddItem( new Item( "ground beef",3.69 ) );
        myList.AddItem( new Item( "bread",0.89 ) );
        
// Serialization
        XmlSerializer s = new XmlSerializer( typeof( ShoppingList ) );
        TextWriter w = new StreamWriter( @"c:\list.xml" );
        s.Serialize( w, myList );
        w.Close();
        
// Deserialization
    }
    [Test]
    public void TestDeserialization() {
        ShoppingList newList;
        XmlSerializer s = new XmlSerializer( typeof( ShoppingList ) );
        TextReader r = new StreamReader( @"c:\list.xml" );
        newList = (ShoppingList)s.Deserialize( r );
        r.Close();
    }
    
    [Test]
    public void TestSampleClassSerialization() {
        XMLSerialization.SampleClass cls = new XMLSerialization.SampleClass( 10, "hello");
        Assert.AreEqual( 10, cls.Value);
        XmlSerializer s = new XmlSerializer( typeof( XMLSerialization.SampleClass ) );
        TextWriter w = new StreamWriter( @"c:\sampleclass.xml" );
        s.Serialize( w, cls );
        w.Close();
    }
    [Test]
    public void TestSampleClassDeserialization() {
        XMLSerialization.SampleClass cls;
        XmlSerializer s = new XmlSerializer( typeof( XMLSerialization.SampleClass ) );
        TextReader r = new StreamReader( @"c:\sampleclass.xml" );
        cls = (XMLSerialization.SampleClass)s.Deserialize( r );
        r.Close();
        Assert.AreEqual( 10, cls.Value);
    }
}


