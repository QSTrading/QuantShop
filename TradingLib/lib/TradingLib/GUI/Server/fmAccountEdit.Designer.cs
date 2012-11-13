namespace TradingLib.GUI.Server
{
    partial class fmAccountEdit
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabAccount = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ctRuleSet1 = new TradingLib.GUI.Server.ctRuleSet();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabAccount.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabAccount
            // 
            this.tabAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabAccount.Controls.Add(this.tabPage1);
            this.tabAccount.Controls.Add(this.tabPage2);
            this.tabAccount.Controls.Add(this.tabPage3);
            this.tabAccount.Controls.Add(this.tabPage4);
            this.tabAccount.Location = new System.Drawing.Point(0, 0);
            this.tabAccount.Name = "tabAccount";
            this.tabAccount.SelectedIndex = 0;
            this.tabAccount.Size = new System.Drawing.Size(713, 344);
            this.tabAccount.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(705, 319);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ctRuleSet1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(705, 319);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "交易设定";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ctRuleSet1
            // 
            this.ctRuleSet1.Location = new System.Drawing.Point(0, 3);
            this.ctRuleSet1.Name = "ctRuleSet1";
            this.ctRuleSet1.Size = new System.Drawing.Size(700, 309);
            this.ctRuleSet1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(705, 319);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "智能提示";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 21);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(705, 319);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "仓位管理";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // fmAccountEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 344);
            this.Controls.Add(this.tabAccount);
            this.Name = "fmAccountEdit";
            this.Text = "fmAccountEdit";
            this.tabAccount.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabAccount;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ctRuleSet ctRuleSet1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
    }
}