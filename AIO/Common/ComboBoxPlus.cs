
using System;

using System.ComponentModel;

namespace System.Windows.Forms
{

    //public class TextBoxPlus : System.Windows.Forms.TextBox
    //{
    //    private const int CB_SETCUEBANNER = 0x1501;

    //    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
    //    private static extern int SendMessage(System.IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);

    //    private string placeholder = string.Empty;

    //    public string Placeholder
    //    {
    //        get { return placeholder; }
    //        set
    //        {
    //            placeholder = value;
    //            SendMessage(Handle, CB_SETCUEBANNER, 0, Placeholder);
    //        }
    //    }

    //}


    public class ComboBoxPlus : System.Windows.Forms.ComboBox
    {
        private const int CB_SETCUEBANNER = 0x1703;

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int SendMessage(System.IntPtr hWnd, int msg, int wParam, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lParam);

        private string placeholder = string.Empty;

        public string Placeholder
        {
            get { return placeholder; }
            set
            {
                placeholder = value;
                SendMessage(Handle, CB_SETCUEBANNER, 0, Placeholder);
            }
        }

    }

    //public partial class HintComboBox : System.Windows.Forms.ComboBox
    //{
    //    string hint;
    //    Font greyFont;
    //    [Localizable(true)]
    //    public string Hint
    //    {
    //        get { return hint; }
    //        set { hint = value; Invalidate(); }
    //    }
    //    public HintComboBox()
    //    {
    //        InitializeComponent();
    //    }
    //    protected override void OnCreateControl()
    //    {
    //        base.OnCreateControl();
    //        if (string.IsNullOrEmpty(Text))
    //        {
    //            this.ForeColor = SystemColors.GrayText;
    //            Text = Hint;
    //        }
    //        else
    //        {
    //            this.ForeColor = Color.Black;
    //        }
    //    }
    //    private void HintComboBox_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        if (string.IsNullOrEmpty(Text))
    //        {
    //            this.ForeColor = SystemColors.GrayText;
    //            Text = Hint;
    //        }
    //        else
    //        {
    //            this.ForeColor = Color.Black;
    //        }
    //    }
    //}

}