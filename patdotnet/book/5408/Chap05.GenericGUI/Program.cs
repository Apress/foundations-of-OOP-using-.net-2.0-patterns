using System;
using System.Collections.Generic;
using System.Windows.Forms;
using wx;

namespace TemplateExample {
    abstract class TemplateBaseClass {
        public abstract int Add( int num1, int num2);

        public int DoThreeNumberAdd(int num1, int num2, int num3) {
            return Add(num1, Add(num2, num3));
        }
    }
}

namespace TemplateGenericsExample {
    interface IIntMath {
        int Add(int num1, int num2);
    }

    class TemplateBaseClass< additiontype> where additiontype : IIntMath, new() {
        public int DoThreeNumberAdd(int num1, int num2, int num3) {
            IIntMath cls = new additiontype();
            return cls.Add(num1, cls.Add(num2, num3));
        }
    }
}

namespace Adapter {
    class WxToFormsAdapter: System.Windows.Forms.TextBox {
        wx.TextCtrl _ctrl = new wx.TextCtrl((wx.Window)null);

        public override string Text {
            get {
                return _ctrl.Value;
            }
            set {
                _ctrl.Value = value;
            }
        }
    }

    class BusinessLogic {
        void TransferData<Control1, Control2>(Control1 ctrl1, Control2 ctrl2)
            where Control1: System.Windows.Forms.TextBox
            where Control2: System.Windows.Forms.TextBox {
            ctrl1.Text = ctrl2.Text;
        }
    }
}



namespace Chap05.GenericGUI {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.Run(new MediatorForm());
            //Application.Run( new FormUsingLoadable() );
            //Application.Run( new BasicForm() );
            //Application.Run( new Form1() );

            //HelloWorld hello = new HelloWorld();
            //hello.Run();
        }
    }
}