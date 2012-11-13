namespace TradingLib.GUI
{
    partial class TradeView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tradeGrid = new System.Windows.Forms.DataGridView();
            this.tradeid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.tradeGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tradeGrid
            // 
            this.tradeGrid.AllowUserToAddRows = false;
            this.tradeGrid.AllowUserToDeleteRows = false;
            this.tradeGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tradeGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.tradeGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tradeGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.tradeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tradeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tradeid,
            this.symbol,
            this.side,
            this.xSize,
            this.xPrice,
            this.xTime,
            this.account});
            this.tradeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeGrid.Location = new System.Drawing.Point(0, 0);
            this.tradeGrid.Name = "tradeGrid";
            this.tradeGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.tradeGrid.RowHeadersVisible = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tradeGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.tradeGrid.RowTemplate.Height = 23;
            this.tradeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tradeGrid.Size = new System.Drawing.Size(700, 250);
            this.tradeGrid.TabIndex = 1;
            // 
            // tradeid
            // 
            this.tradeid.HeaderText = "成交编号";
            this.tradeid.Name = "tradeid";
            // 
            // symbol
            // 
            this.symbol.HeaderText = "合约";
            this.symbol.Name = "symbol";
            // 
            // side
            // 
            this.side.HeaderText = "方向";
            this.side.Name = "side";
            // 
            // xSize
            // 
            this.xSize.HeaderText = "数量";
            this.xSize.Name = "xSize";
            // 
            // xPrice
            // 
            this.xPrice.HeaderText = "价格";
            this.xPrice.Name = "xPrice";
            // 
            // xTime
            // 
            this.xTime.HeaderText = "时间";
            this.xTime.Name = "xTime";
            // 
            // account
            // 
            this.account.HeaderText = "账户";
            this.account.Name = "account";
            // 
            // TradeView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tradeGrid);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TradeView";
            this.Size = new System.Drawing.Size(700, 250);
            ((System.ComponentModel.ISupportInitialize)(this.tradeGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView tradeGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn tradeid;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn side;
        private System.Windows.Forms.DataGridViewTextBoxColumn xSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn xPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn xTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn account;
    }
}
