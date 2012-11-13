namespace TradingLib.GUI
{
    partial class fmSimpleChart
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
            Easychart.Finance.ExchangeIntraday exchangeIntraday1 = new Easychart.Finance.ExchangeIntraday();
            this.Designer = new Easychart.Finance.Win.ChartWinControl();
            this.SuspendLayout();
            // 
            // Designer
            // 
            this.Designer.AreaPercent = "3;1;1";
            this.Designer.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Designer.CausesValidation = false;
            this.Designer.ChartDragMode = Easychart.Finance.ChartDragMode.Chart;
            this.Designer.CurrentDataCycle.CycleBase = Easychart.Finance.DataCycleBase.MINUTE;
            this.Designer.DefaultFormulas = "MAIN#KELTNER;VOLMA;FastSTO";
            this.Designer.Designing = false;
            this.Designer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Designer.EndTime = new System.DateTime(((long)(0)));
            this.Designer.FavoriteFormulas = "QSTradingInfo;VOLMA;MACD;FastSTO;RSI;WR;";
            exchangeIntraday1.TimePeriods = new Easychart.Finance.TimePeriod[0];
            exchangeIntraday1.TimeZone = -4D;
            this.Designer.IntradayInfo = exchangeIntraday1;
            this.Designer.Location = new System.Drawing.Point(0, 0);
            this.Designer.MaxPrice = 0D;
            this.Designer.MinPrice = 0D;
            this.Designer.Name = "Designer";
            this.Designer.PriceLabelFormat = "\"{CODE} O:{OPEN} H:{HIGH} L:{LOW} C:{CLOSE} Chg:{CHANGE}\"";
            this.Designer.Size = new System.Drawing.Size(1117, 549);
            this.Designer.Skin = "GreenRed";
            this.Designer.StartTime = new System.DateTime(((long)(0)));
            this.Designer.StickRenderType = Easychart.Finance.StickRenderType.Column;
            this.Designer.TabIndex = 8;
            // 
            // fmSimpleChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 549);
            this.Controls.Add(this.Designer);
            this.Name = "fmSimpleChart";
            this.Text = "fmSimpleChart";
            this.ResumeLayout(false);

        }

        #endregion

        private Easychart.Finance.Win.ChartWinControl Designer;
    }
}