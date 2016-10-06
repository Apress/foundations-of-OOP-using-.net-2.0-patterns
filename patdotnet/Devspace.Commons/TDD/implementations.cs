#region devspace.com Copyright (c) 2005, Christian Gross
// one line to give the library's name and an idea of what it does.
// Copyright (C) 2005  Christian Gross devspace.com (christianhgross@yahoo.ca)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

using System;

#if ! LOGCONSOLE
using log4net;

[assembly: log4net.Config.DOMConfigurator(Watch=true)]
#endif


namespace Devspace.Commons.TDD {

 #if LOGCONSOLE
     public class Logging {
         public static void Debug( string message) {
             Console.WriteLine( "Debug - " + message);
         }
         public static void Info( string message) {
             Console.WriteLine( "Info - " + message);
         }
         public static void Warn( string message) {
             Console.WriteLine( "Warn - " + message);
         }
         public static void Error( string message) {
             Console.WriteLine( "Error - " + message);
         }
         public static void Fatal( string message) {
             Console.WriteLine( "Fatal - " + message);
         }
     }

 #elif LOGLOG4NET
     public class Logging {
         private static readonly ILog log = LogManager.GetLogger( "logger.log4net");
         static void Debug( string message) {
             if( log.IsDebugEnabled) {
                 log.Debug( message);
             }
         }
         static void Info( string message) {
             if( log.IsInfoEnabled) {
                 log.Info( message);
             }
         }
         static void Warn( string message) {
             if( log.IsWarnEnabled) {
                 log.Warn( message);
             }
         }
         static void Error( string message) {
             if( log.IsErrorEnabled) {
                 log.Error( message);
             }
         }
         static void Fatal( string message) {
             if( log.IsFatalEnabled) {
                 log.Fatal( message);
             }
         }
     }

 #else
     public class Logging {
         public static void Debug( string message) {
         }
         public static void Info( string message) {
         }
         public static void Warn( string message) {
         }
         public static void Error( string message) {
         }
         public static void Fatal( string message) {
         }
     }
 #endif

}


