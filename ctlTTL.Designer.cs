namespace SmartReverseDnsPlugIn
{
    partial class ctlTTL
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
            this.txtVal = new System.Windows.Forms.TextBox();
            this.comExt = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtVal
            // 
            this.txtVal.Location = new System.Drawing.Point(0, 0);
            this.txtVal.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.txtVal.Name = "txtVal";
            this.txtVal.Size = new System.Drawing.Size(75, 20);
            this.txtVal.TabIndex = 0;
            this.txtVal.Text = "0";
            this.txtVal.KeyPress+=txtVal_KeyPress;
            this.txtVal.TextChanged+=txtVal_TextChanged;
            // 
            // comExt
            // 
            this.comExt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comExt.FormattingEnabled = true;
            this.comExt.Items.AddRange(new object[] {
            "Seconds",
            "Minutes",
            "Hours",
            "Days"});
            this.comExt.Location = new System.Drawing.Point(81, 0);
            this.comExt.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.comExt.Name = "comExt";
            this.comExt.Size = new System.Drawing.Size(75, 21);
            this.comExt.TabIndex = 1;
            // 
            // ctlTTL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.comExt);
            this.Controls.Add(this.txtVal);
            this.Name = "ctlTTL";
            this.Size = new System.Drawing.Size(156, 21);
            this.ResumeLayout(false);
            this.PerformLayout();
            this.Validating+=ctlTTL_Validating;

        }

        #endregion

        private System.Windows.Forms.TextBox txtVal;
        private System.Windows.Forms.ComboBox comExt;
    }
}
