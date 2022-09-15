namespace AIO.Common
{
    partial class AuthorizeControl
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
            this.btnLoginLogout = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.cbMirrorDomain = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbApiKey = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnLoginLogout
            // 
            this.btnLoginLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoginLogout.Location = new System.Drawing.Point(661, 28);
            this.btnLoginLogout.Name = "btnLoginLogout";
            this.btnLoginLogout.Size = new System.Drawing.Size(75, 23);
            this.btnLoginLogout.TabIndex = 13;
            this.btnLoginLogout.Text = "Login";
            this.btnLoginLogout.UseVisualStyleBackColor = true;
            this.btnLoginLogout.Click += new System.EventHandler(this.btnLoginLogout_Click);
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(436, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(28, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "Site:";
            // 
            // cbMirrorDomain
            // 
            this.cbMirrorDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbMirrorDomain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMirrorDomain.FormattingEnabled = true;
            this.cbMirrorDomain.Location = new System.Drawing.Point(439, 29);
            this.cbMirrorDomain.Name = "cbMirrorDomain";
            this.cbMirrorDomain.Size = new System.Drawing.Size(216, 21);
            this.cbMirrorDomain.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 13);
            this.label12.TabIndex = 10;
            this.label12.Text = "Api key:";
            // 
            // tbApiKey
            // 
            this.tbApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApiKey.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbApiKey.Location = new System.Drawing.Point(6, 29);
            this.tbApiKey.Name = "tbApiKey";
            this.tbApiKey.Size = new System.Drawing.Size(427, 20);
            this.tbApiKey.TabIndex = 9;
            this.tbApiKey.UseSystemPasswordChar = true;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(737, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(52, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "Currency:";
            // 
            // cbCurrency
            // 
            this.cbCurrency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrency.FormattingEnabled = true;
            this.cbCurrency.Location = new System.Drawing.Point(742, 29);
            this.cbCurrency.Name = "cbCurrency";
            this.cbCurrency.Size = new System.Drawing.Size(166, 21);
            this.cbCurrency.TabIndex = 14;
            this.cbCurrency.SelectedIndexChanged += new System.EventHandler(this.cbCurrency_SelectedIndexChanged);
            // 
            // AuthorizeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbCurrency);
            this.Controls.Add(this.btnLoginLogout);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbMirrorDomain);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbApiKey);
            this.Name = "AuthorizeControl";
            this.Size = new System.Drawing.Size(915, 63);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoginLogout;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbMirrorDomain;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbApiKey;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbCurrency;
    }
}
