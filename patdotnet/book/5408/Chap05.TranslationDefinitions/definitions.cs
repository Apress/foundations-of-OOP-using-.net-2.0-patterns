using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace Chap05.TranslationDefinitions {

    public interface ITranslationServices {
        string Translate( string word );
    }

    internal class PluginBase {
        private string _path;

        public PluginBase( string path ) {
            _path = path;
        }

        public instanceType CreateInstance<instanceType>( string assemblyidentifier, string typeidentifier ) where instanceType : class {
            Assembly assembly;
            OperatingSystem os = System.Environment.OSVersion;
            if( (int)os.Platform == 128 ) {
                // This is Linux
                assembly = Assembly.LoadFrom( _path );
            }
            else {
                // This is everything else
                assembly = Assembly.Load( AssemblyName.GetAssemblyName( _path ) );
            }
            if( assembly != null ) {
                return (assembly.CreateInstance( typeidentifier )) as instanceType;
            }
            else {
                return null;
            }
        }
    }

    public class Loader :
        Devspace.Commons.Loader.Dispatcher<Devspace.Commons.Loader.Identifier> {
        Devspace.Commons.Loader.ResolverStaticAssemblies _resolverImpl;
        private string _path = @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\bin";
        private string _assembly = "Chap05.TranslationImplementations, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        private string _type = "Chap05.TranslationImplementations.TranslateToGerman";

        public override void Initialize() {
            _resolverImpl = new Devspace.Commons.Loader.ResolverStaticAssemblies(
                "TranslationServices" );
            _resolverImpl.AppendPath( _path );
            _resolvers.Add( _resolverImpl);
        }
        public override void Destroy() {
        }
        public Loader() {
        }
        public ITranslationServices CreateGermanTranslationDynamic() {
            return CreateInstance<ITranslationServices>(
                new Devspace.Commons.Loader.Identifier( _assembly, _type));
        }
        public ITranslationServices CreateGermanTranslationStatic() {
            PluginBase plugin = new PluginBase( "path" );
            return plugin.CreateInstance<ITranslationServices>( "assembly path", "client identifier" );
        }
    }
}
