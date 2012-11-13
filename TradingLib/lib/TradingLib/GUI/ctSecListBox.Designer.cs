namespace TradingLib.GUI
{
    partial class ctSecListBox
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
            this.basket = new System.Windows.Forms.ComboBox();
            this.lb = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // basket
            // 
            this.basket.FormattingEnabled = true;
            this.basket.Location = new System.Drawing.Point(3, 3);
            this.basket.Name = "basket";
            this.basket.Size = new System.Drawing.Size(149, 20);
            this.basket.TabIndex = 0;
            this.basket.SelectedValueChanged += new System.EventHandler(this.basket_SelectedValueChanged);
            // 
            // lb
            // 
            this.lb.FormattingEnabled = true;
            this.lb.ItemHeight = 12;
            this.lb.Location = new System.Drawing.Point(3, 29);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(149, 352);
            this.lb.TabIndex = 1;
            this.lb.SelectedValueChanged += new System.EventHandler(this.lb_SelectedValueChanged);
            // 
            // ctSecListBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb);
            this.Controls.Add(this.basket);
            this.Name = "ctSecListBox";
            this.Size = new System.Drawing.Size(155, 386);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox basket;
        private System.Windows.Forms.ListBox lb;
    }
}
