// Factory
// ******************************************************************************
public interface Shape
{
	void Output();
}

public interface IShapeFactory
{
	Shape createInstance();
}

public interface IShapeAbstractFactory
{
	Shape createBox();
	Shape createTriangle();
}

	// ******************************************************************************
public class Box : Shape
{
	public void Output()
	{
		System.Console.WriteLine( "You have a box");
	}
}

public class BoxFactory : IShapeFactory
{
	public Shape createInstance()
	{
		return new Box();
	}
}

// ******************************************************************************
public class Triangle : Shape
{
	public void Output()
	{
		System.Console.WriteLine( "You have a triangle");
	}
}

public class TriangleBox : IShapeFactory
{
	public Shape createInstance()
	{
		return new Triangle();
	}
}

// ******************************************************************************
public class AbstractFactoryImpl : IShapeAbstractFactory
{
	public Shape createBox()
	{
		return new Box();
	}
	public Shape createTriangle()
	{
		return new Triangle();
	}
}


public class TestFactory
{
	public void Execute()
	{
		AbstractFactoryImpl impl = new AbstractFactoryImpl();

		Shape shp = impl.createBox();

		shp.Output();
	}
}