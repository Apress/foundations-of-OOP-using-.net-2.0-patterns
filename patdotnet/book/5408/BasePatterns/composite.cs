using System;
using System.Collections;

public abstract class INode
{
	protected string _name;

	public INode( string name)
	{
		_name = name;
	}

	public virtual void Add( INode node)
	{
		Console.WriteLine( "Not implemented");
	}
	public abstract void DumpContents();
}

public class Directory : INode
{
	private ArrayList _entries = new ArrayList();

	public Directory( string name) : base( name) { }

	public override void Add( INode node)
	{
		_entries.Add( node);
	}
	public override void DumpContents()
	{
		Console.WriteLine( "Node : {0}", _name);
		foreach( INode node in _entries)
		{
			node.DumpContents();
		}
	}
}

public class File : INode
{
	public File( string name) : base( name) { }

	public override void DumpContents()
	{
		Console.WriteLine( "Name of file is " + _name);
	}
}


public class TestComposite
{
	public void Execute()
	{
		Directory direc = new Directory( "/");

		direc.Add( new File( "sample.txt"));
		direc.Add( new File( "another.txt"));
		Directory subDir = new Directory( "samples/");
		direc.Add( subDir);
		subDir.Add( new File( "text.xml"));

		direc.DumpContents();
	}
}
