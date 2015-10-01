using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SmartReverseDnsPlugIn
{
    public partial class SettingsUI : JHSoftware.SimpleDNS.Plugin.OptionsUI 
    {
        private bool IPv6 ;

        public SettingsUI()
        {
            InitializeComponent();
            FillSubNetDD();
            ddIP.SelectedIndex = 0;
        }

        public override void LoadData(string config)
        {
            if (string.IsNullOrEmpty(config)) return;
            var cfg = SrdConfig.DeSerialize(config);
            txtIP.Text = cfg.FirstIP;
            ddSubnet.SelectedIndex =IPv6 ? (124 - cfg.Subnet) / 4 : (24 - cfg.Subnet) / 8;
            txtPrefix.Text = cfg.Prefix;
            ddIP.SelectedIndex= cfg.FullIP ? 0 : 1;
            txtSuffix.Text = cfg.Suffix;
            ctlTTL1.Value = cfg.TTL;
            chkHostReq.Checked = cfg.HostReq;
        }

        public override string SaveData()
        {
            var rv = new SrdConfig();
            rv.FirstIP = txtIP.Text.Trim();
            rv.Subnet = IPv6 ? 124 - 4 * ddSubnet.SelectedIndex : 24 - 8 * ddSubnet.SelectedIndex;
            rv.Prefix = txtPrefix.Text.Trim().ToLower();
            rv.FullIP = ddIP.SelectedIndex==0;
            rv.Suffix = txtSuffix.Text.Trim().ToLower();
            rv.TTL = ctlTTL1.Value;
            rv.HostReq = chkHostReq.Checked;
            return rv.Serialize();
        }

        private void FillSubNetDD()
        {
            if(IPv6)
            {
                if (ddSubnet.Items.Count > 3) return;
                ddSubnet.Items.Clear();
                for (var i = 124; i > 0; i -= 4) ddSubnet.Items.Add(i.ToString());
            }
            else
            {
                if (ddSubnet.Items.Count == 3) return;
                ddSubnet.Items.Clear();
                ddSubnet.Items.Add("24  (255.255.255.0)");
                ddSubnet.Items.Add("16  (255.255.0.0)");
                ddSubnet.Items.Add("8  (255.0.0.0)");
            }
            ddSubnet.SelectedIndex = 0;
        }

        public override bool ValidateData()
        {
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(txtIP.Text.Trim(), out ip))  { ShowErr("Invalid first IP address"); return false; }
            txtPrefix.Text = txtPrefix.Text.Trim().ToLower();
            if (txtPrefix.Text.Length > 0)
            {
                if (!ValidHostName(txtPrefix.Text)) { ShowErr("Invalid host name prefix"); return false; }
                if (txtPrefix.Text.StartsWith(".") || txtPrefix.Text.StartsWith("-")) { ShowErr("Invalid host name prefix"); return false; }
            }
            txtSuffix.Text = txtSuffix.Text.Trim().ToLower();
            if (txtSuffix.Text.Length ==0) { ShowErr("Host name suffix is required"); return false; }
            if (!ValidHostName(txtSuffix.Text)) { ShowErr("Invalid host name suffix"); return false; }
            if (txtSuffix.Text.EndsWith(".") || txtSuffix.Text.EndsWith("-")) { ShowErr("Invalid host name suffix"); return false; }
            return true;
        }

        private void ShowErr(string msg)
        {
            MessageBox.Show(msg, "Smart reverse DNS plug-in", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private bool ValidHostName(string hn)
        {
            for (var i = 0; i < hn.Length; i++)
            {
                if(!((hn[i] >='a' && hn[i]<='z') || (hn[i] >='0' && hn[i]<='9') || hn[i] == '-' || hn[i] == '.')) return false;
            }
            if (hn.IndexOf("..") >= 0) return false;
            return true;
        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {
            System.Net.IPAddress ip;
            if (!System.Net.IPAddress.TryParse(txtIP.Text.Trim(), out ip)) return;
            IPv6 = (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
            FillSubNetDD();
        }
    }
}
