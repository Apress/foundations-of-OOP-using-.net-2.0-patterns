using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chap05.GenericGUI {
    public partial class BasicForm : Form {
        private Original.TranslateToGerman _translation = new Original.TranslateToGerman();

        public BasicForm() {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e ) {
            textBox2.Text = _translation.Translate( textBox1.Text );
        }
    }
}