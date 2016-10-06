using System;
using System.Text;
using NUnit.Framework;
using System.Collections;

public class Something : IComparer {
    
    /// <summary>Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.</summary>
    /// <returns>Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y. </returns>
    /// <param name="y">Second object to compare. </param>
    /// <param name="x">First object to compare. </param>
    /// <exception cref="T:System.ArgumentException">Neither x nor y implements the <see cref="T:System.IComparable"></see> interface.-or- x and y are of different types and neither one can handle comparisons with the other. </exception>
    /// <filterpriority>2</filterpriority>
    public int Compare(Object x, Object y) {
        // TODO
        return 0;
    }
    
    
}



[TestFixture]
public class Tests {
    [Test]
    public void TestSimple() {
        Console.WriteLine( "--- TestSimple ---");
        ExampleIterator iter = new ExampleIterator();
        foreach( int number in iter) {
            Console.WriteLine( "Number (" + number + ")");
        }
    }
    [Test]
    public void TestPredicate() {
        Console.WriteLine( "--- TestPredicate ---");
        IntegerData data = new IntegerData(
            delegate( int value) {
                if( value > 10) {
                    return true;
                }
                else {
                    return false;
                }
            });
        
        data.Add( 1);
        data.Add( 5);
        data.Add( 15);
        data.Add( 20);
        
        foreach( int number in data) {
            Console.WriteLine( "Number " + number);
        }
        
    }
}


