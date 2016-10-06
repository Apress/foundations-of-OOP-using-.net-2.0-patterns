using System;
using System.Collections;
using NUnit.Framework;

internal class PropertyKeeper {
    public string Value;
}

internal class NUnitAdapter: Abstractions.IControlAdapter {
    public type GetValue<ControlType, type>(ControlType control) where type: class {
        PropertyKeeper cls = control as PropertyKeeper;
        return cls.Value as type;
    }

    public void SetValue<ControlType, type>(ControlType control, type value) {
        PropertyKeeper cls = control as PropertyKeeper;
        cls.Value = value as string;
   }
}


[TestFixture]
public class TestBusinessLogic {
    [Test]
    public void TestTranslation() {
        PropertyKeeper src = new PropertyKeeper();
        PropertyKeeper dest = new PropertyKeeper();

        src.Value = "Good Morning";
        Abstractions.TranslationServices<NUnitAdapter> srvc =
            new Abstractions.TranslationServices<NUnitAdapter>();
        srvc.DoTranslation(src, dest);
        Assert.AreEqual("Guten Morgen", dest.Value);
    }
}

