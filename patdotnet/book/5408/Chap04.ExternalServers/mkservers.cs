using System;

namespace ExternalServers {
    internal class LocalBankAccount : Definitions.Account {

    }
    internal class LocalBankClient : Definitions.Client {

    }

    internal class LocalAccountRemotable : Definitions.Account, Definitions.IAccount {
        public override void Add( Definitions.Entry entry ) {
            base.Add( entry );
        }
        public override Decimal Balance {
            get {
                return base.Balance;
            }
        }
    }

    internal class LocalClientRemotable : Definitions.Client, Definitions.IClient {
        public override void Add( Definitions.IAccount account ) {
            base.Add( account );
        }
        public override void Remove( Definitions.IAccount account ) {
            base.Remove( account );
        }
    }
}

