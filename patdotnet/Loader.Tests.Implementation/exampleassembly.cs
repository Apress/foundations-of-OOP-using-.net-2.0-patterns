using System;

class ExampleClass : MarshalByRefObject, ExampleInterface {
    public void Message() {
        Console.WriteLine( "Hello from ExampleClass.Messagessssss");
    }
}

[Serializable]
public class AnotherClass {
    public void Method() {
        Console.WriteLine( "AnotherClass.Method");
    }
}

/*

public class Factory : Devspace.Commons.Loader.IFactory {
    public Object CreateInstance( string identifier) {
        if( String.Compare( identifier, "ExampleClass") == 0) {
            return (Object)(ExampleInterface)(new ExampleClass());
        }
        return null;
    }
}

*/