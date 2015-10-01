using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SmartReverseDnsPlugIn
{
    public partial class ctlTTL : UserControl
    {
        public ctlTTL()
        {
            InitializeComponent();
        }

       private bool DoChangeEvents = true;

        public bool ReadOnly
        {
            get { return txtVal.ReadOnly; }
            set
            {
                txtVal.ReadOnly = value;
                comExt.Enabled = (value == false);
            }
        }

        [System.ComponentModel.DefaultValue(0)]
        public int Value
        {
            get
            {
                int rv = 0;
                long lv = 0;
                if (!long.TryParse(txtVal.Text,out lv))
                    return 0;
                if (lv < 0)
                    return 0;
                if (lv > 2147483647)
                    rv = 2147483647;
                else
                    rv = Convert.ToInt32(lv);
                switch (comExt.SelectedIndex)
                {
                    case 0:
                        // seconds
                        return rv;
                    case 1:
                        // minutes
                        if (rv > 35791394)
                            rv = 35791394;
                        return rv * 60;
                    case 2:
                        // hours
                        if (rv > 596523)
                            rv = 596523;
                        return rv * 3600;
                    case 3:
                        // days
                        if (rv > 24855)
                            rv = 24855;
                        return rv * 86400;
                }
                return 3600;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("TTL must be zero or greater");
                DoChangeEvents = false;
                if (value == 0)
                {
                    txtVal.Text = "0";
                    comExt.SelectedIndex = 0;
                }
                else if (value % 86400 == 0)
                {
                    comExt.SelectedIndex = 3;
                    txtVal.Text = (value / 86400).ToString();
                }
                else if (value % 3600 == 0)
                {
                    comExt.SelectedIndex = 2;
                    txtVal.Text = (value / 3600).ToString();
                }
                else if (value % 60 == 0)
                {
                    comExt.SelectedIndex = 1;
                    txtVal.Text = (value / 60).ToString();
                }
                else
                {
                    comExt.SelectedIndex = 0;
                    txtVal.Text = value.ToString();
                }
                DoChangeEvents = true;
            }
        }

        private void txtVal_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9') return;
            switch (e.KeyChar)
            {
                case 's':
                case 'S':
                    comExt.SelectedIndex = 0;
                    break;
                case 'm':
                case 'M':
                    comExt.SelectedIndex = 1;
                    break;
                case 'h':
                case 'H':
                    comExt.SelectedIndex = 2;
                    break;
                case 'd':
                case 'D':
                    comExt.SelectedIndex = 3;
                    break;
                default:
                    if (e.KeyChar < 32 | e.KeyChar == 127)
                        return;

                    // control chars
                    break;
            }
            e.Handled = true;
        }

        private void txtVal_TextChanged(System.Object sender, System.EventArgs e)
        {
            if (!DoChangeEvents)
                return;
            // for example on paste
            string nv = "";
            for (int i = 0; i <= txtVal.Text.Length - 1; i++)
            {
                if (txtVal.Text[i] >= '0' && txtVal.Text[i] <= '9')
                    nv += txtVal.Text[i];
            }
            if (txtVal.Text != nv)
                txtVal.Text = nv;
        }

        private void ctlTTL_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Value = Value;
        }
    }
}
