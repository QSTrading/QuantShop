namespace TradingLib.GUI
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
            this.button1 = new System.Windows.Forms.Button();
            this.ctTimesLineChart1 = new TradingLib.GUI.ctTimesLineChart();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(571, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ctTimesLineChart1
            // 
            this.ctTimesLineChart1.Location = new System.Drawing.Point(0, 42);
            this.ctTimesLineChart1.Name = "ctTimesLineChart1";
            this.ctTimesLineChart1.Size = new System.Drawing.Size(657, 336);
            this.ctTimesLineChart1.TabIndex = 0;
            // 
            // demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 373);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ctTimesLineChart1);
            this.Name = "demo";
            this.Text = "demo";
            this.ResumeLayout(false);

        }

        #endregion

        private ctTimesLineChart ctTimesLineChart1;
        private System.Windows.Forms.Button button1;


    }
}