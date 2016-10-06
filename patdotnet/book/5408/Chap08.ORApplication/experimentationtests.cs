using System;
using System.Collections;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

using Chap08.ObjectRelational;

using Experimentations;

using NUnit.Framework;


[TestFixture]
public class TestExperimentations {
    Configuration _cfg;
    ISessionFactory _factory;
    ISession _session;
    
    [TestFixtureSetUp]
    public void Setup() {
        _cfg = new Configuration();
        _cfg.SetProperty( "hibernate.dialect", "NHibernate.Dialect.FirebirdDialect");
        _cfg.SetProperty( "hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
        _cfg.SetProperty( "hibernate.connection.driver_class", "NHibernate.Driver.FirebirdDriver");
        //cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=localhost;Port=3050");
        _cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\BOOKS.FDB;DataSource=192.168.1.103;Port=3050");
        // ;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;Packet Size=8192;ServerType=0
        _cfg.AddAssembly("Chap08.ObjectRelational");
        //cfg.AddXmlFile( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ObjectRelational\user.hbm.xml");
        //cfg.Configure( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ORApplication\nhibernate.config");
        _factory = _cfg.BuildSessionFactory();
        _session = _factory.OpenSession();
    }
    [TestFixtureTearDown]
    public void TearDown() {
        if( _session != null) {
            _session.Close();
        }
    }
    [Test]
    public void TestWriteDescriptor() {
        try {
            // We are going to catch this exception in case the object is already there
            ITransaction transaction = _session.BeginTransaction();
            
            Descriptor descriptor = new Descriptor( 100, "hello world");
            _session.Save( descriptor);
            transaction.Commit();
        }
        catch( Exception ex) {
            Console.WriteLine( "Could happen error (" + ex.Message + ")");
        }
    }
    [Test]
    public void TestReadDescriptor() {
        Descriptor descriptor = (Descriptor)_session.Load(typeof(Descriptor), 100);
        if( descriptor != null) {
            Console.WriteLine( "Description (" + descriptor.Description + ")");
        }
        else {
            Console.WriteLine( "could not be loaded");
        }
        
    }
    
}

