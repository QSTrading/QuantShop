namespace TradingLib.GUI
{
    partial class ctTimesLineChart
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            Easychart.Finance.ExchangeIntraday exchangeIntraday1 = new Easychart.Finance.ExchangeIntraday();
            this.chartWinControl1 = new Easychart.Finance.Win.ChartWinControl();
            this.SuspendLayout();
            // 
            // chartWinControl1
            // 
            this.chartWinControl1.AreaPercent = "1";
            this.chartWinControl1.CausesValidation = false;
            this.chartWinControl1.ChartDragMode = Easychart.Finance.ChartDragMode.Chart;
            this.chartWinControl1.CurrentDataCycle.CycleBase = Easychart.Finance.DataCycleBase.MINUTE;
            this.chartWinControl1.DefaultCursor = System.Windows.Forms.Cursors.Default;
            this.chartWinControl1.DefaultFormulas = null;
            this.chartWinControl1.Designing = false;
            this.chartWinControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartWinControl1.EndTime = new System.DateTime(((long)(0)));
            this.chartWinControl1.FavoriteFormulas = "VOLMA;RSI;CCI;OBV;ATR;FastSTO;SlowSTO;ROC;TRIX;WR;AD;CMF;PPO;StochRSI;ULT;BBWidth" +
                ";PVO";
            exchangeIntraday1.TimeZone = -4D;
            this.chartWinControl1.IntradayInfo = exchangeIntraday1;
            this.chartWinControl1.Location = new System.Drawing.Point(0, 0);
            this.chartWinControl1.MaxPrice = 0D;
            this.chartWinControl1.MinPrice = 0D;
            this.chartWinControl1.Name = "chartWinControl1";
            this.chartWinControl1.PriceLabelFormat = null;
            this.chartWinControl1.ShowStatistic = false;
            this.chartWinControl1.Size = new System.Drawing.Size(519, 287);
            this.chartWinControl1.Skin = "GreenRed";
            this.chartWinControl1.StartTime = new System.DateTime(((long)(0)));
            this.chartWinControl1.StockRenderType = Easychart.Finance.StockRenderType.Line;
            this.chartWinControl1.TabIndex = 0;
            // 
            // ctTimesLineChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartWinControl1);
            this.Name = "ctTimesLineChart";
            this.Size = new System.Drawing.Size(519, 287);
            this.ResumeLayout(false);

        }

        #endregion

        private Easychart.Finance.Win.ChartWinControl chartWinControl1;

    }
}
