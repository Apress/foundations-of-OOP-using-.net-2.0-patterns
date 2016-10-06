using System;
using System.Text;
using NUnit.Framework;

namespace Attempt1 {
    class Rectangle {
        private long _length, _width;
        
        public Rectangle( long length, long width) {
            _length = length;
            _width = width;
        }
        public virtual long Length {
            get {
                return _length;
            }
            set {
                _length = value;
            }
        }
        public virtual long Width {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
    }
    
    class Square : Rectangle {
        public Square( long width) : base( width, width) {
            
        }
    }
}

namespace Attempt2 {
    class Rectangle {
        private long _length, _width;
        
        public Rectangle( long length, long width) {
            _length = length;
            _width = width;
        }
        public virtual long Length {
            get {
                return _length;
            }
            set {
                _length = value;
            }
        }
        public virtual long Width {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
    }
    
    class Square : Rectangle {
        public Square( long width) : base( width, width) {
            
        }
        public override long Length {
            get {
                return base.Length;
            }
            set {
                base.Length = value;
                base.Width = value;
            }
        }
        public override long Width {
            get {
                return base.Width;
            }
            set {
                base.Length = value;
                base.Width = value;
            }
        }
    }
}

namespace Attempt3 {
    class Square {
        private long _width;
        
        public Square( long width)  {
            _width = width;
        }
        public virtual long Width {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
    }

    class Rectangle : Square {
        private long _length;
        
        public Rectangle( long length, long width) : base(width) {
            _length = length;
        }
        public virtual long Length {
            get {
                return _length;
            }
            set {
                _length = value;
            }
        }
    }
    
}


namespace Attempt4 {
    interface IShape {
        long Length {
            get; set;
        }
        long Width {
            get; set;
        }
    }
    
    class Rectangle : IShape {
        private long _width;
        private long _length;
        
        public Rectangle( long width)  {
            _width = width;
        }
        public virtual long Width {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
        public virtual long Length {
            get {
                return _length;
            }
            set {
                _length = value;
            }
        }
    }
    
    class Square : IShape {
        private long _width;
        
        public Square( long width) {
            _width = width;
        }
        public virtual long Length {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
        public virtual long Width {
            get {
                return _width;
            }
            set {
                _width = value;
            }
        }
    }
    
}

class Rectangle {
    private readonly long _length, _width;
    
    public Rectangle( long length, long width) {
        _length = length;
        _width = width;
    }
    public virtual long Length {
        get {
            return _length;
        }
    }
    public virtual long Width {
        get {
            return _width;
        }
    }
}

class Square : Rectangle {
    public Square( long width) : base( width, width) {
        
    }
}

[TestFixture]
public class TestShapes {
    [Test]
    public void TestConsistencyProblems() {
        Attempt1.Square square = new Attempt1.Square( 10);
        Attempt1.Rectangle squarishRectangle = square;
        
        squarishRectangle.Length = 20;
        Assert.AreNotEqual( square.Length, square.Width);
    }
    [Test]
    public void TestWorkingButComplicated() {
        Attempt2.Square square = new Attempt2.Square( 10);
        Attempt2.Rectangle squarishRectangle = square;
        
        squarishRectangle.Length = 20;
        Assert.AreEqual( square.Length, square.Width);
    }
    void TestSquare( Attempt3.Square square) {
        square.Width = 10;
    }
    [Test]
    public void TestSubclass() {
        Attempt3.Rectangle rectangle = new Attempt3.Rectangle( 30, 40);
        long oldLength = rectangle.Length;
        TestSquare( rectangle);
        Assert.AreEqual( oldLength, rectangle.Length);
    }
}
