namespace TradingLib.GUI
{
    partial class OrderView
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
            this.orderGrid = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.side = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.limit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cancelled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.orderGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // orderGrid
            // 
            this.orderGrid.AllowUserToAddRows = false;
            this.orderGrid.AllowUserToDeleteRows = false;
            this.orderGrid.AllowUserToResizeRows = false;
            this.orderGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.orderGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.orderGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.orderGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.orderGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.orderGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.oid,
            this.symbol,
            this.side,
            this.orderType,
            this.size,
            this.filled,
            this.limit,
            this.stop,
            this.tif,
            this.cancelled});
            this.orderGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderGrid.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.orderGrid.Location = new System.Drawing.Point(0, 0);
            this.orderGrid.Margin = new System.Windows.Forms.Padding(0);
            this.orderGrid.Name = "orderGrid";
            this.orderGrid.RowHeadersVisible = false;
            this.orderGrid.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Silver;
            this.orderGrid.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderGrid.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.orderGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.orderGrid.RowTemplate.Height = 23;
            this.orderGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.orderGrid.Size = new System.Drawing.Size(700, 250);
            this.orderGrid.TabIndex = 0;
            this.orderGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.orderGrid_CellMouseDoubleClick);
            // 
            // time
            // 
            this.time.HeaderText = "时间";
            this.time.Name = "time";
            // 
            // oid
            // 
            this.oid.HeaderText = "Order编号";
            this.oid.Name = "oid";
            // 
            // symbol
            // 
            this.symbol.HeaderText = "合约";
            this.symbol.Name = "symbol";
            // 
            // side
            // 
            this.side.HeaderText = "买/卖";
            this.side.Name = "side";
            // 
            // orderType
            // 
            this.orderType.HeaderText = "委托类型";
            this.orderType.Name = "orderType";
            // 
            // size
            // 
            this.size.HeaderText = "数量";
            this.size.Name = "size";
            // 
            // filled
            // 
            this.filled.HeaderText = "已成交";
            this.filled.Name = "filled";
            // 
            // limit
            // 
            this.limit.HeaderText = "限价";
            this.limit.Name = "limit";
            // 
            // stop
            // 
            this.stop.HeaderText = "触发价";
            this.stop.Name = "stop";
            // 
            // tif
            // 
            this.tif.HeaderText = "TIF类型";
            this.tif.Name = "tif";
            // 
            // cancelled
            // 
            this.cancelled.HeaderText = "已取消";
            this.cancelled.Name = "cancelled";
            // 
            // OrderView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.orderGrid);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "OrderView";
            this.Size = new System.Drawing.Size(700, 250);
            ((System.ComponentModel.ISupportInitialize)(this.orderGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView orderGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderId;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn oid;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn side;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderType;
        private System.Windows.Forms.DataGridViewTextBoxColumn size;
        private System.Windows.Forms.DataGridViewTextBoxColumn filled;
        private System.Windows.Forms.DataGridViewTextBoxColumn limit;
        private System.Windows.Forms.DataGridViewTextBoxColumn stop;
        private System.Windows.Forms.DataGridViewTextBoxColumn tif;
        private System.Windows.Forms.DataGridViewTextBoxColumn cancelled;

    }
}
