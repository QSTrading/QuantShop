using TradingLib.GUI;
namespace TradingLib
{
    partial class OrderForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.quoteTab = new System.Windows.Forms.TabPage();
            this.posTab = new System.Windows.Forms.TabPage();
            this.orderTab = new System.Windows.Forms.TabPage();
            this.tradeTab = new System.Windows.Forms.TabPage();
            this.spillTick1 = new TradingLib.GUI.SpillTick();
            this.quoteView1 = new TradingLib.GUI.QuoteView();
            this.positionView1 = new TradingLib.GUI.PositionView();
            this.orderView1 = new TradingLib.GUI.OrderView();
            this.tradeView1 = new TradingLib.GUI.TradeView();
            this.tabControl1.SuspendLayout();
            this.quoteTab.SuspendLayout();
            this.posTab.SuspendLayout();
            this.orderTab.SuspendLayout();
            this.tradeTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.quoteTab);
            this.tabControl1.Controls.Add(this.posTab);
            this.tabControl1.Controls.Add(this.orderTab);
            this.tabControl1.Controls.Add(this.tradeTab);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(700, 300);
            this.tabControl1.TabIndex = 0;
            // 
            // quoteTab
            // 
            this.quoteTab.Controls.Add(this.quoteView1);
            this.quoteTab.Location = new System.Drawing.Point(4, 21);
            this.quoteTab.Name = "quoteTab";
            this.quoteTab.Padding = new System.Windows.Forms.Padding(3);
            this.quoteTab.Size = new System.Drawing.Size(692, 275);
            this.quoteTab.TabIndex = 0;
            this.quoteTab.Text = "报价";
            this.quoteTab.UseVisualStyleBackColor = true;
            // 
            // posTab
            // 
            this.posTab.Controls.Add(this.positionView1);
            this.posTab.Location = new System.Drawing.Point(4, 21);
            this.posTab.Name = "posTab";
            this.posTab.Padding = new System.Windows.Forms.Padding(3);
            this.posTab.Size = new System.Drawing.Size(692, 275);
            this.posTab.TabIndex = 1;
            this.posTab.Text = "持仓";
            this.posTab.UseVisualStyleBackColor = true;
            // 
            // orderTab
            // 
            this.orderTab.Controls.Add(this.orderView1);
            this.orderTab.Location = new System.Drawing.Point(4, 21);
            this.orderTab.Name = "orderTab";
            this.orderTab.Padding = new System.Windows.Forms.Padding(3);
            this.orderTab.Size = new System.Drawing.Size(692, 275);
            this.orderTab.TabIndex = 2;
            this.orderTab.Text = "委托";
            this.orderTab.UseVisualStyleBackColor = true;
            // 
            // tradeTab
            // 
            this.tradeTab.Controls.Add(this.tradeView1);
            this.tradeTab.Location = new System.Drawing.Point(4, 21);
            this.tradeTab.Name = "tradeTab";
            this.tradeTab.Padding = new System.Windows.Forms.Padding(3);
            this.tradeTab.Size = new System.Drawing.Size(692, 275);
            this.tradeTab.TabIndex = 3;
            this.tradeTab.Text = "成交";
            this.tradeTab.UseVisualStyleBackColor = true;
            // 
            // spillTick1
            // 
            this.spillTick1.Location = new System.Drawing.Point(710, 21);
            this.spillTick1.Name = "spillTick1";
            this.spillTick1.Size = new System.Drawing.Size(300, 250);
            this.spillTick1.TabIndex = 1;
            // 
            // quoteView1
            // 
            this.quoteView1.Location = new System.Drawing.Point(0, 0);
            this.quoteView1.Name = "quoteView1";
            this.quoteView1.Size = new System.Drawing.Size(700, 250);
            this.quoteView1.TabIndex = 0;
            // 
            // positionView1
            // 
            this.positionView1.BackColor = System.Drawing.SystemColors.Control;
            this.positionView1.Location = new System.Drawing.Point(0, 0);
            this.positionView1.Name = "positionView1";
            this.positionView1.Size = new System.Drawing.Size(700, 250);
            this.positionView1.TabIndex = 0;
            // 
            // orderView1
            // 
            //this.orderView1.BrokerFeed = null;
            this.orderView1.Location = new System.Drawing.Point(0, 0);
            this.orderView1.Name = "orderView1";
            this.orderView1.Size = new System.Drawing.Size(700, 250);
            this.orderView1.TabIndex = 0;
            // 
            // tradeView1
            // 
            this.tradeView1.Location = new System.Drawing.Point(0, 0);
            this.tradeView1.Name = "tradeView1";
            this.tradeView1.Size = new System.Drawing.Size(700, 250);
            this.tradeView1.TabIndex = 0;
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 316);
            this.Controls.Add(this.spillTick1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderForm";
            this.Text = "OrderForm";
            this.tabControl1.ResumeLayout(false);
            this.quoteTab.ResumeLayout(false);
            this.posTab.ResumeLayout(false);
            this.orderTab.ResumeLayout(false);
            this.tradeTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage quoteTab;
        private System.Windows.Forms.TabPage posTab;
        private QuoteView quoteView1;
        
        private PositionView positionView1;
        private System.Windows.Forms.TabPage orderTab;
        private OrderView orderView1;
        private System.Windows.Forms.TabPage tradeTab;
        private TradeView tradeView1;
        private SpillTick spillTick1;
    }
}