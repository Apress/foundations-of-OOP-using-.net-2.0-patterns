using System;
using System.Text;
using NUnit.Framework;

[TestFixture]
public class TestExtensions {
    [Test]
    public void TestStaticExtension() {
        Extensions.IBase1 base1 = new StaticExtensions.ImplementationBoth();
        Extensions.IBase2 base2 = base1 as Extensions.IBase2;
        
        Assert.IsTrue( base1 is Extensions.IBase2);
        Assert.AreEqual( 1, base1.Value());
        Assert.AreEqual( 1, base2.Value());
    }
    [Test]
    public void TestDoubleStaticExtension() {
        StaticExtensions.ImplementationBothSeparate cls = new StaticExtensions.ImplementationBothSeparate();
        Extensions.IBase1 base1 = cls;
        Extensions.IBase2 base2 = base1 as Extensions.IBase2;
        
        Assert.AreEqual( 0, cls.Value());
        Assert.AreEqual( 1, base1.Value());
        Assert.AreEqual( 2, base2.Value());
    }
    [Test]
    public void TestDoubleInherited() {
        StaticExtensions.Implementation cls = new StaticExtensions.Implementation();
        Extensions.IBase1 base1 = cls;
        
        Assert.AreEqual( 1, cls.Value());
        Assert.IsFalse( cls is Extensions.IBase2);
        
        cls = new StaticExtensions.Derived();
        base1 = cls;
        Assert.AreEqual( 1, cls.Value());
        Assert.IsTrue( base1 is Extensions.IBase2);
        
        Extensions.IBase2 base2 = base1 as Extensions.IBase2;
        Assert.AreEqual( 2, base2.Value());
    }
    [Test]
    public void TestDoubleVirtual() {
        StaticExtensions.ImplementationVirtual cls = new StaticExtensions.ImplementationVirtual();
        Extensions.IBase1 base1 = cls;
        
        Assert.AreEqual( 1, cls.Value());
        Assert.IsFalse( cls is Extensions.IBase2);
        
        cls = new StaticExtensions.DerivedVirtual();
        base1 = cls;
        Assert.AreEqual( 2, cls.Value());
        Assert.IsTrue( base1 is Extensions.IBase2);
        
        Extensions.IBase2 base2 = base1 as Extensions.IBase2;
        Assert.AreEqual( 2, base2.Value());
    }
}
