namespace SmartReverseDnsPlugIn
{
    partial class SettingsUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ddSubnet = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.chkHostReq = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ctlTTL1 = new SmartReverseDnsPlugIn.ctlTTL();
            this.ddIP = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "First IP address:";
            // 
            // txtIP
            // 
            this.txtIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIP.Location = new System.Drawing.Point(96, 0);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(281, 20);
            this.txtIP.TabIndex = 1;
            this.txtIP.TextChanged += new System.EventHandler(this.txtIP_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-3, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Subnet mask size:";
            // 
            // ddSubnet
            // 
            this.ddSubnet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddSubnet.FormattingEnabled = true;
            this.ddSubnet.Location = new System.Drawing.Point(96, 26);
            this.ddSubnet.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.ddSubnet.Name = "ddSubnet";
            this.ddSubnet.Size = new System.Drawing.Size(156, 21);
            this.ddSubnet.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-3, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(295, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Respond with PTR-record pointing to synthesized host name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(245, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Suffix:";
            // 
            // txtSuffix
            // 
            this.txtSuffix.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSuffix.Location = new System.Drawing.Point(248, 96);
            this.txtSuffix.Margin = new System.Windows.Forms.Padding(0, 3, 3, 10);
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(129, 20);
            this.txtSuffix.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Prefix (optional)";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(22, 96);
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(3, 3, 0, 10);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(120, 20);
            this.txtPrefix.TabIndex = 6;
            // 
            // chkHostReq
            // 
            this.chkHostReq.AutoSize = true;
            this.chkHostReq.Checked = true;
            this.chkHostReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHostReq.Location = new System.Drawing.Point(0, 167);
            this.chkHostReq.Name = "chkHostReq";
            this.chkHostReq.Size = new System.Drawing.Size(301, 17);
            this.chkHostReq.TabIndex = 15;
            this.chkHostReq.Text = "Respond to requests for matching host records (A / AAAA)";
            this.chkHostReq.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(-3, 203);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(269, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "* Dots / colons in IP address are replaced with hyphens";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-3, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Response TTL:";
            // 
            // ctlTTL1
            // 
            this.ctlTTL1.AutoSize = true;
            this.ctlTTL1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctlTTL1.BackColor = System.Drawing.Color.Transparent;
            this.ctlTTL1.Location = new System.Drawing.Point(96, 128);
            this.ctlTTL1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
            this.ctlTTL1.Name = "ctlTTL1";
            this.ctlTTL1.ReadOnly = false;
            this.ctlTTL1.Size = new System.Drawing.Size(156, 21);
            this.ctlTTL1.TabIndex = 14;
            this.ctlTTL1.Value = 3600;
            // 
            // ddIP
            // 
            this.ddIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddIP.FormattingEnabled = true;
            this.ddIP.Items.AddRange(new object[] {
            "Full",
            "Right part"});
            this.ddIP.Location = new System.Drawing.Point(155, 96);
            this.ddIP.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.ddIP.Name = "ddIP";
            this.ddIP.Size = new System.Drawing.Size(80, 21);
            this.ddIP.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(152, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "IP address (*):";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(142, 99);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "+";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(235, 99);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "+";
            // 
            // SettingsUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ddIP);
            this.Controls.Add(this.ctlTTL1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkHostReq);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ddSubnet);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Name = "SettingsUI";
            this.Size = new System.Drawing.Size(377, 251);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddSubnet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.CheckBox chkHostReq;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private ctlTTL ctlTTL1;
        private System.Windows.Forms.ComboBox ddIP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
    }
}
