using System;
using wx;

public class HelloWorldFrame : Frame {
    public enum Cmd {
        ID_MAINBUTTON,
        ID_TEXT1,
        ID_TEXT2,
    }
    protected Button mainbutton;
    protected TextCtrl text1;
    protected TextCtrl text2;
    protected BoxSizer sizer;

    public HelloWorldFrame( Window parent, int id, string title )
        :
        base( parent, id, "Helloworldframe", wxDefaultPosition, wxDefaultSize, wxDEFAULT_FRAME_STYLE ) {
        mainbutton = new Button( this, (int)Cmd.ID_MAINBUTTON, "Press Me" );
        mainbutton.Click += new EventListener( OnButtonClicked );

        text1 = new TextCtrl( this, (int)Cmd.ID_TEXT1, "" );
        text2 = new TextCtrl( this, (int)Cmd.ID_TEXT2, "" );
        sizer = new BoxSizer( Orientation.wxVERTICAL );
        sizer.Add( mainbutton, 1, Direction.wxALL | Stretch.wxEXPAND, 2 );
        sizer.Add( text1, 1, Direction.wxALL | Stretch.wxEXPAND, 2 );
        sizer.Add( text2, 1, Direction.wxALL | Stretch.wxEXPAND, 2 );
        sizer.Fit( this );
        sizer.SetSizeHints( this );
        SetSizer( sizer );
        Layout();

    }

    private class wxAdapter : Abstractions.IControlAdapter {
        #region IControlAdapter Members

        public type GetValue<ControlType, type>( ControlType control ) where type : class {
            if( control is TextCtrl ) {
                TextCtrl cls = control as TextCtrl;
                return cls.Value as type;
            }
            return default( type );
        }

        public void SetValue<ControlType, type>( ControlType control, type value ) {
            if( control is TextCtrl ) {
                TextCtrl cls = control as TextCtrl;
                String strValue = value as String;
                cls.Value = strValue;
            }
        }

        #endregion
    }
    public void OnButtonClicked( object sender, Event e ) {
        Abstractions.BusinessLogic< wxAdapter> cls = new Abstractions.BusinessLogic<wxAdapter>();
        cls.TransferData( text1, text2 );
    }
}

public class HelloWorld : App {
    public override bool OnInit() {
        HelloWorldFrame _eventdemoframe = new HelloWorldFrame( null, -1, "" );
        _eventdemoframe.Show( true );
        return true;
    }
}



