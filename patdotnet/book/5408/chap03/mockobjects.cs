using Devspace.Commons.TDD;

namespace Chap03MockObjects {

    public delegate void FeedbackString( string message);

    class NoCallbackDefinedException : ApplicationFatalException {
        public NoCallbackDefinedException() : base( "No callback is defined") {
        }
        public static System.Exception Instantiate() {
            return new NoCallbackDefinedException();
        }
    }

    public class Callback {
        private static FeedbackString _feedback;

        public static FeedbackString CBFeedbackString {
            get {
                if( _feedback == null) {
                    throw new NoCallbackDefinedException();
                }
                return _feedback;
            }
            set {
                _feedback = value;
            }
        }
    }

    public class Console {
        public Console() {

        }
        public static void WriteLine( string message) {
            Callback.CBFeedbackString( message);
        }
    }
}
