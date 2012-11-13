namespace TradingLib.GUI.Server
{
    partial class demo
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
            this.ctRuleSet1 = new TradingLib.GUI.Server.ctRuleSet();
            this.SuspendLayout();
            // 
            // ctRuleSet1
            // 
            this.ctRuleSet1.Location = new System.Drawing.Point(71, 12);
            this.ctRuleSet1.Name = "ctRuleSet1";
            this.ctRuleSet1.Size = new System.Drawing.Size(713, 398);
            this.ctRuleSet1.TabIndex = 0;
            // 
            // demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(939, 474);
            this.Controls.Add(this.ctRuleSet1);
            this.Name = "demo";
            this.Text = "demo";
            this.ResumeLayout(false);

        }

        #endregion

        private ctRuleSet ctRuleSet1;
    }
}