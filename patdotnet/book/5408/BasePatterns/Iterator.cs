using System;
using System.Collections;

public class CardboardBox
{
	public int _startX;
	public int _startY;
	public int _lengthX;
	public int _lengthY;
}

public class CardboardBoxes : IEnumerator, IEnumerable
{
	ArrayList _array = new ArrayList();

	public CardboardBoxes()
	{
	}

	public void Add( CardboardBox box)
	{
		_array.Add( box);
	}

	public System.Object Current
	{
		get
		{
			return null;
		}
	}
	public bool MoveNext()
	{
		return false;
	}
	public void Reset()
	{

	}
	public System.Collections.IEnumerator GetEnumerator()
	{
		return this;
	}
}

public class Warehouse : IEnumerable
{
	public System.Collections.IEnumerator GetEnumerator()
	{
		return new CardboardBoxes();
	}
}


public class TestIterator
{
	public void Execute()
	{
		CardboardBoxes boxes = new CardboardBoxes();
	
		boxes.Add( new CardboardBox());
		boxes.Add( new CardboardBox());
		boxes.Add( new CardboardBox());

		foreach( CardboardBox box in boxes)
		{
			Console.WriteLine( "hello world");
		}
	}
}