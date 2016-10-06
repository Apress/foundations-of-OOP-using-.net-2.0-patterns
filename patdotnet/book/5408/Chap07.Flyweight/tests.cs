using System;
using System.Text;
using NUnit.Framework;


[TestFixture]
public class TestString {
    [Test]
    public void Test() {
        String flyweight = "flyweight", pattern = "pattern";
        String flyweight2 = "flyweight", pattern2 = "pattern";
        
        // Test 1
        Assert.IsTrue( Object.ReferenceEquals( flyweight, flyweight2));
        // Test 2
        Assert.IsTrue(Object.ReferenceEquals( pattern, pattern2));
        
        String distinctString = flyweight + pattern;
        // Test 3
        Assert.IsFalse( Object.ReferenceEquals( distinctString, "flyweightpattern"));
        
        String flyweightpattern = String.Intern(flyweight + pattern);
        // Test 4
        Assert.IsTrue( Object.ReferenceEquals( flyweightpattern,"flyweight"));
    }
    [Test]
    public void TestFlyweightReadOnly() {
        ImmutableClass cls1 = new ImmutableClass( "hello", 10);
        ImmutableClass cls2 = new ImmutableClass( "hello", 10);
        
        Assert.IsFalse( Object.ReferenceEquals( cls1, cls2));
    }
}

