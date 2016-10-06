using System;

class ConcurrentAccess {
    private int _a;
    
    public void AssignVariable( int a) {
        lock( this) {
            _a = a;
        }
    }
}

class Regular {
    private int _value;
    
    public Regular( int initial) {
        _value = initial;
    }
    public Regular Increment() {
        _value ++;
        return this;
    }
}

class Immutable {
    private readonly int _value;
    
    public Immutable( int initial) {
        _value = initial;
    }
    public Immutable Increment() {
        return new Immutable( _value + 1);
    }
}

struct structImmutable {
    private readonly int _value;
    public structImmutable( int initial) {
        _value = initial;
    }
    public structImmutable Increment() {
        return new structImmutable( _value + 1);
    }
}

