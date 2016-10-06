using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Threading;

namespace UserReference {
    /***
    There are multiple ways of defining classes in a common assembly shared
    by multiple other assemblies and applications
    
    // Used when wanting to abstract intention from implementation
    public interface xxx {
    
    }
    
    // Used when wanting to implement some default functionality that is shared
    // in multiple places
    abstract class xxx {
        
    }
    // Used when wanting to share a class in multiple places that includes
    // some functionality.  The class is sealed so that others cannot derive
    // from it reducing the number of dependencies
    sealed class xxx {
        
    }
    
    
    
    ***/
	public interface IUser {
		string name { get; set; }
		string street { get; set; }
		string city { get; set; }
        string country { get; set; }
	}

    public interface IOperations {
        IUser LoadUser( string reference);
    }
}
