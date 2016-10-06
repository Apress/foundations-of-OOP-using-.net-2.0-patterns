using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chap05.GenericGUI {
    public partial class FormAdaptable : Form {
        public FormAdaptable() {
            InitializeComponent();
        }

        private class WindowsAdapter : Abstractions.IControlAdapter {
            #region IControlAdapter Members

            public type GetValue<ControlType, type>( ControlType control ) where type : class {
                if( control is TextBox ) {
                    TextBox cls = control as TextBox;
                    return cls.Text as type;
                }
                return default( type );
            }

            public void SetValue<ControlType, type>( ControlType control, type value ) {
                if( control is TextBox ) {
                    TextBox cls = control as TextBox;
                    String strValue = value as String;
                    cls.Text = strValue;
                }
            }

            #endregion
        }
        private void button1_Click( object sender, EventArgs e ) {
            Abstractions.BusinessLogic<WindowsAdapter> cls = new Abstractions.BusinessLogic< WindowsAdapter>();
            cls.TransferData( textBox1, textBox2 );
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }
    }
}

