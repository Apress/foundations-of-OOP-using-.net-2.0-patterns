using System;
using System.Collections.Generic;
using Devspace.Commons.Functors;
using Devspace.Commons.Collections;
using Devspace.Commons.TDD;

namespace Cinema.Implementations {
    public class TicketsBuilder1 {
        private class StatisticsCounter {
            private double _runningTotal;
            public StatisticsCounter() {
                _runningTotal = 0.0;
            }
            public void ClosureAddMethod( Ticket ticket) {
                _runningTotal += ticket.Price;
            }
            public void ClosureRemoveMethod( Ticket ticket) {
                _runningTotal -= ticket.Price;
            }
        }
        public static IList<Ticket> CreateCollection() {
            StatisticsCounter cls = new StatisticsCounter();
            IList<Ticket> parent = new ClosureAddProxy< Ticket>( new List< Ticket>(),
                new DelegateClosure< Ticket>( cls.ClosureAddMethod));
            return new ClosureRemoveProxy<Ticket>( parent,
                new DelegateClosure< Ticket>( cls.ClosureRemoveMethod));
        }
    }

    
    public class TicketsBuilder2 {
        private class StatisticsCounter {
            private double _runningTotal;
            private RunningTotalBroadcast _delegateRunningTotal;

            public StatisticsCounter(RunningTotalBroadcast delegateRunningTotal) {
                _runningTotal = 0.0;
                _delegateRunningTotal = delegateRunningTotal;
            }
            public void ClosureAddMethod(Ticket ticket) {
                _runningTotal += ticket.Price;
                _delegateRunningTotal(_runningTotal);
            }
            public void ClosureRemoveMethod( Ticket ticket) {
                _runningTotal -= ticket.Price;
                _delegateRunningTotal( _runningTotal);
            }
        }
        public static IList<Ticket> CreateCollection(RunningTotalBroadcast runningTotal) {
            StatisticsCounter cls = new StatisticsCounter( runningTotal);
            IList<Ticket> parent = new ClosureAddProxy< Ticket>( new List< Ticket>(),
                new DelegateClosure< Ticket>( cls.ClosureAddMethod));
            return new ClosureRemoveProxy<Ticket>( parent,
                new DelegateClosure< Ticket>( cls.ClosureRemoveMethod));
        }
    }

    public class TicketsFacade2 {
        private double _runningTotal;
        private IList<Ticket> _tickets;

        public TicketsFacade2() {
            _tickets = CreateCollection();
        }
        public void Add(Ticket ticket) {
            _tickets.Add(ticket);
        }
        public double RunningTotal {
            get {
                return _runningTotal;
            }
        }
        private IList<Ticket> CreateCollection() {
            return TicketsBuilder2.CreateCollection(new RunningTotalBroadcast(this.RunningTotalMethod));
        }

        private void RunningTotalMethod(double value) {
            _runningTotal = value;
        }
        public override string ToString() {
            MemoryTracer.Start(this);
            MemoryTracer.Variable("runningtotal", _runningTotal);
            MemoryTracer.StartArray("tickets");
            foreach(Ticket ticket in _tickets) {
                MemoryTracer.Embedded(ticket.ToString());
            }
            MemoryTracer.EndArray();
            return MemoryTracer.End();
        }
    }

    public class TicketsFacade3 {
        private double _runningTotal;
        private IList<Ticket> _tickets;
        private Movie _movie;

        public TicketsFacade3( Movie movie) {
            _movie = movie;
            _tickets = CreateCollection();
            _tickets = _movie.CreatePredicate(_tickets);
        }
        public void Add(Ticket ticket) {
            _tickets.Add(ticket);
        }
        public double RunningTotal {
            get {
                return _runningTotal;
            }
        }
        private IList<Ticket> CreateCollection() {
            return TicketsBuilder2.CreateCollection(new RunningTotalBroadcast(this.RunningTotalMethod));
        }

        private void RunningTotalMethod(double value) {
            _runningTotal = value;
        }
    }
}


