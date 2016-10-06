using System;
using NUnit.Framework;


[TestFixture]
public class TestEmulator {
    [Test]
    public void TestAccount() {
        Emulator.Bank bank = Emulator.Bank.CreateBank();
        Definitions.IClient client = Emulator.Bank.CreateClient();
        Definitions.IAccount account = Emulator.Bank.CreateAccount( client);

        bank.ClientAccount = account;
        bank.MakeDeposit( new Decimal( 100 ) );
        bank.MakeDeposit( new Decimal( 100 ) );
        bank.MakeWithdrawal( new Decimal( 100 ) );
        Console.WriteLine( "*** bank" );
        Console.WriteLine( bank.ToString() );
        Assert.AreEqual( 97, account.Balance );
    }
}

