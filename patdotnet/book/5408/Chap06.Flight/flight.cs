using System;
using System.Collections.Generic;
using Devspace.Commons.Functors;

    
interface IFlight {
    string Origin { get; set; }
    string Destination { get; set;}
    IFlight NextLeg { get; set; }
}
    
class FlightObjectEquals : IFlight {
    private string _origin;
    private string _destination;
    IFlight _nextLeg;
    
    public String Origin {
        get {
            return _origin;
        }
        set {
            _origin = value;
        }
    }
    
    public String Destination {
        get {
            return _destination;
        }
        set {
            _destination = value;
        }
    }
    
    
    public IFlight NextLeg {
        get {
            return _nextLeg;
        }
        set {
            if( !this.Equals( value)) {
                _nextLeg = value;
            }
        }
    }
}


class FlightAlreadyPresentException : Exception {
    
}

class Ticket {
    private IList< IFlight> _flightLegs = new List< IFlight>();
    
    public void AddLeg( IFlight leg) {
        foreach( IFlight flight in _flightLegs) {
            if( flight.Equals( leg)) {
                throw new FlightAlreadyPresentException();
            }
        }
        _flightLegs.Add( leg);
    }
}

class Flight : IFlight {
    private string _origin;
    private string _destination;
    IFlight _nextLeg;
    
    public String Origin {
        get {
            return _origin;
        }
        set {
            _origin = value;
        }
    }
    
    public String Destination {
        get {
            return _destination;
        }
        set {
            _destination = value;
        }
    }
    
    
    public IFlight NextLeg {
        get {
            return _nextLeg;
        }
        set {
            _nextLeg = value;
        }
    }
}

class FlightComparer : IFlight {
    IFlight _parent;
    DelegateComparer< IFlight, IFlight> _delegateComparer;
    
    public FlightComparer( IFlight parent, DelegateComparer< IFlight, IFlight> delg) {
        _delegateComparer = delg;
        _parent = parent;
    }
    
    public String Origin {
        get {
            return _parent.Origin;
        }
        set {
            _parent.Origin = value;
        }
    }
    
    public String Destination {
        get {
            return _parent.Destination;
        }
        set {
            _parent.Destination = value;
        }
    }
    
    public IFlight NextLeg {
        get {
            return _parent.NextLeg;
        }
        set {
            if( _delegateComparer( _parent, value) != 0) {
                _parent.NextLeg = value;
            }
            else {
                throw new ComparerEvaluationException();
            }
        }
    }
}





