using System;
using DbgMgr;
using System.Configuration;
using System.Collections.Specialized;
using UserReference;

class MainClass
{
    public static void RunDynamic() {
        string @class = ConfigurationSettings.AppSettings[ "classload"];
        DebugMgr.outputVar( 8, "class to load", @class);

        IOperations operations = (IOperations)(Reference.Instance().GetObject( @class));
        if( operations == null) {
            DebugMgr.output( 1, "!!! Operations is a null object !!!");
        }
        IUser user = operations.LoadUser("some.identifier");
        DebugMgr.outputVar( 8, "name", user.name);
        DebugMgr.outputVar( 8, "street", user.street);
        DebugMgr.outputVar( 8, "city", user.city);
        DebugMgr.outputVar( 8, "country", user.country);
    }
	public static void Main(string[] args)
	{
        DebugMgr.assignDebugFlags( 10);
        DebugMgr.start( 8, "Main");
        try {
            RunDynamic();
        }
        catch( Exception ex) {
            DebugMgr.output( 1, "!!! " + ex.Message + " !!!");
        }
        DebugMgr.end( 8);
	}
}
