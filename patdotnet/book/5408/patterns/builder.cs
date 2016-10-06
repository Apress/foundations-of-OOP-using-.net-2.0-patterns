using System;

interface IBuilderFactory
{
	void MakeCPU( int speed);
	void MakeHardDisk( int size);
	void MakeVideo( int type);
}

class ComputerBuilder
{
	public void BuildFastComputer( IBuilderFactory factory, int level)
	{
		factory.MakeCPU( 1000);
		if( level == 2)
		{
			factory.MakeHardDisk( 3000);
		}
		else
		{
			factory.MakeHardDisk( 1000);
		}
		factory.MakeVideo( 12);
	}
	public void BuildSlowComputer( IBuilderFactory factory)
	{
		factory.MakeCPU( 500);
		factory.MakeHardDisk( 300);
		factory.MakeVideo( 6);
	}
}

public class IntelBuilderFactory : IBuilderFactory
{
	public void MakeCPU( int speed)
	{
		Console.WriteLine( "Building a CPU " + speed);
	}
	public void MakeHardDisk( int size)
	{
		Console.WriteLine( "Building a Harddisk " + size);
	}
	public void MakeVideo( int type)
	{
		Console.WriteLine( "Building Video " + type);
	}
}


public class TestBuilder
{
	public void Execute()
	{
		ComputerBuilder builder = new ComputerBuilder();
		IntelBuilderFactory factory = new IntelBuilderFactory();

		builder.BuildFastComputer( factory, 100);

		builder.BuildSlowComputer( factory);
	}
}
