using System;
using Chap05.TranslationDefinitions;

namespace Original {
    public class TranslateToGerman {
        public string Translate(string word) {
            if(String.Compare(word, "Good Morning") == 0) {
                return "Guten Morgen";
            }
            else {
                return "Could not translate";
            }
        }
    }
}

namespace Abstractions {


    public interface IControlAdapter {
        returntype GetValue<ControlType, returntype>(ControlType control) where returntype: class;
        void SetValue<ControlType, type>(ControlType control, type value);
    }

    public class BusinessLogic<GUIAdapter> where GUIAdapter: IControlAdapter, new() {
        private IControlAdapter _adapter;

        public BusinessLogic() {
            _adapter = new GUIAdapter();
        }
        public virtual void TransferData<Control1, Control2>(Control1 ctrl1, Control2 ctrl2) {
            _adapter.SetValue(ctrl2, _adapter.GetValue<Control1, string>(ctrl1));
        }
    }

    public class TranslationServices<GUIAdapter> where GUIAdapter: IControlAdapter, new() {
        private ITranslationServices _translation;
        private Loader _loader;
        private GUIAdapter _adapter;

        public TranslationServices() {
            _loader = new Loader();
            //_loader.Load();
            _translation = _loader.CreateGermanTranslationDynamic();
            _adapter = new GUIAdapter();
        }
        public void DoTranslation<Control1, Control2>(Control1 src, Control2 dest) {
            _adapter.SetValue(dest, _translation.Translate(_adapter.GetValue<Control1, string>(src)));
        }
    }
}



/*
public partial class Person
{
    public string firstname;
    public string lastname;
        
    public Person (string firstname, string lastname)
    {
        this.firstname = firstname;
        this.lastname = lastname;
    }
    
    public void Format ()
    {
        // ...
    }
}

public partial class Person
{
    public static Person FromDataRow (DataRow r)
    {
        // ...
    }
    
    public static Person FromAnotherDataSource (DS s)
    {
        // ..
    }
}
*/
