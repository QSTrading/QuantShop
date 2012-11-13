namespace TradingLib.GUI
{
    partial class fmTimeSales
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
            this.ctTimeSales1 = new TradingLib.GUI.ctTimeSales();
            this.SuspendLayout();
            // 
            // ctTimeSales1
            // 
            this.ctTimeSales1.Location = new System.Drawing.Point(0, 0);
            this.ctTimeSales1.Name = "ctTimeSales1";
            this.ctTimeSales1.Size = new System.Drawing.Size(200, 600);
            this.ctTimeSales1.TabIndex = 0;
            this.ctTimeSales1.Type = TradingLib.API.TimeSalesType.Window;
            // 
            // fmTimeSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 600);
            this.Controls.Add(this.ctTimeSales1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "fmTimeSales";
            this.Text = "fmTimeSales";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.fmTimeSales_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fmTimeSales_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private ctTimeSales ctTimeSales1;
    }
}