using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Chap06.WhatAboutReferences {
    public class MyBuffer {
        private string _buffer;
        public MyBuffer(string input) {
            _buffer = input;
        }
        public override bool Equals(object obj) {
            if(GetHashCode() == obj.GetHashCode()) {
                return true;
            }
            return false;
        }
        public override int GetHashCode() {
            return _buffer.GetHashCode();
        }
    }

    public class MyBufferDotNet {
        private string _buffer;
        public MyBufferDotNet(string input) {
            _buffer = input;
        }
    }
}



[TestFixture]
public class TestReferences {
    [Test]
    public void TestEqualsThatDoesNotWork() {
        Chap06.WhatAboutReferences.MyBufferDotNet cls1 = new Chap06.WhatAboutReferences.MyBufferDotNet("hello");
        Chap06.WhatAboutReferences.MyBufferDotNet cls2 = new Chap06.WhatAboutReferences.MyBufferDotNet("hello");

        Assert.IsFalse(cls1.Equals(cls2));
    }
    [Test]
    public void TestEqualsThatDoesWork() {
        Chap06.WhatAboutReferences.MyBuffer cls1 = new Chap06.WhatAboutReferences.MyBuffer("hello");
        Chap06.WhatAboutReferences.MyBuffer cls2 = new Chap06.WhatAboutReferences.MyBuffer("hello");

        Assert.IsTrue(cls1.Equals(cls2));
    }
    [Test]
    public void TestCollectionThatReturnsWhat() {
        IList<Chap06.WhatAboutReferences.MyBuffer> list = new List<Chap06.WhatAboutReferences.MyBuffer>();
        Chap06.WhatAboutReferences.MyBuffer cls1 = new Chap06.WhatAboutReferences.MyBuffer("hello");
        Chap06.WhatAboutReferences.MyBuffer cls2 = new Chap06.WhatAboutReferences.MyBuffer("hello");

        list.Add(cls1);
        list.Add(cls2);

        Assert.AreEqual(0, list.IndexOf(cls2));
    }
}