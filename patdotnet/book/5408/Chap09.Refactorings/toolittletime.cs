using System;
using System.IO;
using System.Text;
namespace Chap09.Refactorings {
    
    [Obsolete( "Don't use it")]
    public class Type1 {
        public void Method() {
            Console.WriteLine( "method");
        }
    }
    
    public class Using {
        public void Method() {
            Type1 cls = new Type1();
            cls.Method();
        }
    }
    public class StringReadStream : ReadStream {
        MemoryStream _memoryStream = new MemoryStream();

        public StringReadStream( string buffer) {
            _memoryStream = new MemoryStream( Encoding.Unicode.GetBytes( buffer));
        }
        public override int Read(byte[] buffer, int offset, int count) {
            return _memoryStream.Read( buffer, offset, count);
        }
        
        /// <summary>
        /// Method ReadByte
        /// </summary>
        /// <returns>An int</returns>
        public override int ReadByte() {
            // TODO
            return 0;
        }
        
        public override long Position {
            get {
                // TODO
                return 0;
            }
            set {
                // TODO
            }
        }
        
        /// <summary>
        /// Method Seek
        /// </summary>
        /// <returns>A long</returns>
        /// <param name="offset">A  long</param>
        /// <param name="origin">A  System.IO.SeekOrigin</param>
        public override long Seek(long offset, SeekOrigin origin) {
            // TODO
            return 0;
        }
        
        public override long Length {
            get {
                // TODO
                return 0;
            }
        }
        
    }
}
