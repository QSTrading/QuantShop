namespace TradingLib.GUI
{
    partial class ctExchangeList
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
            this.exchList = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.exchList)).BeginInit();
            this.SuspendLayout();
            // 
            // exchList
            // 
            this.exchList.AllowUserToAddRows = false;
            this.exchList.AllowUserToDeleteRows = false;
            this.exchList.AllowUserToResizeRows = false;
            this.exchList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.exchList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.exchList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.exchList.Location = new System.Drawing.Point(0, 0);
            this.exchList.MultiSelect = false;
            this.exchList.Name = "exchList";
            this.exchList.ReadOnly = true;
            this.exchList.RowHeadersVisible = false;
            this.exchList.RowTemplate.Height = 23;
            this.exchList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.exchList.Size = new System.Drawing.Size(483, 202);
            this.exchList.TabIndex = 0;
            this.exchList.DoubleClick += new System.EventHandler(this.exchList_DoubleClick);
            // 
            // ctExchangeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.exchList);
            this.Name = "ctExchangeList";
            this.Size = new System.Drawing.Size(483, 202);
            this.Load += new System.EventHandler(this.ctExchangeListControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.exchList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView exchList;
    }
}
