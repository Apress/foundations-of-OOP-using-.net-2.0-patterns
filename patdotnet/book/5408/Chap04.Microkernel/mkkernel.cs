using System;
using System.Collections.Generic;
using Devspace.Commons.TDD;

namespace Definitions {

    [Serializable]
    public struct Entry {
        public readonly Decimal value;
        public readonly Decimal charges;
        public readonly string purpose;
        public readonly bool positive;

        public Entry( Decimal inpValue, Decimal inpCharges, string inpPurpose, bool inpPositive) {
            value = inpValue;
            charges = inpCharges;
            purpose = inpPurpose;
            positive = inpPositive;
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Variable( "Value", value.ToString());
            MemoryTracer.Variable( "Charges", charges.ToString() );
            MemoryTracer.Variable( "Purpose", purpose );
            MemoryTracer.Variable( "Positive", positive );
            return MemoryTracer.End();
        }
    }

    public interface IAccount {
        void Add( Entry entry );
        Decimal Balance {
            get;
        }
    }

    public interface IClient {
        void Add( IAccount account );
        void Remove( IAccount account );
    }

    public abstract class Account : MarshalByRefObject {
        protected List< Entry> _entries = new List< Entry>();
        
        public virtual Decimal Balance {
            get {
                Decimal balance = new Decimal( 0);
                foreach( Entry entry in _entries) {
                    if( entry.positive ) {
                        balance = Decimal.Add( balance, entry.value);
                    }
                    else {
                        balance = Decimal.Subtract( balance, entry.value);
                    }
                    balance = Decimal.Subtract( balance, entry.charges);
                }
                return balance;
            }
        }
        public virtual void Add( Entry entry) {
            _entries.Add( entry);
        }

        public System.Collections.IEnumerator GetEnumerator() {
            foreach( Entry entry in _entries) {
                yield return entry;
            }
        }
        public override string ToString() {
            MemoryTracer.Start( this);
            MemoryTracer.Variable( "Entry Count", _entries.Count );
            MemoryTracer.StartArray( "Entry Collection" );
            foreach( Entry entry in _entries ) {
                MemoryTracer.Embedded( entry.ToString());
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }

    public abstract class Client : MarshalByRefObject {
        protected List< IAccount> _accounts = new List< IAccount>();

        public virtual void Add( IAccount account) {
            _accounts.Add( account);
        }
        public virtual void Remove( IAccount account) {
            _accounts.Remove( account);
        }

        public System.Collections.IEnumerator GetEnumerator() {
            foreach( IAccount account in _accounts) {
                yield return account;
            }
        }
        public override string ToString() {
            MemoryTracer.Start( this );
            MemoryTracer.Variable( "Account count", _accounts.Count);
            MemoryTracer.StartArray( "Account Collection" );
            foreach( Object account in _accounts ) {
                MemoryTracer.Embedded( account.ToString() );
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }
}


namespace Implementations {

    public static class ChargeCosts {
        public static Decimal GetDepositCharge() {
            return InternalServers.EntryCosts.GetCostItem( "deposit" );
        }
        public static Decimal GetWithdrawalCharge() {
            return InternalServers.EntryCosts.GetCostItem( "withdrawal" );
        }
    }

    public class Resolver : 
        Devspace.Commons.Loader.Dispatcher< Devspace.Commons.Loader.Identifier>  {
        Devspace.Commons.Loader.ResolverStaticAssemblies _resolverImpl; 
        
        public override void Initialize() {
            _resolverImpl = new Devspace.Commons.Loader.ResolverStaticAssemblies(
                InternalServers.GeneralIdentifiers.ApplicationName );
            InternalServers.PathsResolver.AppendPaths( this);
        }
        public override void Destroy() {
        }
        internal void AppendPaths( string paths ) {
            _resolverImpl.AppendPath( paths );
            _resolverImpl = null;
        }
        public Resolver() {
        }
        public Definitions.IClient CreateClient() {
            return CreateInstance< Definitions.IClient>(
                new Devspace.Commons.Loader.Identifier( 
                    InternalServers.PathsResolver.ExternalServer, 
                    InternalServers.AssemblyIdentifierResolver.Client));
        }
        public Definitions.IAccount CreateAccount() {
            return CreateInstance<Definitions.IAccount>(
                new Devspace.Commons.Loader.Identifier(
                    InternalServers.PathsResolver.ExternalServer,
                    InternalServers.AssemblyIdentifierResolver.Account ) );
        }
    }
}

