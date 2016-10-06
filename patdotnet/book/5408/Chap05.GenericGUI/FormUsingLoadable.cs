using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Chap05.TranslationDefinitions;

namespace Chap05.GenericGUI {
    public partial class FormUsingLoadable : Form {
        private ITranslationServices _translation;
        private Loader _loader;

        public FormUsingLoadable() {
            InitializeComponent();
            _loader = new Loader();
            //_loader.Load();
            _translation = _loader.CreateGermanTranslationDynamic();
        }

        private void button1_Click( object sender, EventArgs e ) {
            textBox2.Text = _translation.Translate( textBox1.Text );
        }
    }
}

