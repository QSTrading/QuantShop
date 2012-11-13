namespace TradingLib.GUI
{
    partial class PositionView
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
            this.positionGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // positionGrid
            // 
            this.positionGrid.AllowUserToAddRows = false;
            this.positionGrid.AllowUserToDeleteRows = false;
            this.positionGrid.AllowUserToResizeRows = false;
            this.positionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.positionGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.positionGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.positionGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.positionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.positionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionGrid.Location = new System.Drawing.Point(0, 0);
            this.positionGrid.Name = "positionGrid";
            this.positionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.positionGrid.RowHeadersVisible = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.positionGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.positionGrid.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.Silver;
            this.positionGrid.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.positionGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.positionGrid.RowTemplate.Height = 23;
            this.positionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.positionGrid.Size = new System.Drawing.Size(700, 250);
            this.positionGrid.TabIndex = 0;
            this.positionGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.positionGrid_CellMouseClick);
            // 
            // PositionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.positionGrid);
            this.Name = "PositionView";
            this.Size = new System.Drawing.Size(700, 250);
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView positionGrid;
    }
}
