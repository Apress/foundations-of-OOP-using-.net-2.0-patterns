using System;
using NUnit.Framework;
using Chap03;
using Taxation;

// mono ../../apps/nunit-2.2.0/bin/nunit-console.exe chap03.exe


[TestFixture]
public class IntroTests {
    private string _strHelloAnybodyThere =  "hello anybody there";

    [Test]public void SimpleBridge() {
        Intention obj = Factory.Instantiate();
        Chap03MockObjects.Callback.CBFeedbackString =
            new Chap03MockObjects.FeedbackString( this.CallbackSimpleBridge);
        obj.Echo( _strHelloAnybodyThere);
    }
    void CallbackSimpleBridge( string message) {
        string test = "From the console " + _strHelloAnybodyThere;
        if( message != test) {
            throw new Exception();
        }
    }

    [Test]public void DotNet1dot1Test() {
        IMathematicsObj obj = FactoryIMathematicsObj.Instantiate();

        Assert.AreEqual( 3, obj.Add( 1, 2), "Incorrect Addition");
    }


    [Test]
    [ExpectedException(typeof(InvalidCastException))]
    public void DotNet1dot1Test2() {
        IMathematicsObj obj = FactoryIMathematicsObj.Instantiate();

        Assert.AreEqual( 3, obj.Add( "hello", 2), "Incorrect Addition");
    }
    [Test]public void AddListDotNet1() {
        MathBridgeDotNet1.Operations ops = new MathBridgeDotNet1.Operations();
        ops.Math = MathBridgeDotNet1.Factory.Instantiate();

        int[] values = new int[] {1,2,3,4,5};

        Assert.AreEqual( 15, ops.AddArray( values), "List did not add");
    }

#if GenericsSupported
    [Test]public void AddListDotNet2() {
        MathBridgeDotNet2.Operations< int> ops = new MathBridgeDotNet2.Operations< int>();
        ops.Math = MathBridgeDotNet2.Factory.Instantiate();

        int[] values = new int[] {1,2,3,4,5};

        Assert.AreEqual( 15, ops.AddArray( values), "List did not add");
    }

#endif
}


internal class MockNotImplemented : Devspace.Commons.TDD.ApplicationFatalException {
    public MockNotImplemented( ) : base( "method not implemented") {
    }
}

internal class MockBaseTaxation : BaseTaxation {
    public override Decimal CalculateTax() {
        throw new MockNotImplemented();
    }
}


internal class MockBaseTaxationRequiresCall : BaseTaxation {
    private bool _didCall;
    public MockBaseTaxationRequiresCall() {
        _didCall = false;
    }
    public bool DidCall {
        get { return _didCall; }
    }
    public override Decimal CalculateTax() {
        _didCall = true;
        return new Decimal();
    }
}

internal class MockIncome : IIncomes {
    public void SampleMethod() {
        throw new MockNotImplemented();
    }
}

[TestFixture]public class TaxTests {
    [Test]public void TestAssignIncomeProperty() {
        IIncomes[] inc = new IIncomes[ 1];
        inc[ 0] = new MockIncome();
        ITaxation taxation = new MockBaseTaxation();
        taxation.Incomes = inc;
        Assert.AreEqual( inc, taxation.Incomes, "Not same object");
    }
    [Test]
    [ExpectedException(typeof(PropertyNotDefined))]
    public void TestRetrieveIncomeProperty() {
        ITaxation taxation = new MockBaseTaxation();
        IIncomes[] inc = taxation.Incomes;
    }
    [Test]public void TestDidCallMethod() {
        MockBaseTaxationRequiresCall taxation = new MockBaseTaxationRequiresCall();
        // Call some methods
        Assert.IsTrue( taxation.DidCall);
    }
}





