
using System;
using System.Text;

class ConcurrentAccess {
    private int _a;
    
    public void AssignVariable( int a) {
        lock( this) {
            _a = a;
        }
    }
    public void AssignAndIncrement( int a) {
        lock( this) {
            _a = a;
            _a ++;
        }
    }
}

