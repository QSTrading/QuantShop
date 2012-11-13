namespace TradingLib.GUI
{
    partial class fmChartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmChartForm));
            Easychart.Finance.ExchangeIntraday exchangeIntraday1 = new Easychart.Finance.ExchangeIntraday();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripAdjustSize = new System.Windows.Forms.ToolStripButton();
            this.tbnSymbolsMenu = new System.Windows.Forms.ToolStripDropDownButton();
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
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDraw = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonObject = new System.Windows.Forms.ToolStripDropDownButton();
            this.lineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStatisWin = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTimeSales = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxHistoryDate = new System.Windows.Forms.ToolStripComboBox();
            this.tsbRequestHistData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.Designer = new Easychart.Finance.Win.ChartWinControl();
            this.ctTimeSales1 = new TradingLib.GUI.ctTimeSales();
            this.ToolPanel = new Easychart.Finance.Objects.ObjectToolPanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripAdjustSize,
            this.tbnSymbolsMenu,
            this.toolStripDropDownTimeFrame,
            this.toolStripSeparator2,
            this.toolStripDraw,
            this.toolStripDropDownButtonObject,
            this.toolStripButtonStatisWin,
            this.toolStripButtonTimeSales,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.toolStripComboBoxHistoryDate,
            this.tsbRequestHistData,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(988, 25);
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
            // tbnSymbolsMenu
            // 
            this.tbnSymbolsMenu.Image = ((System.Drawing.Image)(resources.GetObject("tbnSymbolsMenu.Image")));
            this.tbnSymbolsMenu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnSymbolsMenu.Name = "tbnSymbolsMenu";
            this.tbnSymbolsMenu.Size = new System.Drawing.Size(76, 22);
            this.tbnSymbolsMenu.Text = "Symbols";
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDraw
            // 
            this.toolStripDraw.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDraw.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDraw.Image")));
            this.toolStripDraw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDraw.Name = "toolStripDraw";
            this.toolStripDraw.Size = new System.Drawing.Size(23, 22);
            this.toolStripDraw.Text = "toolStripButton1";
            this.toolStripDraw.Click += new System.EventHandler(this.toolStripDraw_Click);
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
            this.lineToolStripMenuItem.Name = "lineToolStripMenuItem";
            this.lineToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.lineToolStripMenuItem.Text = "Line";
            this.lineToolStripMenuItem.Click += new System.EventHandler(this.lineToolStripMenuItem_Click);
            // 
            // rayToolStripMenuItem
            // 
            this.rayToolStripMenuItem.Name = "rayToolStripMenuItem";
            this.rayToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.rayToolStripMenuItem.Text = "Ray";
            this.rayToolStripMenuItem.Click += new System.EventHandler(this.rayToolStripMenuItem_Click);
            // 
            // eToolStripMenuItem
            // 
            this.eToolStripMenuItem.Name = "eToolStripMenuItem";
            this.eToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.eToolStripMenuItem.Text = "Extended Line";
            this.eToolStripMenuItem.Click += new System.EventHandler(this.eToolStripMenuItem_Click);
            // 
            // hLineToolStripMenuItem
            // 
            this.hLineToolStripMenuItem.Name = "hLineToolStripMenuItem";
            this.hLineToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.hLineToolStripMenuItem.Text = "H Line";
            this.hLineToolStripMenuItem.Click += new System.EventHandler(this.hLineToolStripMenuItem_Click);
            // 
            // vLineToolStripMenuItem
            // 
            this.vLineToolStripMenuItem.Name = "vLineToolStripMenuItem";
            this.vLineToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.vLineToolStripMenuItem.Text = "V Line";
            this.vLineToolStripMenuItem.Click += new System.EventHandler(this.vLineToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
            // 
            // toolStripButtonStatisWin
            // 
            this.toolStripButtonStatisWin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStatisWin.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStatisWin.Image")));
            this.toolStripButtonStatisWin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStatisWin.Name = "toolStripButtonStatisWin";
            this.toolStripButtonStatisWin.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStatisWin.Text = "toolStripButton1";
            this.toolStripButtonStatisWin.Click += new System.EventHandler(this.toolStripButtonStatisWin_Click);
            // 
            // toolStripButtonTimeSales
            // 
            this.toolStripButtonTimeSales.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonTimeSales.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTimeSales.Image")));
            this.toolStripButtonTimeSales.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTimeSales.Name = "toolStripButtonTimeSales";
            this.toolStripButtonTimeSales.Size = new System.Drawing.Size(33, 22);
            this.toolStripButtonTimeSales.Text = "盘口";
            this.toolStripButtonTimeSales.Click += new System.EventHandler(this.toolStripButtonTimeSales_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(53, 22);
            this.toolStripLabel1.Text = "历史数据";
            // 
            // toolStripComboBoxHistoryDate
            // 
            this.toolStripComboBoxHistoryDate.Name = "toolStripComboBoxHistoryDate";
            this.toolStripComboBoxHistoryDate.Size = new System.Drawing.Size(121, 25);
            // 
            // tsbRequestHistData
            // 
            this.tsbRequestHistData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbRequestHistData.Image = ((System.Drawing.Image)(resources.GetObject("tsbRequestHistData.Image")));
            this.tsbRequestHistData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRequestHistData.Name = "tsbRequestHistData";
            this.tsbRequestHistData.Size = new System.Drawing.Size(33, 22);
            this.tsbRequestHistData.Text = "刷新";
            this.tsbRequestHistData.Click += new System.EventHandler(this.tsbRequestHistData_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
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
            this.Designer.Location = new System.Drawing.Point(0, 25);
            this.Designer.MaxPrice = 0D;
            this.Designer.MinPrice = 0D;
            this.Designer.Name = "Designer";
            this.Designer.PriceLabelFormat = "\"{CODE} O:{OPEN} H:{HIGH} L:{LOW} C:{CLOSE} Chg:{CHANGE}\"";
            this.Designer.Size = new System.Drawing.Size(788, 559);
            this.Designer.Skin = "GreenRed";
            this.Designer.StartTime = new System.DateTime(((long)(0)));
            this.Designer.StickRenderType = Easychart.Finance.StickRenderType.Column;
            this.Designer.TabIndex = 7;
            this.Designer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Designer_KeyDown);
            // 
            // ctTimeSales1
            // 
            this.ctTimeSales1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ctTimeSales1.Location = new System.Drawing.Point(788, 25);
            this.ctTimeSales1.Name = "ctTimeSales1";
            this.ctTimeSales1.Size = new System.Drawing.Size(200, 559);
            this.ctTimeSales1.TabIndex = 6;
            this.ctTimeSales1.Type = TradingLib.API.TimeSalesType.Window;
            // 
            // ToolPanel
            // 
            this.ToolPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.ToolPanel.Location = new System.Drawing.Point(0, 25);
            this.ToolPanel.Name = "ToolPanel";
            this.ToolPanel.ResetAfterEachDraw = true;
            this.ToolPanel.Size = new System.Drawing.Size(120, 559);
            this.ToolPanel.TabIndex = 8;
            // 
            // fmChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(988, 584);
            this.Controls.Add(this.ToolPanel);
            this.Controls.Add(this.Designer);
            this.Controls.Add(this.ctTimeSales1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HideOnClose = true;
            this.Name = "fmChartForm";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripAdjustSize;
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
        private System.Windows.Forms.ToolStripButton toolStripDraw;
        private System.Windows.Forms.ToolStripMenuItem hLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem wEEKToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tbnSymbolsMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxHistoryDate;
        private System.Windows.Forms.ToolStripButton tsbRequestHistData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private ctTimeSales ctTimeSales1;
        private System.Windows.Forms.ToolStripButton toolStripButtonTimeSales;
        private System.Windows.Forms.ToolStripButton toolStripButtonStatisWin;
        private Easychart.Finance.Win.ChartWinControl Designer;
        private Easychart.Finance.Objects.ObjectToolPanel ToolPanel;

    }
}

