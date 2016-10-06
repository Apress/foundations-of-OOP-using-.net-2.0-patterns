using System;

namespace Experimentations {
    public class Descriptor {
        private int _id;
        private string _description;
        
        public Descriptor() { }
        public Descriptor( int id, string description) {
            _id = id;
            _description = description;
        }
        
        public int ID {
            get {
                return _id;
            }
            internal set {
                _id = value;
            }
        }
        public string Description {
            get {
                return _description;
            }
            internal set {
                _description = value;
            }
        }
    }
}

