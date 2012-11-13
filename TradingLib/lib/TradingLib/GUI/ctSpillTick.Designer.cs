namespace TradingLib.GUI
{
    partial class SpillTick
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
            this.butBuy = new System.Windows.Forms.Button();
            this.butSell = new System.Windows.Forms.Button();
            this.size = new System.Windows.Forms.NumericUpDown();
            this.symbol = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.isMarket = new System.Windows.Forms.CheckBox();
            this.isLimit = new System.Windows.Forms.CheckBox();
            this.isStop = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.askPrice = new System.Windows.Forms.Label();
            this.bidPrice = new System.Windows.Forms.Label();
            this.price = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.price)).BeginInit();
            this.SuspendLayout();
            // 
            // butBuy
            // 
            this.butBuy.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.butBuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.butBuy.Location = new System.Drawing.Point(9, 134);
            this.butBuy.Name = "butBuy";
            this.butBuy.Size = new System.Drawing.Size(75, 40);
            this.butBuy.TabIndex = 0;
            this.butBuy.Text = "买入";
            this.butBuy.UseVisualStyleBackColor = true;
            this.butBuy.Click += new System.EventHandler(this.butBuy_Click);
            // 
            // butSell
            // 
            this.butSell.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.butSell.ForeColor = System.Drawing.Color.Teal;
            this.butSell.Location = new System.Drawing.Point(100, 134);
            this.butSell.Name = "butSell";
            this.butSell.Size = new System.Drawing.Size(75, 40);
            this.butSell.TabIndex = 1;
            this.butSell.Text = "卖出";
            this.butSell.UseVisualStyleBackColor = true;
            this.butSell.Click += new System.EventHandler(this.butSell_Click);
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(53, 71);
            this.size.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(120, 21);
            this.size.TabIndex = 3;
            this.size.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // symbol
            // 
            this.symbol.FormattingEnabled = true;
            this.symbol.Location = new System.Drawing.Point(53, 14);
            this.symbol.Name = "symbol";
            this.symbol.Size = new System.Drawing.Size(121, 20);
            this.symbol.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "合约";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "价格";
            // 
            // isMarket
            // 
            this.isMarket.AutoSize = true;
            this.isMarket.Checked = true;
            this.isMarket.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isMarket.Location = new System.Drawing.Point(9, 99);
            this.isMarket.Name = "isMarket";
            this.isMarket.Size = new System.Drawing.Size(48, 16);
            this.isMarket.TabIndex = 7;
            this.isMarket.Text = "市价";
            this.isMarket.UseVisualStyleBackColor = true;
            this.isMarket.CheckedChanged += new System.EventHandler(this.isMarket_CheckedChanged);
            // 
            // isLimit
            // 
            this.isLimit.AutoSize = true;
            this.isLimit.Location = new System.Drawing.Point(71, 99);
            this.isLimit.Name = "isLimit";
            this.isLimit.Size = new System.Drawing.Size(48, 16);
            this.isLimit.TabIndex = 8;
            this.isLimit.Text = "限价";
            this.isLimit.UseVisualStyleBackColor = true;
            this.isLimit.CheckedChanged += new System.EventHandler(this.isLimit_CheckedChanged);
            // 
            // isStop
            // 
            this.isStop.AutoSize = true;
            this.isStop.Location = new System.Drawing.Point(127, 99);
            this.isStop.Name = "isStop";
            this.isStop.Size = new System.Drawing.Size(48, 16);
            this.isStop.TabIndex = 9;
            this.isStop.Text = "追价";
            this.isStop.UseVisualStyleBackColor = true;
            this.isStop.CheckedChanged += new System.EventHandler(this.isStop_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "数量";
            // 
            // askPrice
            // 
            this.askPrice.AutoSize = true;
            this.askPrice.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.askPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.askPrice.Location = new System.Drawing.Point(12, 115);
            this.askPrice.Name = "askPrice";
            this.askPrice.Size = new System.Drawing.Size(16, 16);
            this.askPrice.TabIndex = 11;
            this.askPrice.Text = "0";
            // 
            // bidPrice
            // 
            this.bidPrice.AutoSize = true;
            this.bidPrice.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bidPrice.ForeColor = System.Drawing.Color.Teal;
            this.bidPrice.Location = new System.Drawing.Point(103, 115);
            this.bidPrice.Name = "bidPrice";
            this.bidPrice.Size = new System.Drawing.Size(16, 16);
            this.bidPrice.TabIndex = 12;
            this.bidPrice.Text = "0";
            // 
            // price
            // 
            this.price.DecimalPlaces = 2;
            this.price.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.price.Location = new System.Drawing.Point(53, 41);
            this.price.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(120, 21);
            this.price.TabIndex = 13;
            this.price.DoubleClick += new System.EventHandler(this.price_DoubleClick);
            this.price.KeyUp += new System.Windows.Forms.KeyEventHandler(this.price_KeyUp);
            // 
            // SpillTick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.price);
            this.Controls.Add(this.bidPrice);
            this.Controls.Add(this.askPrice);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.isStop);
            this.Controls.Add(this.isLimit);
            this.Controls.Add(this.isMarket);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.symbol);
            this.Controls.Add(this.size);
            this.Controls.Add(this.butSell);
            this.Controls.Add(this.butBuy);
            this.Name = "SpillTick";
            this.Size = new System.Drawing.Size(205, 180);
            ((System.ComponentModel.ISupportInitialize)(this.size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.price)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butBuy;
        private System.Windows.Forms.Button butSell;
        private System.Windows.Forms.NumericUpDown size;
        private System.Windows.Forms.ComboBox symbol;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox isMarket;
        private System.Windows.Forms.CheckBox isLimit;
        private System.Windows.Forms.CheckBox isStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label askPrice;
        private System.Windows.Forms.Label bidPrice;
        private System.Windows.Forms.NumericUpDown price;
    }
}
