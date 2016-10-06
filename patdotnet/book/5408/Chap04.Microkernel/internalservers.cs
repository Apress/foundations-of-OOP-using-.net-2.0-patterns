using System;
using NUnit.Framework;
using Definitions;

namespace InternalServers {
    internal static class PathsResolver {
        private static string LinuxPath {
            get {
                return "";
            }
        }
		
        public static string WindowsPath {
            get {
                return @"C:\Documents and Settings\cgross\Desktop\documents\active\oop-using-net-patterns\bin";
            }
        }
        public static string ExternalServer {
            get {
                return "Chap04.ExternalServers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            }
        }
        public static void AppendPaths( Implementations.Resolver resolver) {
            OperatingSystem os = System.Environment.OSVersion;
            if( (int)os.Platform == 128 ) {
                // This is Linux
                resolver.AppendPaths( LinuxPath );
            }
            else {
                resolver.AppendPaths( WindowsPath );
            }
        }
    }
    internal static class AssemblyIdentifierResolver {
        public static string Account {
            get {
                return "ExternalServers.LocalAccountRemotable";
            }
        }
        public static string Client {
            get {
                return "ExternalServers.LocalClientRemotable";
            }
        }
    }

    internal static class GeneralIdentifiers {
        public static string ApplicationName {
            get {
                return "Chap04-Micro-Kernel";
            }
        }
    }

    public class CostDoesNotExist : Exception {
        public CostDoesNotExist() : base( "Cost does not exist" ) {
        }
    }

    internal static class EntryCosts {
        public static Decimal GetCostItem( string type ) {
            if( String.Compare( type, "withdrawal" ) == 0 ) {
                return new Decimal( 1.0 );
            }
            else if( String.Compare( type, "deposit" ) == 0 ) {
                return new Decimal( 1.0 );
            }
            else {
                throw new CostDoesNotExist();
            }
        }
    }
}
