namespace TradingLib.GUI
{
    partial class fmPositionExitStrategy
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
            this.ctDefaultSymbolList1 = new TradingLib.GUI.ctDefaultSymbolList();
            this.ctPositionExitStrategy1 = new TradingLib.GUI.ctPositionExitStrategy();
            this.SuspendLayout();
            // 
            // ctDefaultSymbolList1
            // 
            this.ctDefaultSymbolList1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.ctDefaultSymbolList1.Location = new System.Drawing.Point(4, 0);
            this.ctDefaultSymbolList1.Name = "ctDefaultSymbolList1";
            this.ctDefaultSymbolList1.Size = new System.Drawing.Size(94, 302);
            this.ctDefaultSymbolList1.TabIndex = 1;
            // 
            // ctPositionExitStrategy1
            // 
            this.ctPositionExitStrategy1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctPositionExitStrategy1.Location = new System.Drawing.Point(104, 0);
            this.ctPositionExitStrategy1.Name = "ctPositionExitStrategy1";
            this.ctPositionExitStrategy1.Size = new System.Drawing.Size(495, 355);
            this.ctPositionExitStrategy1.TabIndex = 0;
            // 
            // fmPositionExitStrategy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 353);
            this.Controls.Add(this.ctDefaultSymbolList1);
            this.Controls.Add(this.ctPositionExitStrategy1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmPositionExitStrategy";
            this.Text = "fmSymbolExitStrategyQuckLink";
            this.ResumeLayout(false);

        }

        #endregion

        private ctPositionExitStrategy ctPositionExitStrategy1;
        private ctDefaultSymbolList ctDefaultSymbolList1;
    }
}