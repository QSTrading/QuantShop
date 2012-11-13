namespace TradingLib.GUI.Server
{
    partial class fmSrvClearCentre
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabOrder = new System.Windows.Forms.TabPage();
            this.tabTrades = new System.Windows.Forms.TabPage();
            this.tabPosition = new System.Windows.Forms.TabPage();
            this.accountlist = new System.Windows.Forms.DataGridView();
            this.orderView1 = new TradingLib.GUI.OrderView();
            this.tradeView1 = new TradingLib.GUI.TradeView();
            this.positionView1 = new TradingLib.GUI.PositionView();
            this.tabs.SuspendLayout();
            this.tabOrder.SuspendLayout();
            this.tabTrades.SuspendLayout();
            this.tabPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountlist)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.tabOrder);
            this.tabs.Controls.Add(this.tabTrades);
            this.tabs.Controls.Add(this.tabPosition);
            this.tabs.Location = new System.Drawing.Point(0, 220);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(938, 434);
            this.tabs.TabIndex = 0;
            // 
            // tabOrder
            // 
            this.tabOrder.Controls.Add(this.orderView1);
            this.tabOrder.Location = new System.Drawing.Point(4, 21);
            this.tabOrder.Name = "tabOrder";
            this.tabOrder.Padding = new System.Windows.Forms.Padding(3);
            this.tabOrder.Size = new System.Drawing.Size(930, 409);
            this.tabOrder.TabIndex = 0;
            this.tabOrder.Text = "委托";
            this.tabOrder.UseVisualStyleBackColor = true;
            // 
            // tabTrades
            // 
            this.tabTrades.Controls.Add(this.tradeView1);
            this.tabTrades.Location = new System.Drawing.Point(4, 21);
            this.tabTrades.Name = "tabTrades";
            this.tabTrades.Padding = new System.Windows.Forms.Padding(3);
            this.tabTrades.Size = new System.Drawing.Size(930, 409);
            this.tabTrades.TabIndex = 1;
            this.tabTrades.Text = "成交";
            this.tabTrades.UseVisualStyleBackColor = true;
            // 
            // tabPosition
            // 
            this.tabPosition.Controls.Add(this.positionView1);
            this.tabPosition.Location = new System.Drawing.Point(4, 21);
            this.tabPosition.Name = "tabPosition";
            this.tabPosition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPosition.Size = new System.Drawing.Size(930, 409);
            this.tabPosition.TabIndex = 2;
            this.tabPosition.Text = "持仓";
            this.tabPosition.UseVisualStyleBackColor = true;
            // 
            // accountlist
            // 
            this.accountlist.AllowUserToAddRows = false;
            this.accountlist.AllowUserToDeleteRows = false;
            this.accountlist.AllowUserToResizeRows = false;
            this.accountlist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.accountlist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.accountlist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accountlist.Location = new System.Drawing.Point(0, 0);
            this.accountlist.MultiSelect = false;
            this.accountlist.Name = "accountlist";
            this.accountlist.RowHeadersVisible = false;
            this.accountlist.RowTemplate.Height = 23;
            this.accountlist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.accountlist.Size = new System.Drawing.Size(938, 220);
            this.accountlist.TabIndex = 1;
            this.accountlist.DoubleClick += new System.EventHandler(this.accountlist_DoubleClick);
            // 
            // orderView1
            // 
            this.orderView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.orderView1.Location = new System.Drawing.Point(-2, 0);
            this.orderView1.Margin = new System.Windows.Forms.Padding(0);
            this.orderView1.Name = "orderView1";
            this.orderView1.Size = new System.Drawing.Size(934, 409);
            this.orderView1.TabIndex = 1;
            // 
            // tradeView1
            // 
            this.tradeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tradeView1.Location = new System.Drawing.Point(0, 0);
            this.tradeView1.Margin = new System.Windows.Forms.Padding(0);
            this.tradeView1.Name = "tradeView1";
            this.tradeView1.Size = new System.Drawing.Size(934, 409);
            this.tradeView1.TabIndex = 0;
            // 
            // positionView1
            // 
            this.positionView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionView1.AutoSize = true;
            this.positionView1.BackColor = System.Drawing.SystemColors.Control;
            this.positionView1.Location = new System.Drawing.Point(0, 0);
            this.positionView1.Name = "positionView1";
            this.positionView1.Size = new System.Drawing.Size(934, 409);
            this.positionView1.TabIndex = 0;
            // 
            // fmSrvClearCentre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 652);
            this.Controls.Add(this.accountlist);
            this.Controls.Add(this.tabs);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "fmSrvClearCentre";
            this.Text = "账户监控";
            this.tabs.ResumeLayout(false);
            this.tabOrder.ResumeLayout(false);
            this.tabTrades.ResumeLayout(false);
            this.tabPosition.ResumeLayout(false);
            this.tabPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accountlist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabOrder;
        private System.Windows.Forms.TabPage tabTrades;
        private System.Windows.Forms.DataGridView accountlist;
        private System.Windows.Forms.TabPage tabPosition;
        private OrderView orderView1;
        private TradeView tradeView1;
        private PositionView positionView1;
    }
}