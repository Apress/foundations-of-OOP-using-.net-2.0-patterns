using System.Collections.Generic;
using System;
using Devspace.Commons.Functors;

namespace Proxy {
    class SynchronizedList<type>: ICollection<type> {
        private ICollection<type> _parent;

        public SynchronizedList(ICollection<type> parent) {
            _parent = parent;
        }

        #region ICollection<type> Members

        public void Add(type item) {
            lock(this) {
                _parent.Add(item);
            }
        }

        public void Clear() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public bool Contains(type item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public void CopyTo(type[] array, int arrayIndex) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool IsReadOnly {
            get { throw new System.Exception("The method or operation is not implemented."); }
        }

        public bool Remove(type item) {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<type> Members

        public IEnumerator<type> GetEnumerator() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new System.Exception("The method or operation is not implemented.");
        }

        #endregion
    }
    
    interface IFlight {
        string Origin { get; set; }
        string Destination { get; set;}
        IFlight NextLeg { get; set; }
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
    
    class Proxy {
        public static void Method() {
            IList<string> list = new System.Collections.Generic.List<string>() as IList<string>;
            
            List<string> list2 = new List<string>();
            
            //List<string> synchronized = List<string>.Synchronize( new List());
        }
        public static void OtherExample() {
            ICollection<string> collection = new SynchronizedList<string>(new List<string>());
        }
    }
}




