namespace TradingLib.GUI
{
    partial class fmSecListEdit
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.addBasket = new System.Windows.Forms.Button();
            this.dellBasket = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.expire = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.margin = new System.Windows.Forms.NumericUpDown();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.ctSecListBox = new TradingLib.GUI.ctSecListBox();
            this.ctSecurityList1 = new TradingLib.GUI.ctSecurityList();
            ((System.ComponentModel.ISupportInitialize)(this.margin)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(169, 414);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(57, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "<--";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // addBasket
            // 
            this.addBasket.Location = new System.Drawing.Point(9, 396);
            this.addBasket.Name = "addBasket";
            this.addBasket.Size = new System.Drawing.Size(65, 23);
            this.addBasket.TabIndex = 4;
            this.addBasket.Text = "增加列表";
            this.addBasket.UseVisualStyleBackColor = true;
            this.addBasket.Click += new System.EventHandler(this.addBasket_Click);
            // 
            // dellBasket
            // 
            this.dellBasket.Location = new System.Drawing.Point(83, 396);
            this.dellBasket.Name = "dellBasket";
            this.dellBasket.Size = new System.Drawing.Size(62, 23);
            this.dellBasket.TabIndex = 5;
            this.dellBasket.Text = "删除列表";
            this.dellBasket.UseVisualStyleBackColor = true;
            this.dellBasket.Click += new System.EventHandler(this.dellBasket_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(169, 385);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(57, 23);
            this.btnDel.TabIndex = 6;
            this.btnDel.Text = "-->";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(249, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "交割月";
            // 
            // expire
            // 
            this.expire.FormattingEnabled = true;
            this.expire.Location = new System.Drawing.Point(296, 389);
            this.expire.Name = "expire";
            this.expire.Size = new System.Drawing.Size(88, 20);
            this.expire.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(405, 395);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "保证金";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(410, 431);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 11;
            // 
            // margin
            // 
            this.margin.DecimalPlaces = 2;
            this.margin.Location = new System.Drawing.Point(452, 390);
            this.margin.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.margin.Name = "margin";
            this.margin.Size = new System.Drawing.Size(88, 21);
            this.margin.TabIndex = 12;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(563, 393);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(60, 16);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "可交易";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // ctSecListBox
            // 
            this.ctSecListBox.Location = new System.Drawing.Point(1, 4);
            this.ctSecListBox.Name = "ctSecListBox";
            this.ctSecListBox.SelectedBasket = "";
            this.ctSecListBox.Size = new System.Drawing.Size(155, 386);
            this.ctSecListBox.TabIndex = 2;
            // 
            // ctSecurityList1
            // 
            this.ctSecurityList1.Location = new System.Drawing.Point(169, -6);
            this.ctSecurityList1.Name = "ctSecurityList1";
            this.ctSecurityList1.Size = new System.Drawing.Size(630, 385);
            this.ctSecurityList1.TabIndex = 1;
            // 
            // fmSecListEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 452);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.margin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.expire);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.dellBasket);
            this.Controls.Add(this.addBasket);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.ctSecListBox);
            this.Controls.Add(this.ctSecurityList1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmSecListEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "合约列表编辑";
            ((System.ComponentModel.ISupportInitialize)(this.margin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ctSecurityList ctSecurityList1;
        private ctSecListBox ctSecListBox;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button addBasket;
        private System.Windows.Forms.Button dellBasket;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox expire;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown margin;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}