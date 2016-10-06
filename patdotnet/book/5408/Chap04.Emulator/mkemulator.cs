using System;
using Devspace.Commons.TDD;

namespace Emulator {
    public class Bank {
        Definitions.IAccount _account;

        public static Definitions.IClient CreateClient() {
            Implementations.Resolver resolver = new Implementations.Resolver();

            return resolver.CreateClient();
        }
        public static Definitions.IAccount CreateAccount( Definitions.IClient client) {
            Implementations.Resolver resolver = new Implementations.Resolver();

            Definitions.IAccount account = resolver.CreateAccount();
            client.Add( account );
            return account;
        }
        public static Bank CreateBank() {
            return new Bank();
        }
        private Bank() {
        }
        public Definitions.IAccount ClientAccount {
            get {
                return _account;
            }
            set {
                _account = value;
            }
        }
        public void MakeDeposit( Decimal value) {
            _account.Add( new Definitions.Entry( value,
                Implementations.ChargeCosts.GetDepositCharge(), "Default deposit", true));
        }
        public void MakeWithdrawal( Decimal value) {
            _account.Add( new Definitions.Entry( value,
                Implementations.ChargeCosts.GetWithdrawalCharge(), "Default withdrawal", false));
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            if( _account != null ) {
                MemoryTracer.Embedded( _account.ToString() );
            }
            return MemoryTracer.End();
        }
    }
}

