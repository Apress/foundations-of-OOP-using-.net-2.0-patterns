using System;
using System.Reflection;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Threading;
using DbgMgr;

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
    public class Reference {
        private static Reference _instance;

        private string _path;
        private string[] _pluginEntries;
        private ArrayList _assemblies = new ArrayList();

        public string PluginPath 
        {
            get 
            {
                return _path;
            }
        }

        // Constructor is made private so that only 
        // the class can instantiate itself
        private Reference() 
        {
            DebugMgr.start( 9, "Reference.Reference");
            NameValueCollection pluginConfig = (NameValueCollection) ConfigurationSettings.GetConfig("settings/plugin");

            if(pluginConfig["dir"] == null) {
                throw(new ConfigurationException("Incorrect Plugin Directory"));
            }
            else {
                _path = pluginConfig[ "dir"];
                DebugMgr.outputVar( 9, "Plugin directory", _path);
                _pluginEntries = Directory.GetFileSystemEntries( _path, "*.dll");
                OperatingSystem os = System.Environment.OSVersion;
                DebugMgr.outputVar( 9, "Operating System", os.Platform);
                foreach( string path in _pluginEntries) {
                    DebugMgr.outputVar( 9, "assembly", path);
                    Assembly assembly;
                    if( (int)os.Platform == 128) {
                        // This is Linux
                        assembly = Assembly.LoadFrom( path);
                    }
                    else {
                        // This is everything else
                        assembly = Assembly.Load( AssemblyName.GetAssemblyName( path));
                    }
                    _assemblies.Add( assembly);
                }
            }
            DebugMgr.end( 9);
        }
        public static Reference Instance() 
        {
            Monitor.Enter( typeof(Reference));
            if( _instance == null) 
            {
                _instance = new Reference();
            }
            Monitor.Exit( typeof(Reference));
            return _instance;
        }

        public Object GetObject( string @class) 
        {
            Object retval = null;
            DebugMgr.start( 9, "Reference.GetObject");
            DebugMgr.outputVar( 9, "Class to instantiate", @class);
            foreach( Assembly assembly in _assemblies) 
            {
                Object @object;

                @object = assembly.CreateInstance( @class);
                if( @object != null) 
                {
                    DebugMgr.outputVar( 9, "loaded object", @class);
                    retval = @object;
                    break;
                }
            }
            DebugMgr.end( 9);
            return retval;
        }
    }	
}
