namespace TradingLib
{
    partial class ctChartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctChartForm));
            this.Designer = new Easychart.Finance.Win.ChartWinControl();
            this.ToolPanel = new Easychart.Finance.Objects.ObjectToolPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAdjustSize = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownTimeFrame = new System.Windows.Forms.ToolStripDropDownButton();
            this.mINToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mINToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mINToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mINToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mINToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.hOURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dAYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wEEKToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mONTHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yEARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonObject = new System.Windows.Forms.ToolStripDropDownButton();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Designer
            // 
            this.Designer.AreaPercent = "3;1;1";
            this.Designer.CausesValidation = false;
            this.Designer.CurrentDataCycle.CycleBase = Easychart.Finance.DataCycleBase.MINUTE;
            this.Designer.DefaultFormulas = "MAIN#AreaBB#MA(50)#MA(200);VOLMA;RSI(14)#RSI(28)";
            this.Designer.Designing = false;
            this.Designer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Designer.EndTime = new System.DateTime(((long)(0)));
            this.Designer.FavoriteFormulas = "VOLMA;RSI;CCI;OBV;ATR;FastSTO;SlowSTO;ROC;TRIX;WR;AD;CMF;PPO;StochRSI;ULT;BBWidth" +
                ";PVO";
            exchangeIntraday1.TimePeriods = new Easychart.Finance.TimePeriod[0];
            exchangeIntraday1.TimeZone = -4D;
            this.Designer.IntradayInfo = exchangeIntraday1;
            this.Designer.Location = new System.Drawing.Point(0, 0);
            this.Designer.MaxPrice = 0D;
            this.Designer.MinPrice = 0D;
            this.Designer.Name = "Designer";
            this.Designer.PriceLabelFormat = "\"{CODE} O:{OPEN} H:{HIGH} L:{LOW} C:{CLOSE} Chg:{CHANGE}\"";
            this.Designer.Size = new System.Drawing.Size(816, 514);
            this.Designer.Skin = "GreenRed";
            this.Designer.StartTime = new System.DateTime(((long)(0)));
            this.Designer.TabIndex = 0;
            // 
            // ToolPanel
            // 
            this.ToolPanel.Location = new System.Drawing.Point(0, 25);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.ResetAfterEachDraw = true;
            this.ToolPanel.Size = new System.Drawing.Size(102, 484);
            this.ToolPanel.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAdjustSize,
            this.toolStripDropDownButton1,
            this.toolStripDropDownTimeFrame,
            this.toolStripDropDownButtonObject,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(816, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripAdjustSize
            // 
            this.toolStripAdjustSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripAdjustSize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripAdjustSize.Image")));
            this.toolStripAdjustSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAdjustSize.Name = "toolStripAdjustSize";
            this.toolStripAdjustSize.Size = new System.Drawing.Size(23, 22);
            this.toolStripAdjustSize.Text = "toolStripButton2";
            this.toolStripAdjustSize.Click += new System.EventHandler(this.toolStripAdjustSize_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // toolStripDropDownTimeFrame
            // 
            this.toolStripDropDownTimeFrame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mINToolStripMenuItem,
            this.mINToolStripMenuItem1,
            this.mINToolStripMenuItem2,
            this.mINToolStripMenuItem3,
            this.mINToolStripMenuItem4,
            this.hOURToolStripMenuItem,
            this.dAYToolStripMenuItem,
            this.wEEKToolStripMenuItem,
            this.mONTHToolStripMenuItem,
            this.yEARToolStripMenuItem});
            this.toolStripDropDownTimeFrame.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownTimeFrame.Image")));
            this.toolStripDropDownTimeFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownTimeFrame.Name = "toolStripDropDownTimeFrame";
            this.toolStripDropDownTimeFrame.Size = new System.Drawing.Size(64, 22);
            this.toolStripDropDownTimeFrame.Text = "1分钟";
            // 
            // mINToolStripMenuItem
            // 
            this.mINToolStripMenuItem.Name = "mINToolStripMenuItem";
            this.mINToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.mINToolStripMenuItem.Text = "1MIN";
            this.mINToolStripMenuItem.Click += new System.EventHandler(this.mINToolStripMenuItem_Click);
            // 
            // mINToolStripMenuItem1
            // 
            this.mINToolStripMenuItem1.Name = "mINToolStripMenuItem1";
            this.mINToolStripMenuItem1.Size = new System.Drawing.Size(106, 22);
            this.mINToolStripMenuItem1.Text = "3MIN";
            this.mINToolStripMenuItem1.Click += new System.EventHandler(this.mINToolStripMenuItem1_Click);
            // 
            // mINToolStripMenuItem2
            // 
            this.mINToolStripMenuItem2.Name = "mINToolStripMenuItem2";
            this.mINToolStripMenuItem2.Size = new System.Drawing.Size(106, 22);
            this.mINToolStripMenuItem2.Text = "5MIN";
            this.mINToolStripMenuItem2.Click += new System.EventHandler(this.mINToolStripMenuItem2_Click);
            // 
            // mINToolStripMenuItem3
            // 
            this.mINToolStripMenuItem3.Name = "mINToolStripMenuItem3";
            this.mINToolStripMenuItem3.Size = new System.Drawing.Size(106, 22);
            this.mINToolStripMenuItem3.Text = "15MIN";
            this.mINToolStripMenuItem3.Click += new System.EventHandler(this.mINToolStripMenuItem3_Click);
            // 
            // mINToolStripMenuItem4
            // 
            this.mINToolStripMenuItem4.Name = "mINToolStripMenuItem4";
            this.mINToolStripMenuItem4.Size = new System.Drawing.Size(106, 22);
            this.mINToolStripMenuItem4.Text = "30MIN";
            this.mINToolStripMenuItem4.Click += new System.EventHandler(this.mINToolStripMenuItem4_Click);
            // 
            // hOURToolStripMenuItem
            // 
            this.hOURToolStripMenuItem.Name = "hOURToolStripMenuItem";
            this.hOURToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.hOURToolStripMenuItem.Text = "1HOUR";
            this.hOURToolStripMenuItem.Click += new System.EventHandler(this.hOURToolStripMenuItem_Click);
            // 
            // dAYToolStripMenuItem
            // 
            this.dAYToolStripMenuItem.Name = "dAYToolStripMenuItem";
            this.dAYToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.dAYToolStripMenuItem.Text = "1DAY";
            this.dAYToolStripMenuItem.Click += new System.EventHandler(this.dAYToolStripMenuItem_Click);
            // 
            // wEEKToolStripMenuItem
            // 
            this.wEEKToolStripMenuItem.Name = "wEEKToolStripMenuItem";
            this.wEEKToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.wEEKToolStripMenuItem.Text = "1WEEK";
            this.wEEKToolStripMenuItem.Click += new System.EventHandler(this.wEEKToolStripMenuItem_Click);
            // 
            // mONTHToolStripMenuItem
            // 
            this.mONTHToolStripMenuItem.Name = "mONTHToolStripMenuItem";
            this.mONTHToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.mONTHToolStripMenuItem.Text = "1MONTH";
            this.mONTHToolStripMenuItem.Click += new System.EventHandler(this.mONTHToolStripMenuItem_Click);
            // 
            // yEARToolStripMenuItem
            // 
            this.yEARToolStripMenuItem.Name = "yEARToolStripMenuItem";
            this.yEARToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.yEARToolStripMenuItem.Text = "1YEAR";
            this.yEARToolStripMenuItem.Click += new System.EventHandler(this.yEARToolStripMenuItem_Click);
            // 
            // toolStripDropDownButtonObject
            // 
            this.toolStripDropDownButtonObject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonObject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lineToolStripMenuItem,
            this.rayToolStripMenuItem,
            this.eToolStripMenuItem,
            this.hLineToolStripMenuItem,
            this.vLineToolStripMenuItem,
            this.toolStripSeparator1});
            this.toolStripDropDownButtonObject.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonObject.Image")));
            this.toolStripDropDownButtonObject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonObject.Name = "toolStripDropDownButtonObject";
            this.toolStripDropDownButtonObject.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonObject.Text = "toolStripDropDownButton2";
            // 
            // lineToolStripMenuItem
            // 
            this.lineToolStripMenuItem.Image = global::TradingLib.Properties.Resources.Easychart_Finance_Objects_Icons_Segment;
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // rayToolStripMenuItem
            // 
            this.rayToolStripMenuItem.Image = global::TradingLib.Properties.Resources.Easychart_Finance_Objects_Icons_Line1;
            this.rayToolStripMenuItem.Name = "rayToolStripMenuItem";
            this.rayToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.rayToolStripMenuItem.Text = "Ray";
            this.rayToolStripMenuItem.Click += new System.EventHandler(this.rayToolStripMenuItem_Click);
            // 
            // eToolStripMenuItem
            // 
            this.eToolStripMenuItem.Image = global::TradingLib.Properties.Resources.Easychart_Finance_Objects_Icons_Line2;
            this.eToolStripMenuItem.Name = "eToolStripMenuItem";
            this.eToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.eToolStripMenuItem.Text = "Extended Line";
            this.eToolStripMenuItem.Click += new System.EventHandler(this.eToolStripMenuItem_Click);
            // 
            // hLineToolStripMenuItem
            // 
            this.hLineToolStripMenuItem.Image = global::TradingLib.Properties.Resources.Easychart_Finance_Objects_Icons_HLine;
            this.hLineToolStripMenuItem.Name = "hLineToolStripMenuItem";
            this.hLineToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hLineToolStripMenuItem.Text = "H Line";
            this.hLineToolStripMenuItem.Click += new System.EventHandler(this.hLineToolStripMenuItem_Click);
            // 
            // vLineToolStripMenuItem
            // 
            this.vLineToolStripMenuItem.Image = global::TradingLib.Properties.Resources.Easychart_Finance_Objects_Icons_VLine;
            this.vLineToolStripMenuItem.Name = "vLineToolStripMenuItem";
            this.vLineToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.vLineToolStripMenuItem.Text = "V Line";
            this.vLineToolStripMenuItem.Click += new System.EventHandler(this.vLineToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // ctChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 514);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ToolPanel);
            this.Controls.Add(this.Designer);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ctChartForm";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Easychart.Finance.Win.ChartWinControl Designer;
        private Easychart.Finance.Objects.ObjectToolPanel ToolPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripAdjustSize;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownTimeFrame;
        private System.Windows.Forms.ToolStripMenuItem mINToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mINToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mINToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mINToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mINToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem hOURToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dAYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mONTHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yEARToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonObject;
        private System.Windows.Forms.ToolStripMenuItem lineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem hLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem wEEKToolStripMenuItem;

    }
}

