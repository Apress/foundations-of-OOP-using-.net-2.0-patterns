using System;
using System.Collections;
using System.Text;
using NHibernate;
using NHibernate.Cfg;

using Chap08.ObjectRelational;

using NUnit.Framework;
 

[TestFixture]
public class TestBooks {
    Configuration _cfg;
    ISessionFactory _factory;
    ISession _session;
    
    private void Initialize() {
        Console.WriteLine( "Connect");
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
    private  void Destroy() {
        Console.WriteLine( "Disconnect");
        if( _session != null) {
            _session.Close();
        }
    }
    [Test]
    public void TestWriteBook() {
        Initialize();
        try {
            // We are going to catch this exception in case the object is already there
            ITransaction transaction = _session.BeginTransaction();
            Book book = new Book( "1-59059-540-8", "Foundations of Object Oriented Programming Using .NET 2.0 Patterns", "Christian Gross");
            _session.Save( book);
            transaction.Commit();
        }
        catch( Exception ex) {
            Console.WriteLine( "Could happen error (" + ex.Message + ")");
        }
        Destroy();
    }
    [Test]
    public void TestReadBook() {
        Initialize();
        Book myBook = (Book)_session.Load(typeof(Book), "1-59059-540-8");
        if( myBook != null) {
            Console.WriteLine( "Title (" + myBook.Title + ")");
        }
        else {
            Console.WriteLine( "could not be loaded");
        }
        BookUpdater.UpdateAuthor( myBook, "Different Author");
        _session.Flush();
        Destroy();
    }
    [Test]
    public void TestSelectAllComments() {
        Initialize();
        
        IList list;
        list = _session.CreateCriteria( typeof( Comment)).List();
        if( list.Count == 0) {
            Console.WriteLine( "huh????");
        }
        foreach( Comment comment in list) {
            Console.WriteLine( "Comment (" + comment.Text + ")(" + comment.WhoMadeComment + ")");
            Console.WriteLine( "-- Parent (" + comment.Parent.Title + ")");
        }
        Destroy();
    }
    [Test]
    public void TestSelectQuery() {
        Initialize();
        
        IList list;
        list = _session.CreateQuery( "from Chap08.ObjectRelational.Comment as comment where comment.WhoMadeComment = 'me'").List();
        if( list.Count == 0) {
            Console.WriteLine( "huh????");
        }
        foreach( Comment comment in list) {
            Console.WriteLine( "Comment (" + comment.Text + ")");
            Console.WriteLine( "-- Parent (" + comment.Parent.Title + ")");
        }
        Destroy();
    }
    /*[Test]
    public void TestDeleteBook() {
        Initialize();
        ITransaction transaction = _session.BeginTransaction();
        Book myBook = (Book)_session.Load(typeof(Book), "1-59059-540-8");
        if( myBook != null) {
            Console.WriteLine( "Title (" + myBook.Title + ")");
        }
        else {
            Console.WriteLine( "could not be loaded");
        }
        _session.Delete( myBook);
        transaction.Commit();
        Destroy();
    }*/
}

[TestFixture]
public class TestParentChildRelations {
    Configuration _cfg;
    ISessionFactory _factory;
    ISession _session;
    
    private void Initialize() {
        Console.WriteLine( "Connect");
        _cfg = new Configuration();
        _cfg.SetProperty( "hibernate.dialect", "NHibernate.Dialect.FirebirdDialect");
        _cfg.SetProperty( "hibernate.connection.provider", "NHibernate.Connection.DriverConnectionProvider");
        _cfg.SetProperty( "hibernate.connection.driver_class", "NHibernate.Driver.FirebirdDriver");
        _cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\BOOKS.FDB;DataSource=localhost;Port=3050");
        //_cfg.SetProperty( "hibernate.connection.connection_string", @"User=SYSDBA;Password=masterkey;Database=c:\db\BOOKS.FDB;DataSource=192.168.1.103;Port=3050");
        // ;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;Packet Size=8192;ServerType=0
        _cfg.AddAssembly("Chap08.ObjectRelational");
        //cfg.AddXmlFile( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ObjectRelational\user.hbm.xml");
        //cfg.Configure( @"C:\Documents and Settings\cgross\Desktop\projects\oop-using-net-patterns\src\Chap08.ORApplication\nhibernate.config");
        _factory = _cfg.BuildSessionFactory();
        _session = _factory.OpenSession();
    }
    private  void Destroy() {
        Console.WriteLine( "Disconnect");
        if( _session != null) {
            _session.Close();
        }
    }
    [Test]
    public void CreateComment() {
        Initialize();
        Book myBook = (Book)_session.Load(typeof(Book), "1-59059-540-8");
        if( myBook != null) {
            Console.WriteLine( "Title (" + myBook.Title + ")");
        }
        else {
            Console.WriteLine( "could not be loaded");
        }
        if( myBook.Comments.Count > 0) {
            Console.WriteLine( "We have some comments");
            foreach( Comment comment in myBook.Comments) {
                Console.WriteLine( "Comment (" + comment.Text + ")");
            }
        }
        else {
            Console.WriteLine( "no comments");
        }
        myBook.AddComment( new Comment( "another comment", "my author"));
        //Comment newcomment = new Comment( "another comment", "my author");
        //myBook.Comments.Add( newcomment);
        //_session.Save( newcomment);
        _session.Flush();
        Destroy();
    }
}


