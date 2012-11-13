namespace TradingLib.GUI
{
    partial class fmOrdEntry
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
            this.spillTick1 = new TradingLib.GUI.SpillTick();
            this.tradeTab = new System.Windows.Forms.TabPage();
            this.tradeView1 = new TradingLib.GUI.TradeView();
            this.posTab = new System.Windows.Forms.TabPage();
            this.positionView1 = new TradingLib.GUI.PositionView();
            this.quoteTab = new System.Windows.Forms.TabPage();
            this.quoteView1 = new TradingLib.GDIControl.ViewQuoteList();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.orderView1 = new TradingLib.GUI.OrderView();
            this.tradeTab.SuspendLayout();
            this.posTab.SuspendLayout();
            this.quoteTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spillTick1
            // 
            this.spillTick1.AssumeNewOrder = true;
            this.spillTick1.Location = new System.Drawing.Point(-1, -1);
            this.spillTick1.Name = "spillTick1";
            this.spillTick1.Size = new System.Drawing.Size(205, 201);
            this.spillTick1.TabIndex = 0;
            // 
            // tradeTab
            // 
            this.tradeTab.Controls.Add(this.tradeView1);
            this.tradeTab.Location = new System.Drawing.Point(4, 21);
            this.tradeTab.Margin = new System.Windows.Forms.Padding(0);
            this.tradeTab.Name = "tradeTab";
            this.tradeTab.Size = new System.Drawing.Size(833, 242);
            this.tradeTab.TabIndex = 3;
            this.tradeTab.Text = "成交";
            this.tradeTab.UseVisualStyleBackColor = true;
            // 
            // tradeView1
            // 
            this.tradeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeView1.Location = new System.Drawing.Point(0, 0);
            this.tradeView1.Margin = new System.Windows.Forms.Padding(0);
            this.tradeView1.Name = "tradeView1";
            this.tradeView1.Size = new System.Drawing.Size(833, 242);
            this.tradeView1.TabIndex = 0;
            // 
            // posTab
            // 
            this.posTab.Controls.Add(this.positionView1);
            this.posTab.Location = new System.Drawing.Point(4, 21);
            this.posTab.Name = "posTab";
            this.posTab.Size = new System.Drawing.Size(833, 242);
            this.posTab.TabIndex = 1;
            this.posTab.Text = "持仓";
            this.posTab.UseVisualStyleBackColor = true;
            // 
            // positionView1
            // 
            this.positionView1.AutoSize = true;
            this.positionView1.BackColor = System.Drawing.SystemColors.Control;
            this.positionView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionView1.Location = new System.Drawing.Point(0, 0);
            this.positionView1.Name = "positionView1";
            this.positionView1.PositionCheckCentre = null;
            this.positionView1.Size = new System.Drawing.Size(833, 242);
            this.positionView1.TabIndex = 0;
            this.positionView1.TradingTrackerCentre = null;
            // 
            // quoteTab
            // 
            this.quoteTab.Controls.Add(this.quoteView1);
            this.quoteTab.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quoteTab.Location = new System.Drawing.Point(4, 21);
            this.quoteTab.Margin = new System.Windows.Forms.Padding(0);
            this.quoteTab.Name = "quoteTab";
            this.quoteTab.Size = new System.Drawing.Size(833, 242);
            this.quoteTab.TabIndex = 0;
            this.quoteTab.Text = "报价";
            this.quoteTab.UseVisualStyleBackColor = true;
            // 
            // quoteView1
            // 
            this.quoteView1.BackColor = System.Drawing.Color.SlateGray;
            this.quoteView1.DNColor = System.Drawing.Color.Green;
            this.quoteView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quoteView1.HeaderBackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.quoteView1.HeaderFont = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quoteView1.HeaderFontColor = System.Drawing.Color.Turquoise;
            this.quoteView1.Location = new System.Drawing.Point(0, 0);
            this.quoteView1.Name = "quoteView1";
            this.quoteView1.QuoteBackColor1 = System.Drawing.Color.SlateGray;
            this.quoteView1.QuoteBackColor2 = System.Drawing.Color.LightSlateGray;
            this.quoteView1.QuoteFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quoteView1.QuoteViewWidth = 1020;
            this.quoteView1.SelectedQuoteRow = -1;
            this.quoteView1.Size = new System.Drawing.Size(833, 242);
            this.quoteView1.SymbolFont = new System.Drawing.Font("Times New Roman", 12F);
            this.quoteView1.SymbolFontColor = System.Drawing.Color.Green;
            this.quoteView1.TabIndex = 0;
            this.quoteView1.TableLineColor = System.Drawing.Color.Silver;
            this.quoteView1.Text = "viewQuoteList1";
            this.quoteView1.UPColor = System.Drawing.Color.Red;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.quoteTab);
            this.tabControl1.Controls.Add(this.posTab);
            this.tabControl1.Controls.Add(this.tradeTab);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(60, 17);
            this.tabControl1.Location = new System.Drawing.Point(187, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(841, 267);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.orderView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(833, 242);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "委托";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // orderView1
            // 
            this.orderView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderView1.Location = new System.Drawing.Point(0, 0);
            this.orderView1.Margin = new System.Windows.Forms.Padding(0);
            this.orderView1.Name = "orderView1";
            this.orderView1.Size = new System.Drawing.Size(833, 242);
            this.orderView1.TabIndex = 0;
            // 
            // fmOrdEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 269);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.spillTick1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "fmOrdEntry";
            this.Text = "下单中心";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmOrdEntry_FormClosing);
            this.tradeTab.ResumeLayout(false);
            this.posTab.ResumeLayout(false);
            this.posTab.PerformLayout();
            this.quoteTab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TradingLib.GUI.SpillTick spillTick1;
        private System.Windows.Forms.TabPage tradeTab;
        private TradeView tradeView1;
        private System.Windows.Forms.TabPage posTab;
        private PositionView positionView1;
        private System.Windows.Forms.TabPage quoteTab;
        private GDIControl.ViewQuoteList quoteView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private OrderView orderView1;
    }
}