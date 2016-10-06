using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;

[TestFixture]
public class TestMovie {
    [Test]
    public void TestVersion1() {
        Console.WriteLine( "--- TestVersion1 ---");
        IList<Cinema.Ticket> list = Cinema.Implementations.TicketsBuilder1.CreateCollection();

        list.Add(new Cinema.Ticket(10.0, 12));
        list.Add(new Cinema.Ticket(10.0, 12));
    }
    private void RunningTotalMethod(double runningTotal) {
        Console.WriteLine("Running Total " + runningTotal);
    }
    [Test]
    public void TestVersion2() {
        Console.WriteLine( "--- TestVersion2 ---");
        IList<Cinema.Ticket> list = Cinema.Implementations.TicketsBuilder2.CreateCollection(
            new Cinema.RunningTotalBroadcast( this.RunningTotalMethod));
        list.Add(new Cinema.Ticket(10.0, 12));
        list.Add(new Cinema.Ticket(10.0, 12));
        list.RemoveAt( 1);
    }
    [Test]
    public void TestVersion3() {
        Console.WriteLine( "--- TestVersion3 ---");
        IList<Cinema.Ticket> list = Cinema.Implementations.TicketsBuilder2.CreateCollection(
            Cinema.NullRunningTotalBroadcast.GetInstance());
        list.Add(new Cinema.Ticket(10.0, 12));
        list.Add(new Cinema.Ticket(10.0, 12));
        list.RemoveAt( 1);
        
    }
}


