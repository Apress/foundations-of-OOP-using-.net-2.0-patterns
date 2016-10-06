using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Chap05.TranslationDefinitions;

/*
public class TranslationTextBox : System.Windows.Forms.TextBox {
    private System.Windows.Forms.TextBox _textBox;
    private System.Windows.Forms.Form _form;

    private ITranslationServices _translation;

    public TranslationTextBox(System.Windows.Forms.TextBox textbox) {
        _textBox = textbox;
    }
    protected override void OnTextChanged( EventArgs e) {
        _textBox.Text = _translation.Translate(this.Text);
    }
    public void AssignTranslation(ITranslationServices translation) {
        _translation = translation;
    }
}
public class TranslationTextBox: System.Windows.Forms.TextBox {
        private ITranslationServices _translation;
        private Loader _loader;

    public TranslationTextBox() {
        _loader = new Loader();
        _loader.Load();
        _translation = _loader.CreateGermanTranslationDynamic();
    }
    protected override void OnTextChanged( EventArgs e) {
        DoTranslation();
    }
    public void DoTranslation() {
        Form parent;
        TextBox othertextbox;

        parent = this.Parent as Form;
        othertextbox = parent.Controls["textbox2"] as TextBox;
        othertextbox.Text = _translation.Translate(this.Text);
    }
}
*/


public class TranslationTextBox: System.Windows.Forms.TextBox, Abstractions.IControlAdapter {

    #region IControlAdapter Members
    public type GetValue<ControlType, type>(ControlType control) where type: class {
        if(control is TextBox) {
            TextBox cls = control as TextBox;
            return cls.Text as type;
        }
        return default(type);
    }

    public void SetValue<ControlType, type>(ControlType control, type value) {
        if(control is TextBox) {
            TextBox cls = control as TextBox;
            String strValue = value as String;
            cls.Text = strValue;
        }
    }
    #endregion

    public TranslationTextBox() {
    }
    protected override void OnTextChanged(EventArgs e) {
        DoTranslation();
    }
    public void DoTranslation() {
        Form parent;
        TextBox othertextbox;

        parent = this.Parent as Form;
        othertextbox = parent.Controls["textbox2"] as TextBox;

        Abstractions.TranslationServices<TranslationTextBox> srvc =
            new Abstractions.TranslationServices<TranslationTextBox>();
        srvc.DoTranslation( this, othertextbox);
    }
}

namespace Chap05.GenericGUI {
    public partial class MediatorForm: Form {

        public MediatorForm() {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e ) {
            textBox1.DoTranslation();
        }
    }
}