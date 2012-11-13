namespace TradingLib.GUI
{
    partial class ctSecTreeView
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
            this.SecLists = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // SecLists
            // 
            this.SecLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.SecLists.Location = new System.Drawing.Point(3, 3);
            this.SecLists.Name = "SecLists";
            this.SecLists.Size = new System.Drawing.Size(121, 344);
            this.SecLists.TabIndex = 0;
            // 
            // ctSecLists
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SecLists);
            this.Name = "ctSecLists";
            this.Size = new System.Drawing.Size(127, 351);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView SecLists;
    }
}
