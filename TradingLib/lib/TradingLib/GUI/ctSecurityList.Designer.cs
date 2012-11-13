namespace TradingLib.GUI
{
    partial class ctSecurityList
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
            this.secList = new System.Windows.Forms.DataGridView();
            this.exch = new System.Windows.Forms.ComboBox();
            this.secType = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.secList)).BeginInit();
            this.SuspendLayout();
            // 
            // secList
            // 
            this.secList.AllowUserToAddRows = false;
            this.secList.AllowUserToDeleteRows = false;
            this.secList.AllowUserToResizeRows = false;
            this.secList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.secList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.secList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.secList.Location = new System.Drawing.Point(0, 44);
            this.secList.Name = "secList";
            this.secList.ReadOnly = true;
            this.secList.RowHeadersVisible = false;
            this.secList.RowTemplate.Height = 23;
            this.secList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.secList.Size = new System.Drawing.Size(707, 244);
            this.secList.TabIndex = 0;
            this.secList.SelectionChanged += new System.EventHandler(this.secList_SelectionChanged);
            this.secList.DoubleClick += new System.EventHandler(this.secList_DoubleClick);
            // 
            // exch
            // 
            this.exch.FormattingEnabled = true;
            this.exch.Location = new System.Drawing.Point(112, 14);
            this.exch.Name = "exch";
            this.exch.Size = new System.Drawing.Size(137, 20);
            this.exch.TabIndex = 1;
            this.exch.SelectedIndexChanged += new System.EventHandler(this.exch_SelectedIndexChanged);
            // 
            // secType
            // 
            this.secType.FormattingEnabled = true;
            this.secType.Location = new System.Drawing.Point(0, 14);
            this.secType.Name = "secType";
            this.secType.Size = new System.Drawing.Size(84, 20);
            this.secType.TabIndex = 2;
            this.secType.SelectedIndexChanged += new System.EventHandler(this.secType_SelectedIndexChanged);
            // 
            // ctSecurityList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.secType);
            this.Controls.Add(this.exch);
            this.Controls.Add(this.secList);
            this.Name = "ctSecurityList";
            this.Size = new System.Drawing.Size(707, 288);
            this.Load += new System.EventHandler(this.ctSecurityListControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.secList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView secList;
        private System.Windows.Forms.ComboBox exch;
        private System.Windows.Forms.ComboBox secType;
    }
}
