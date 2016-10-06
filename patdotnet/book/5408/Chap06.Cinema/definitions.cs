using System;
using System.Collections.Generic;
using Devspace.Commons.TDD;
using Devspace.Commons.Functors;
using Devspace.Commons.Collections;

namespace Cinema {
    public class Ticket {
        private double _price;
        private int _age;
        
        public Ticket( double price, int age) {
            _price = price;
            _age = age;
        }
        
        public virtual double Price {
            get {
                return _price;
            }
        }
        public virtual int Age {
            get {
                return _age;
            }
        }
        public override string ToString() {
            MemoryTracer.Start(this);
            MemoryTracer.Variable("price", _price);
            MemoryTracer.Variable("age", _age);
            return MemoryTracer.End();
        }
    }
    
    public delegate void RunningTotalBroadcast( double runningTotal);

    public class NullRunningTotalBroadcast {
        static void NothingRunningTotalBroadcast( double runningTotal) {
        }
        public static RunningTotalBroadcast GetInstance() {
            return new RunningTotalBroadcast( NothingRunningTotalBroadcast);
        }
    }
    
    public class Movie {
        public enum MOVIE_RATING {
            GENERAL,
            PARENTS_CAUTIONED,
            RESTRICTED
        }

        private string _identifier;
        private MOVIE_RATING _rating;
        
        public Movie(string identifier, MOVIE_RATING rating) {
            _rating = rating;
            _identifier = identifier;
        }

        public virtual MOVIE_RATING Rating {
            get {
                return _rating;
            }
        }
        public virtual IList<Ticket> CreatePredicate(IList<Ticket> parent) {
            return new PredicateProxy<Ticket>(parent,
                PredicateOrFunctor<Ticket>.CreateInstance(new DelegatePredicate<Ticket>[] {
                    delegate( Ticket ticket) {
                        if( _rating == MOVIE_RATING.RESTRICTED && ticket.Age >= 18) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    },
                    delegate( Ticket ticket) {
                        if( _rating == MOVIE_RATING.PARENTS_CAUTIONED && ticket.Age >= 13) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    },
                    delegate( Ticket ticket) {
                        if( _rating == MOVIE_RATING.GENERAL) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    },
                }
            ));
        }
        public override string ToString() {
            MemoryTracer.Start(this);
            MemoryTracer.Variable("identifier", _identifier);
            if(_rating == MOVIE_RATING.GENERAL) {
                MemoryTracer.Variable("rating", "GENERAL");
            }
            else if(_rating == MOVIE_RATING.PARENTS_CAUTIONED) {
                MemoryTracer.Variable("rating", "PARENTS_CAUTIONED");
            }
            else if(_rating == MOVIE_RATING.RESTRICTED) {
                MemoryTracer.Variable("rating", "RESTRICTED");
            }
            return MemoryTracer.End();
        }
    }

    public class NullMovie: Movie {
        public NullMovie(string identifier, MOVIE_RATING rating) : base( "", MOVIE_RATING.GENERAL) {
        }
        public override Movie.MOVIE_RATING Rating {
            get { return MOVIE_RATING.GENERAL; }
        }
        public override IList<Ticket> CreatePredicate(IList<Ticket> parent) {
            return parent;
        }
        public override string ToString() {
            MemoryTracer.Start(this);
            return MemoryTracer.End();
        }
    }
}


