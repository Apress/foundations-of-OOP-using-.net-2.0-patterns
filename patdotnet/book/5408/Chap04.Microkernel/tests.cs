using System;
using NUnit.Framework;

class TestAbstractAccount : Definitions.Account {
}
class TestAbstractClient : Definitions.Client {
}

class TestAccount : Definitions.IAccount {
    public void Add( Definitions.Entry entry ) {
    }
    public void Remove( Definitions.Entry entry ) {
    }
    public Decimal Balance {
        get {
            return new Decimal( 0);
        }
    }
    public override string ToString() {
        return "TestAccount nothing to return";
    }
}

class TestClient : Definitions.IClient {
    public void Add( Definitions.IAccount account ) {
    }
    public void Remove( Definitions.IAccount account ) {
    }
    public override string ToString() {
        return "TestClient nothing to return";
    }
}

[TestFixture]
public class TestMicrokernel {
    [Test]
    public void TestCreateAccount() {
        TestAbstractAccount account = new TestAbstractAccount();

        account.Add( new Definitions.Entry( 
            new Decimal( 10), new Decimal( 1), "nothing", true));
        account.Add(new Definitions.Entry(
            new Decimal(20), new Decimal(1), "nothing", true));
        account.Add(new Definitions.Entry(
            new Decimal(20), new Decimal(1), "nothing", false));
        int counter = 0;
        foreach( Definitions.Entry entry in account ) {
            counter++;
        }
        Assert.AreEqual( 3, counter );
        Console.WriteLine( "*** Account");
        Console.WriteLine( account.ToString());
        Assert.AreEqual(7, account.Balance);
    }
    [Test]
    public void TestCreateClient() {
        TestAbstractClient client = new TestAbstractClient();

        client.Add( new TestAccount() );
        client.Add( new TestAccount() );
        int counter = 0;
        foreach( Definitions.Account account in client ) {
            counter++;
        }
        Assert.AreEqual( 2, counter);
        Console.WriteLine( "*** Client" );
        Console.WriteLine( client.ToString() );
    }
    private string LinuxPath {
        get {
            return "";
        }
    }
    private string WindowsPath {
        get {
            return @"C:\Documents and Settings\cgross\Desktop\documents\active\oop-using-net-patterns\bin";
        }
    }
    private string ExternalServer {
        get {
            return "Chap04.ExternalServers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
        }
    }
    private void AppendPaths( Implementations.Resolver resolver ) {
        OperatingSystem os = System.Environment.OSVersion;
        if( (int)os.Platform == 128 ) {
            // This is Linux
            //resolver.AppendPath( LinuxPath);
        }
        else {
            //resolver.AppendPath( WindowsPath );
        }
    }
    [Test]
    public void TestPlugins2() {
        Implementations.Resolver resolver = new Implementations.Resolver();
        AppendPaths( resolver );

        //resolver.Load();
        Console.WriteLine( "*** Resolver Before" );
        Console.WriteLine( resolver.ToString() );
        //Definitions.IAccount account =
        //    (Definitions.IAccount)resolver.CreateInstance( ExternalServer, "ExternalServers.LocalBankRemotable" );
        //Console.WriteLine( "*** Resolver After" );
        //Console.WriteLine( resolver.ToString() );
    }
    [Test]
    public void TestPlugins() {
        Implementations.Resolver resolver = new Implementations.Resolver();
        AppendPaths( resolver );

        //resolver.Load();
        Console.WriteLine( "*** Resolver Before" );
        Console.WriteLine( resolver.ToString() );
        //Definitions.Account account =
        //    (Definitions.Account)resolver.CreateInstance( ExternalServer, "ExternalServers.LocalBankAccount" );
        //Console.WriteLine( "*** Resolver After" );
        //Console.WriteLine( resolver.ToString() );
    }
}

