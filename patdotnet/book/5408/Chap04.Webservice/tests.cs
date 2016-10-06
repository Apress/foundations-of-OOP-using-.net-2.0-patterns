using System;
using NUnit.Framework;

[TestFixture]
public class TestWebService {
    [Test]
    public void RunRawWebService() {
        ITestServiceSoap srvc = Chap04.Webservice.Factory.Create();

        Assert.AreEqual( "hello", srvc.Echo( "hello" ) );
    }
}