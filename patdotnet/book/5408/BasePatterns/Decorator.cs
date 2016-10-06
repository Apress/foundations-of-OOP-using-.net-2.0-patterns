using System;

public interface Ingredient {
    String GetIdentifier();
}


public abstract class Decorator : Ingredient {
    protected Ingredient _ingredient;

    public Decorator( Ingredient ingredient) {
        _ingredient = ingredient;
    }
    public virtual String GetIdentifier() {
        return _ingredient.GetIdentifier();
    }
}

public class Bun : Ingredient {
    private String _description = "bun";

    public Bun() {
    }
    public String GetIdentifier() {
        return _description;
    }
}

public class Lettuce : Decorator {
    private String _description = "lettuce";

    public Lettuce( Ingredient component) : base (component) {
    }
    public override String GetIdentifier() {
        return _ingredient.GetIdentifier() + " " + _description;
    }
}

public class Cheese : Decorator {
    private String _description = "cheese";

    public Cheese( Ingredient component) : base( component) {
    }
    public override String GetIdentifier() {
        return _ingredient.GetIdentifier() + " " + _description;
	}
}

public class Meat : Decorator {
    private String _description = "meat";

    public Meat( Ingredient component) : base( component) {
    }

    public override String GetIdentifier() {
        return _ingredient.GetIdentifier() + " " + _description;
    }
}

public class TestDecorater {
    public void Execute() {
        Ingredient hamburger = new Meat( new Lettuce( new Bun()));

        System.Console.WriteLine( "Meal is " + hamburger.GetIdentifier());

        Ingredient cheeseBurger = new Meat( new Cheese( new Lettuce( new Bun())));

        System.Console.WriteLine( "Meal is " + cheeseBurger.GetIdentifier());
	}
}










