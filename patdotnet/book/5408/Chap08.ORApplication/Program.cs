using System;
using System.Collections;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Examples.QuickStart;

namespace Chap08_ORApplication
{
    class Program
    {
        static void SaveDB() {
            Configuration cfg = new Configuration();
            cfg.SetProperty( "hibernate.dialect", "NHibernate.Dialect.FirebirdDialect");
            cfg.SetProperty( "hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            cfg.SetProperty( "hibernate.connection.driver_class", "NHibernate.Driver.FirebirdDriver");
            //cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=localhost;Port=3050");
            cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=192.168.1.104;Port=3050");
            // ;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;Packet Size=8192;ServerType=0
            cfg.AddAssembly("Chap08.ObjectRelational");
            //cfg.AddXmlFile( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ObjectRelational\user.hbm.xml");
            //cfg.Configure( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ORApplication\nhibernate.config");
            ISessionFactory factory = cfg.BuildSessionFactory();
            ISession session = factory.OpenSession();
            ITransaction transaction = session.BeginTransaction();
            User newUser = new User( "joe_coolss", "Joseph Cool", "abc123", "joe@cool.com");
            
            // Tell NHibernate that this object should be saved
            session.Save(newUser);
            
            // commit all of the changes to the DB and close the ISession
            transaction.Commit();
            session.Close();
        }
        static void ReadDB() {
            Configuration cfg = new Configuration();
            cfg.SetProperty( "hibernate.dialect", "NHibernate.Dialect.FirebirdDialect");
            cfg.SetProperty( "hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            cfg.SetProperty( "hibernate.connection.driver_class", "NHibernate.Driver.FirebirdDriver");
            //cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=localhost;Port=3050");
            cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=192.168.1.103;Port=3050");
            // ;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;Packet Size=8192;ServerType=0
            cfg.AddAssembly("Chap08.ObjectRelational");
            //cfg.AddXmlFile( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ObjectRelational\user.hbm.xml");
            //cfg.Configure( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ORApplication\nhibernate.config");
            ISessionFactory factory = cfg.BuildSessionFactory();
            ISession session = factory.OpenSession();
            ITransaction transaction = session.BeginTransaction();
            session = factory.OpenSession();
            
            User joeCool = (User)session.Load(typeof(User), "joe_coolss");
            if( joeCool != null) {
                Console.WriteLine( "yupe (" + joeCool.EmailAddress + ")");
            }
            else {
                Console.WriteLine( "ooops");
            }
            joeCool.EmailAddress = "joe@another.com";
            // Writes to the database if any changes are made
            //session.Update( new
            session.Flush();
            transaction.Commit();
            session.Close();
        }
        static void ReadTable() {
            Configuration cfg = new Configuration();
            cfg.SetProperty( "hibernate.dialect", "NHibernate.Dialect.FirebirdDialect");
            cfg.SetProperty( "hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            cfg.SetProperty( "hibernate.connection.driver_class", "NHibernate.Driver.FirebirdDriver");
            //cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=localhost;Port=3050");
            cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\NHIBERNATE.FDB;DataSource=192.168.1.104;Port=3050");
            // ;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;Packet Size=8192;ServerType=0
            cfg.AddAssembly("Chap08.ObjectRelational");
            //cfg.AddXmlFile( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ObjectRelational\user.hbm.xml");
            //cfg.Configure( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ORApplication\nhibernate.config");
            ISessionFactory factory = cfg.BuildSessionFactory();
            ISession session = factory.OpenSession();
            ITransaction transaction = session.BeginTransaction();

            IList userList = session.CreateCriteria(typeof(User)).List();
            foreach(User user in userList)
            {
                System.Diagnostics.Debug.WriteLine(user.Id + " email address " + user.EmailAddress);
            }
        }
        static void Main(string[] args) {
            //SaveDB();
            ReadDB();
            //ReadTable();
        }
    }
}
