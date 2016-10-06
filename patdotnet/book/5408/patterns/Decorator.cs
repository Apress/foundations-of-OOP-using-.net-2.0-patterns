using System;

public interface Ingredient
{
	String getIdentifier();
}


public abstract class Decorator : Ingredient
{
	protected Ingredient _ingredient;

	public Decorator( Ingredient ingredient)
	{
		_ingredient = ingredient;
	}

	public virtual String getIdentifier()
	{
		return _ingredient.getIdentifier();
	}
}

public class Bun : Ingredient
{
	private String _description = "bun";

	public Bun() 
	{
	}

	public String getIdentifier()
	{
		return _description;
	}
}

public class Lettuce : Decorator
{
	private String _description = "lettuce";

	public Lettuce( Ingredient component) : base (component)
	{
	}

	public override String getIdentifier()
	{
		return _ingredient.getIdentifier() + " " + _description;
	}
}

public class Cheese : Decorator
{
	private String _description = "cheese";

	public Cheese( Ingredient component) : base( component)
	{
	}

	public override String getIdentifier()
	{
		return _ingredient.getIdentifier() + " " + _description;
	}
}

public class Meat : Decorator
{
	private String _description = "meat";

	public Meat( Ingredient component) : base( component)
	{
	}

	public override String getIdentifier()
	{
		return _ingredient.getIdentifier() + " " + _description;
	}
}

public class TestDecorater
{
	public void Execute()
	{
		Ingredient hamburger = new Meat( new Lettuce( new Bun()));

		System.Console.WriteLine( "Meal is " + hamburger.getIdentifier());

		Ingredient cheeseBurger = new Meat( new Cheese( new Lettuce( new Bun())));

		System.Console.WriteLine( "Meal is " + cheeseBurger.getIdentifier());
	}

}










