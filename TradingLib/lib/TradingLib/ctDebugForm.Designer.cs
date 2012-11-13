namespace TradingLib
{
    partial class ctDebugForm
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
            this.debugControl1 = new TradeLink.AppKit.DebugControl();
            this.SuspendLayout();
            // 
            // debugControl1
            // 
            this.debugControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugControl1.EnableSearching = true;
            this.debugControl1.ExternalTimeStamp = 0;
            this.debugControl1.Location = new System.Drawing.Point(0, 0);
            this.debugControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.debugControl1.Name = "debugControl1";
            this.debugControl1.Size = new System.Drawing.Size(658, 266);
            this.debugControl1.TabIndex = 0;
            this.debugControl1.TimeStamps = true;
            this.debugControl1.UseExternalTimeStamp = false;
            // 
            // DebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 266);
            this.Controls.Add(this.debugControl1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "DebugForm";
            this.Text = "DebugMessage";
            this.ResumeLayout(false);

        }

        #endregion

        private TradeLink.AppKit.DebugControl debugControl1;
    }
}