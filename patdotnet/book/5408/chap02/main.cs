using System;
using com.devspace.commons.Tracer;
#if LOG4NET
using log4net;
#endif

namespace Chap02SectImplError {
    class BadExceptionUsage {
        public static void ExampleMethod() {
            CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
            int[] args = new int[ 10];

            try {
                for( int c1 = 0; ; c1 ++) {
                    try {
                        CallTracer.Output( "Counter " + c1);
                    }
                    catch( Exception ex) {
                        ;
                    }
                    args[ c1] = 1;
                }
            }
            catch( IndexOutOfRangeException excep) {
                CallTracer.Output( "Yupe hit the end of the array");
            }
            CallTracer.End();
        }
    }

    class BadExampleErrorHandling {
        public static int[] GetArrayValues() {
            return null;
        }
        public static string GetStringValue() {
            return null;
        }
    }


    class BetterExampleErrorHandling {
        private static readonly string EmptyValue = "";

        public static bool IsArrayValid {
            get {
                return false;
            }
        }
        public static int[] GetArrayValues() {
            return new int[ 0];
        }
        public static string GetStringValue() {
            return EmptyValue;
        }
    }

    class ProcessErrorHandlingExamples {
        public static void BadExamples() {
            CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
            int[] arr = BadExampleErrorHandling.GetArrayValues();
            if( arr != null) {
                for( int c1 = 0; c1 < arr.Length; c1 ++) {
                    CallTracer.Output( "Value is " + arr[ c1]);
                }
            }

            string value = BadExampleErrorHandling.GetStringValue();
            if( value != null) {
                if( value.Length > 0) {
                    CallTracer.Output( "Not an empty String");
                }
            }
            CallTracer.End();
        }
        public static void BetterExamples() {
            CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
            int[] arr = BetterExampleErrorHandling.GetArrayValues();
            for( int c1 = 0; c1 < arr.Length; c1 ++) {
                CallTracer.Output( "Value is " + arr[ c1]);
            }
            if( BetterExampleErrorHandling.GetStringValue().Length > 0) {
                CallTracer.Output( "Not an empty String");
            }
            CallTracer.End();
        }
        public static void TestExamples() {
            CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
            CallTracer.Output("You should not get any output other than method names in this method context");
            BetterExamples();
            BadExamples();
            CallTracer.End();
        }
    }
}

class ExampleDefault {
    private int _value;

    ExampleDefault() {
        AssignDefault();
    }
    ExampleDefault( int param) {
        _value = param;
    }
    public void AssignDefault() {
        _value = 12;
    }
}

class ExampleSingleEntrySingleExit {
    private int _defaultValue = 12;

    public int MethodNormal( int param1) {
        if( param1 < 10) {
            return param1 / 2;
        }
        else if( param1 > 100) {
            throw new Exception();
        }
        return _defaultValue;
    }
    public int MethodSeSx( int param1) {
        int retval = _defaultValue;
        if( param1 < 10) {
            retval = param1 / 2;
            goto exit_method;
        }
        else if( param1 > 100) {
            throw new Exception();
        }
    exit_method:
        return retval;
    }
}

class ProgramState {
    private int _variable1;
    private int _variable2;

    public void AssignState( int param1) {
        CallTracer.Output( "AssignState: Entering");
        if( param1 < 1) {
            CallTracer.Output( "AssignState: Throwing the exception");
            throw new Exception();
        }
        _variable2 = param1;
        CallTracer.Output( "AssignState: Exiting");
    }
    public void DoSomething( int param1, int param2) {
        CallTracer.Output( "DoSomething: Entering");
        _variable1 = param1;
        AssignState( param2);
        CallTracer.Output( "DoSomething: Exiting");    
    }
    public void DoSomething2( int param1, int param2) {
        CallTracer.Output( "DoSomething2: Entering");
        int temp = _variable1;
        try {
            _variable1 = param1;
            AssignState( param2);
        }
        finally {
            _variable1 = temp;
            CallTracer.Output( "DoSomething2: Exception in gear, but finally");
        }
        CallTracer.Output( "DoSomething2: Exiting");
    }
    public void DoSomething3( int param1, int param2) {
        CallTracer.Output( "DoSomething3: Entering");
        int temp = param1;
        AssignState( param2);
        param1 = temp;
        CallTracer.Output( "DoSomething3: Exiting");
    }
}

public class MyObject {
#if LOG4NET
    private static readonly ILog log = LogManager.GetLogger( "logger.production");
#endif

    public void DoSomething( int val) {
#if LOG4NET
        if( val < 0 && log.IsErrorEnabled) {
            log.Error( "oops wrong value");
            return;
        }
#endif
    }

    public void DoSomethingWorks( int val) {
#if LOG4NET
        if( val < 0) {
            if( log.IsErrorEnabled) {
                log.Error( "oops wrong value");
            }
            return;
        }
#endif
    }
    public void DoSomethingAssert( int val) {
        Assert.IsFalse( val < 0);
        return;
    }
}

class MyException : Exception {
    private string _message;

    public MyException( string message) {
        _message = message;
    }
}
class ExceptionExamples {
    public ExceptionExamples() {

    }
    public void ThrowException() {
        throw new Exception( "My error");
    }
}

class AssertionFailure : ApplicationFatalException {
    public AssertionFailure( string message) : base( message) {
    }
    public static Exception Instantiate() {
        return new AssertionFailure( "Could not assert");
    }
}

public class MainApp
{
    public static void Assertions() {
        CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
        int param1 = 10;
        Assert.IsTrue( param1 > 0, 
                       new InstantiateException( AssertionFailure.Instantiate));
        param1 = -1;
        CallTracer.Output( "No exception at this point");
        Assert.IsTrue( param1 > 0, 
                       new InstantiateException( AssertionFailure.Instantiate));

        CallTracer.End();
    }
    public static void SimpleLogging() {
#if LOG4NET
        ILog log = LogManager.GetLogger( "Chapter2.Logging");

        if (log.IsDebugEnabled) 
            log.Debug("Hello world");
#endif
    }
	public static void InheritedLogging() {
#if LOG4NET
		ILog log = LogManager.GetLogger( "a.b.c");

        if (log.IsDebugEnabled) 
            log.Debug("Debug Message");

        if (log.IsInfoEnabled) 
            log.Info("Info Message");

        if (log.IsWarnEnabled) 
            log.Warn("Warning Message");
        
		if (log.IsErrorEnabled) 
            log.Error("Error Message");
        
		if (log.IsFatalEnabled) 
            log.Fatal("Fatal Message");
#endif
	}
	public static void DoSomething() {
        CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
		MyObject obj = new MyObject();
		obj.DoSomething( -1);
        try {
            obj.DoSomethingAssert( -1);
        }
        catch( Exception except) {
            CallTracer.Output( except.Message);
            ;
        }
        CallTracer.End();
	}
    public static void DifferenceErrorAndException() {
        CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
        Chap02SectImplError.BadExceptionUsage.ExampleMethod();
        CallTracer.End();
    }
    public static void RunProgramState() {
        CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
        try {
            ProgramState obj = new ProgramState();
            CallTracer.Output( "*****************************************");
            obj.DoSomething( 10, 10);
            CallTracer.Output( "*****************************************");
            obj.DoSomething2( 0, 0);
        }
        catch( Exception ex) {
            CallTracer.Output( "RunProgramState: Caught an exception");
        }
        CallTracer.End();
    }
    public static void RunExceptions() {
        CallTracer.Start( System.Reflection.MethodBase.GetCurrentMethod());
        ExceptionExamples obj = new ExceptionExamples();
        try {
            obj.ThrowException();
        }
        catch( Exception ex) {
            CallTracer.Output( "----\n" + ex.TargetSite.ToString() + "\n-----");
            CallTracer.Output( "----\n" + ex.Source + "\n-----");
            CallTracer.Output( "----\n" + ex.StackTrace + "\n----");
        }
        CallTracer.End();
    }
    public static void Main(string[] args)
    {
        CallTracer.IsActive = true;
        DifferenceErrorAndException();
        Chap02SectImplError.ProcessErrorHandlingExamples.TestExamples();
        SimpleLogging();
		InheritedLogging();
		DoSomething();
        RunProgramState();
        RunExceptions();
        Assertions();
    }
}




