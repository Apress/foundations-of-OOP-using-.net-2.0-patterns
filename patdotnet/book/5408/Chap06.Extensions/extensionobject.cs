using System;
using System.Collections;
using NUnit.Framework;
using Devspace.Commons.TDD;
using System.Reflection;

namespace Extensions {
    public interface ISubject {
        type GetExtension< type>() where type: class;
    }
    
    public interface IExtendedSubject : ISubject {
        void AddExtension< type>( type extension) where type: class;
        void RemoveExtension< type>( type extension) where type: class;
    }

    public sealed class Subject : IExtendedSubject {
        bool InterfaceFilter(Type typeObj, Object criteriaObj) {
            return (typeObj.ToString() == criteriaObj.ToString());
        }
        bool DidImplement(string @interface, Type targetType) {
            TypeFilter interfaceFilter = new TypeFilter(InterfaceFilter);
            Type[] interfaces = targetType.FindInterfaces( interfaceFilter, @interface);
            return (interfaces.Length > 0);
        }
        
        
        ArrayList _list = new ArrayList();
        
        public type GetExtension< type>() where type: class{
            foreach( Object obj in _list) {
                if( DidImplement( typeof( type).FullName, obj.GetType())) {
                    return obj as type;
                }
            }
            return null;
        }
        public void AddExtension< type>( type extension) where type: class{
            if( extension == null) {
                throw new NotSupportedException();
            }
            _list.Add( extension);
        }
        public void RemoveExtension< type>( type extension) where type: class{
            for( int c1 = 0; c1 < _list.Count; c1 ++) {
                if( Object.ReferenceEquals( _list[ c1], extension)) {
                    _list.RemoveAt( c1);
                }
            }
        }
        public override string ToString() {
            MemoryTracer.Start( this);
            MemoryTracer.StartArray( "Extension Objects");
            foreach( Object obj in _list) {
                MemoryTracer.Variable( "type", obj.GetType().FullName);
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }
    
    public interface IBase1 {
        int Value();
    }
    public interface IBase2 {
        int Value();
    }
}



namespace StaticExtensions {
    public class ImplementationBoth : Extensions.IBase1, Extensions.IBase2 {
        public int Value() {
            return 1;
        }
    }
    
    public class ImplementationBothSeparate : Extensions.IBase1, Extensions.IBase2 {
        public int Value() {
            return 0;
        }
        int Extensions.IBase1.Value() {
            return 1;
        }
        int Extensions.IBase2.Value() {
            return 2;
        }
    }
    
    public class Implementation : Extensions.IBase1 {
        public int Value() {
            return 1;
        }
    }
    
    public class Derived : Implementation, Extensions.IBase2 {
        public new int Value() {
            return 2;
        }
    }
    
    public class ImplementationVirtual : Extensions.IBase1 {
        public virtual int Value() {
            return 1;
        }
    }
    public class DerivedVirtual : ImplementationVirtual, Extensions.IBase2 {
        public override int Value() {
            return 2;
        }
        
    }
}


namespace DynamicExtensions {
    public class Implementation1 : Extensions.IBase1, Extensions.ISubject {
        Extensions.Subject _subject;
        public Implementation1( Extensions.Subject subject) {
            _subject = subject;
        }
        public type GetExtension< type>() where type: class {
            return _subject.GetExtension< type>();
        }
        public int Value() {
            return 1;
        }
        public override string ToString() {
            MemoryTracer.Start( this);
            return MemoryTracer.End();
        }
    }
    
    public class Implementation2 : Extensions.IBase2, Extensions.ISubject {
        Extensions.Subject _subject;
        public Implementation2( Extensions.Subject subject) {
            _subject = subject;
        }
        public type GetExtension< type>()  where type: class{
            return _subject.GetExtension< type>();
        }
        public int Value() {
            return 2;
        }
        public override string ToString() {
            MemoryTracer.Start( this);
            return MemoryTracer.End();
        }
    }
    class Factory {
        public static Extensions.IBase1 CreateInstance() {
            Extensions.Subject main = new Extensions.Subject();
            
            DynamicExtensions.Implementation1 impl1 = new DynamicExtensions.Implementation1( main);
            DynamicExtensions.Implementation2 impl2 = new DynamicExtensions.Implementation2( main);
            
            main.AddExtension( impl1 as Extensions.IBase1);
            main.AddExtension( impl2 as Extensions.IBase2);
            Console.WriteLine( main.ToString());
            return impl1;
        }
    }
}

[TestFixture]
public class TestDynamicExtensions {
    public returntype Cast< inputtype, returntype>( inputtype input) where inputtype: class where returntype: class {
        if( input is returntype) {
            return input as returntype;
        }
        return ((Extensions.ISubject)input).GetExtension< returntype>();
    }
    [Test]
    public void TestSimple() {
        Extensions.IBase1 base1 = DynamicExtensions.Factory.CreateInstance();
        Extensions.IBase2 base2 = ((Extensions.ISubject)base1).GetExtension< Extensions.IBase2>();
        NUnit.Framework.Assert.IsNotNull( base2);
        NUnit.Framework.Assert.AreEqual( 2, base2.Value());
        base1 = ((Extensions.ISubject)base2).GetExtension< Extensions.IBase1>();
        NUnit.Framework.Assert.IsNotNull( base1);
        NUnit.Framework.Assert.AreEqual( 1, base1.Value());
    }
}



