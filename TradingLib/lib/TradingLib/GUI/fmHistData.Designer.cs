namespace TradingLib.GUI
{
    partial class fmHistData
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabImport = new System.Windows.Forms.TabPage();
            this.tabExport = new System.Windows.Forms.TabPage();
            this.tabEdit = new System.Windows.Forms.TabPage();
            this.ctMSDataSymbolList1 = new TradingLib.GUI.ctMSDataSymbolList();
            this.tabs.SuspendLayout();
            this.tabEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabImport);
            this.tabs.Controls.Add(this.tabExport);
            this.tabs.Controls.Add(this.tabEdit);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(824, 454);
            this.tabs.TabIndex = 0;
            // 
            // tabImport
            // 
            this.tabImport.Location = new System.Drawing.Point(4, 21);
            this.tabImport.Name = "tabImport";
            this.tabImport.Padding = new System.Windows.Forms.Padding(3);
            this.tabImport.Size = new System.Drawing.Size(816, 429);
            this.tabImport.TabIndex = 0;
            this.tabImport.Text = "导入数据";
            this.tabImport.UseVisualStyleBackColor = true;
            // 
            // tabExport
            // 
            this.tabExport.Location = new System.Drawing.Point(4, 21);
            this.tabExport.Name = "tabExport";
            this.tabExport.Padding = new System.Windows.Forms.Padding(3);
            this.tabExport.Size = new System.Drawing.Size(816, 429);
            this.tabExport.TabIndex = 1;
            this.tabExport.Text = "导出数据";
            this.tabExport.UseVisualStyleBackColor = true;
            // 
            // tabEdit
            // 
            this.tabEdit.Controls.Add(this.ctMSDataSymbolList1);
            this.tabEdit.Location = new System.Drawing.Point(4, 21);
            this.tabEdit.Name = "tabEdit";
            this.tabEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabEdit.Size = new System.Drawing.Size(816, 429);
            this.tabEdit.TabIndex = 2;
            this.tabEdit.Text = "编辑";
            this.tabEdit.UseVisualStyleBackColor = true;
            // 
            // ctMSDataSymbolList1
            // 
            this.ctMSDataSymbolList1.Location = new System.Drawing.Point(8, 16);
            this.ctMSDataSymbolList1.Name = "ctMSDataSymbolList1";
            this.ctMSDataSymbolList1.Size = new System.Drawing.Size(525, 342);
            this.ctMSDataSymbolList1.TabIndex = 0;
            // 
            // fmHistData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 454);
            this.Controls.Add(this.tabs);
            this.Name = "fmHistData";
            this.Text = "fmHistData";
            this.tabs.ResumeLayout(false);
            this.tabEdit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabImport;
        private System.Windows.Forms.TabPage tabExport;
        private System.Windows.Forms.TabPage tabEdit;
        private ctMSDataSymbolList ctMSDataSymbolList1;
    }
}