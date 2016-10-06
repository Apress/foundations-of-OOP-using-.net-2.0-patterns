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
using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Runtime.Remoting;

using Devspace.Commons.TDD;

namespace Devspace{
namespace Commons {
namespace Loader {

    #region AppDomain permission helper methods
    // .NET 2.0 feature, remove the static prefix on the class
    internal static class Permissions {
        public static void SetAppDomainPolicy( AppDomain appDomain ) {
            // Create an AppDomain policy level.
            PolicyLevel pLevel = PolicyLevel.CreateAppDomainLevel();
            // The root code group of the policy level combines all
            // permissions of its children.
            UnionCodeGroup rootCodeGroup;
            PermissionSet ps = new PermissionSet( PermissionState.None );
            ps.AddPermission( new SecurityPermission( SecurityPermissionFlag.Execution ) );
            rootCodeGroup = new UnionCodeGroup( new AllMembershipCondition(),
                                               new PolicyStatement( ps, PolicyStatementAttribute.Nothing ) );

            NamedPermissionSet localIntranet = FindNamedPermissionSet( "LocalIntranet" );
            // The following code limits all code on this machine to local intranet permissions
            // when running in this application domain.
            UnionCodeGroup virtualIntranet = new UnionCodeGroup(
                                                               new ZoneMembershipCondition( SecurityZone.MyComputer ),
                                                               new PolicyStatement( localIntranet,
                                                                                   PolicyStatementAttribute.Nothing ) );
            virtualIntranet.Name = "Virtual Intranet";
            // Add the code groups to the policy level.
            rootCodeGroup.AddChild( virtualIntranet );
            pLevel.RootCodeGroup = rootCodeGroup;
            appDomain.SetAppDomainPolicy( pLevel );
        }

        public static NamedPermissionSet FindNamedPermissionSet( string name ) {
            IEnumerator policyEnumerator = SecurityManager.PolicyHierarchy();

            while( policyEnumerator.MoveNext() ) {
                PolicyLevel currentLevel = (PolicyLevel)policyEnumerator.Current;

                if( currentLevel.Label == "Machine" ) {
                    IList namedPermissions = currentLevel.NamedPermissionSets;
                    IEnumerator namedPermission = namedPermissions.GetEnumerator();

                    while( namedPermission.MoveNext() ) {
                        if( ((NamedPermissionSet)namedPermission.Current).Name == name ) {
                            return ((NamedPermissionSet)namedPermission.Current);
                        }
                    }
                }
            }
            return null;
        }
    }
    #endregion

    #region Base class for Appdomains plugin types
    public abstract class AppDomainsBase {
        protected AppDomain _appDomain;
        protected string _applicationName;

        protected abstract void AssignPrivateBinPath( AppDomainSetup setup );
        protected abstract void AssignShawdowPath( AppDomainSetup setup, bool shawdowCopyAll );

        protected void AssignApplicationName( string applicationName ) {
            _applicationName = applicationName;
        }

        protected virtual void pLoad( bool shadowCopyAll ) {
            AppDomainSetup setup = new AppDomainSetup();

            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            AssignPrivateBinPath( setup );
            AssignShawdowPath( setup, shadowCopyAll );
            setup.ApplicationName = _applicationName;

            _appDomain = AppDomain.CreateDomain( _applicationName, null, setup );

            //Permissions.SetAppDomainPolicy( _appDomain );
        }
        protected virtual void pUnload() {
            AppDomain.Unload( _appDomain );
            _appDomain = null;
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Variable( "application name", _applicationName );

            MemoryTracer.Variable( "Application directory", AppDomain.CurrentDomain.SetupInformation.ApplicationBase );
            MemoryTracer.Variable( "Application bin directory", AppDomain.CurrentDomain.SetupInformation.PrivateBinPath );

            MemoryTracer.StartArray( "Loaded Assemblies" );
            foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() ) {
                MemoryTracer.Variable( "Assembly", assembly.FullName );
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }
    #endregion

    public class ResolverDynamicAssemblyDirectory : AppDomainsBase, IResolver<Identifier> {
        private bool _shadowCopyAll;
        private string _pluginDirectory;
        private string _applicationDirectory;
        private RemoteLoaderPlugins _remoteLoader;

        public ResolverDynamicAssemblyDirectory( string applicationName, string applicationDirectory, bool shadowCopyAll, string pluginDirectory ) {
            AssignApplicationName( applicationName );
            _applicationDirectory = applicationDirectory;
            _shadowCopyAll = shadowCopyAll;
            _pluginDirectory = pluginDirectory;
        }
        protected override void AssignPrivateBinPath( AppDomainSetup setup ) {
            setup.PrivateBinPath = _pluginDirectory;
        }
        protected override void AssignShawdowPath( AppDomainSetup setup, bool shawdowCopyAll ) {
            if( shawdowCopyAll ) {
                setup.ShadowCopyFiles = "true";
            }
        }
        public void Load() {
            pLoad( _shadowCopyAll );
            _remoteLoader = (RemoteLoaderPlugins)_appDomain.CreateInstanceAndUnwrap(
                "devspace.commons.loader", "Devspace.Commons.Loader.RemoteLoaderPlugins" );
            _remoteLoader.LoadTypes( _pluginDirectory );
        }
        public void Unload() {
            pUnload();
        }
        public bool CanCreate( Identifier identifier ) {
            if( identifier.DoesExist( Identifier.ID_type ) ) {
                return _remoteLoader.DoesTypeExist(
                    identifier[ Identifier.ID_type ] );
            }
            return false;
        }
        public ObjectType CreateInstance<ObjectType>( Identifier identifier ) {
            if( identifier.DoesExist( Identifier.ID_type ) ) {
                string typeIdentifier = identifier[ Identifier.ID_type];
                string assemblyIdentifier = _remoteLoader.FindAssemblyForType( typeIdentifier );

                return (ObjectType)_appDomain.CreateInstanceAndUnwrap( assemblyIdentifier, typeIdentifier );
            }
            return default(ObjectType);
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Variable( "Plugin directory", _pluginDirectory );
            MemoryTracer.Variable( "Application directory", _applicationDirectory );
            MemoryTracer.Variable( "Base Directory", AppDomain.CurrentDomain.BaseDirectory );
            MemoryTracer.Variable( "Shadow copy", _shadowCopyAll );
            MemoryTracer.Embedded( base.ToString() );
            MemoryTracer.Embedded( _remoteLoader.ToString() );
            return MemoryTracer.End();
        }
    }

    #region Remote Appdomain plugin assembly loader
    [Serializable]
    public class TypeDefinitions {
        public readonly string name;
        public readonly string assembly;
        public TypeDefinitions( string inpName, string inpAssembly) {
            name = inpName;
            assembly = inpAssembly;
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Variable( "name", name );
            MemoryTracer.Variable( "assembly", assembly );
            return MemoryTracer.End();
            
        }
    }

    public class RemoteLoaderPlugins : MarshalByRefObject {
        private ArrayList _types = new ArrayList();

        public RemoteLoaderPlugins() {
        }

        public void LoadTypes( string plugindirectory ) {
            string[] dlls = Directory.GetFileSystemEntries( plugindirectory, "*.dll" );
            foreach( string path in dlls ) {
                Assembly assembly;
                assembly = Assembly.LoadFrom( path );
                foreach( Type type in assembly.GetTypes() ) {
                    _types.Add( new TypeDefinitions(
                        type.Name, assembly.FullName ) );
                }
            }
        }
        public string FindAssemblyForType( string inpType ) {
            foreach( TypeDefinitions type in _types ) {
                if( String.Compare( type.name, inpType ) == 0 ) {
                    return type.assembly;
                }
            }
            return "";
        }
        public bool DoesTypeExist( string inpType) {
            foreach( TypeDefinitions type in _types ) {
                if( String.Compare( type.name, inpType ) == 0 ) {
                    return true;
                }
            }
            return false;
        }

        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Variable( "Base Directory", AppDomain.CurrentDomain.BaseDirectory );
            MemoryTracer.StartArray( "Loaded Assemblies" );
            foreach( Assembly assembly in AppDomain.CurrentDomain.GetAssemblies() ) {
                MemoryTracer.Variable( "Assembly", assembly.FullName );
            }
            MemoryTracer.EndArray();
            MemoryTracer.StartArray( "Loaded Types" );
            foreach( TypeDefinitions type in _types) {
                MemoryTracer.Embedded( type.ToString() );
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }
    #endregion

    public class ResolverStaticAssemblies : AppDomainsBase, IResolver< Identifier> {
        private RemoteLoaderPlugins _remoteLoader;

        public ResolverStaticAssemblies( string applicationName ) {
            AssignApplicationName( applicationName );
        }

        private bool _shadowCopy;
        public bool ShadowCopy {
            get {
                return _shadowCopy;
            }
            set {
                _shadowCopy = value;
            }
        }

        private ArrayList _paths = new ArrayList();
        private ArrayList _shadowPaths = new ArrayList();
        private string _cacheDirectory;

        public string[] Paths {
            get {
                string[] retval = new string[ _paths.Count ];
                for( int c1 = 0; c1 < _paths.Count; c1++ ) {
                    retval[ c1 ] = (string)(_paths[ c1 ]);
                }
                return retval;
            }
        }

        public string[] ShadowPaths {
            get {
                string[] retval = new string[ _shadowPaths.Count ];
                for( int c1 = 0; c1 < _shadowPaths.Count; c1++ ) {
                    retval[ c1 ] = (string)(_shadowPaths[ c1 ]);
                }
                return retval;
            }
        }
        public string CacheDirectory {
            get {
                return _cacheDirectory;
            }
            set {
                _cacheDirectory = value;
            }
        }
        public void AppendPath( string path ) {
            _paths.Add( path );
        }
        public void AppendPath( string path, bool isShadow ) {
            _paths.Add( path );
            if( isShadow ) {
                AppendShadowPath( path );
            }
        }
        public void AppendShadowPath( string path ) {
            _shadowPaths.Add( path );
        }
        public void RemovePath( string inpPath ) {
            _paths.Remove( inpPath );
        }
        protected override void AssignPrivateBinPath( AppDomainSetup setup ) {
            OperatingSystem os = System.Environment.OSVersion;
            string fullpath = "";
            if( (int)os.Platform == 128 ) {
                // This is Linux
                foreach( string path in _paths ) {
                    fullpath += path + ":";
                }
            }
            else {
                // This is everything else
                foreach( string path in _paths ) {
                    fullpath += path + ";";
                }
            }
            setup.PrivateBinPath = fullpath;
        }
        protected override void AssignShawdowPath( AppDomainSetup setup, bool shawdowCopyAll ) {
            if( shawdowCopyAll ) {
                setup.ShadowCopyFiles = "true";
            }
            else {
                if( _shadowPaths.Count == 0 ) {
                    return;
                }
                OperatingSystem os = System.Environment.OSVersion;
                string fullpath = "";
                if( (int)os.Platform == 128 ) {
                    // This is Linux
                    foreach( string path in _paths ) {
                        fullpath += path + ":";
                    }
                }
                else {
                    // This is everything else
                    foreach( string path in _paths ) {
                        fullpath += path + ";";
                    }
                }
                setup.ShadowCopyDirectories = fullpath;
            }
            if( _cacheDirectory.Length > 0 ) {
                setup.CachePath = _cacheDirectory;
            }
        }
        
        public void Load() {
            pLoad( _shadowCopy );
            _remoteLoader = (RemoteLoaderPlugins)_appDomain.CreateInstanceAndUnwrap(
                "devspace.commons.loader", "Devspace.Commons.Loader.RemoteLoaderPlugins" );

        }
        public void Unload() {
            pUnload();
        }
        public bool CanCreate( Identifier identifier ) {
            if( identifier.DoesExist( Identifier.ID_type ) &&
                identifier.DoesExist( Identifier.ID_assembly)) {
                return true;
            }
            return false;
        }
        public ObjectType CreateInstance<ObjectType>( Identifier identifier ) {
            if( identifier.DoesExist( Identifier.ID_type ) &&
                identifier.DoesExist( Identifier.ID_assembly)) {
                string typeIdentifier = identifier[ Identifier.ID_type ];
                string assemblyIdentifier = identifier[ Identifier.ID_assembly ];

                return (ObjectType)_appDomain.CreateInstanceAndUnwrap( assemblyIdentifier, typeIdentifier );
            }
            return default( ObjectType);
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Embedded( base.ToString() );
            MemoryTracer.Embedded( _remoteLoader.ToString() );
            return MemoryTracer.End();
        }
    }

    public class DispatcherImpl : Dispatcher<Identifier> {
        public DispatcherImpl( string applicationName) {
            _resolvers.Add( new ResolverStaticAssemblies( applicationName ) );
        }
        public override void Initialize() {
        }
        public override void Destroy() {
        }
    }
}
}
}

