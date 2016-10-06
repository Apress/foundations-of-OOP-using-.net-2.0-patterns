using System;

class ImmutableClass {
    private readonly string _buffer;
    private readonly int _value;
    
    public ImmutableClass( string buffer, int value) {
        
    }
    public string Buffer {
        get {
            return _buffer;
        }
    }
    public int Value {
        get {
            return _value;
        }
    }
}


interface IBase {
}

class ImmutableImplementation : IBase{
    public ImmutableImplementation( int initialValue) {}
}

class Factory {
    static public IBase CreateImplementation( string parameter) {
        if( String.Compare( "somevalue", parameter) == 0) {
            return new ImmutableImplementation( 10);
        }
        throw new NotSupportedException();
    }
}

