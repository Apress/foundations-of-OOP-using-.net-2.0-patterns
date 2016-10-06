using System;
using NUnit.Framework;
using Chap03;

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
}




