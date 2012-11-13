namespace TradingLib.GUI
{
    partial class fmSecEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.SECSymbol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SECMargin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SECName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SECSession = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SECType = new System.Windows.Forms.ComboBox();
            this.SECExchange = new System.Windows.Forms.CheckedListBox();
            this.SECCurrency = new System.Windows.Forms.ComboBox();
            this.SECMultiple = new System.Windows.Forms.NumericUpDown();
            this.SECPriceTick = new System.Windows.Forms.ComboBox();
            this.ok = new System.Windows.Forms.Button();
            this.cancle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SECMultiple)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Symbol";
            // 
            // SECSymbol
            // 
            this.SECSymbol.Location = new System.Drawing.Point(12, 33);
            this.SECSymbol.Name = "SECSymbol";
            this.SECSymbol.Size = new System.Drawing.Size(100, 21);
            this.SECSymbol.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "类型";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "跳";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "乘数";
            // 
            // SECMargin
            // 
            this.SECMargin.Location = new System.Drawing.Point(144, 138);
            this.SECMargin.Name = "SECMargin";
            this.SECMargin.Size = new System.Drawing.Size(100, 21);
            this.SECMargin.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "保证金";
            // 
            // SECName
            // 
            this.SECName.Location = new System.Drawing.Point(12, 196);
            this.SECName.Name = "SECName";
            this.SECName.Size = new System.Drawing.Size(232, 21);
            this.SECName.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "名称";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "交易时段";
            // 
            // SECSession
            // 
            this.SECSession.FormattingEnabled = true;
            this.SECSession.Location = new System.Drawing.Point(12, 242);
            this.SECSession.Name = "SECSession";
            this.SECSession.Size = new System.Drawing.Size(232, 20);
            this.SECSession.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(142, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "货币";
            // 
            // SECType
            // 
            this.SECType.FormattingEnabled = true;
            this.SECType.Location = new System.Drawing.Point(144, 34);
            this.SECType.Name = "SECType";
            this.SECType.Size = new System.Drawing.Size(100, 20);
            this.SECType.TabIndex = 16;
            // 
            // SECExchange
            // 
            this.SECExchange.CheckOnClick = true;
            this.SECExchange.FormattingEnabled = true;
            this.SECExchange.Location = new System.Drawing.Point(265, 18);
            this.SECExchange.Name = "SECExchange";
            this.SECExchange.Size = new System.Drawing.Size(169, 244);
            this.SECExchange.TabIndex = 17;
            this.SECExchange.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.SECExchange_ItemCheck);
            // 
            // SECCurrency
            // 
            this.SECCurrency.FormattingEnabled = true;
            this.SECCurrency.Location = new System.Drawing.Point(144, 88);
            this.SECCurrency.Name = "SECCurrency";
            this.SECCurrency.Size = new System.Drawing.Size(100, 20);
            this.SECCurrency.TabIndex = 18;
            // 
            // SECMultiple
            // 
            this.SECMultiple.Location = new System.Drawing.Point(12, 139);
            this.SECMultiple.Name = "SECMultiple";
            this.SECMultiple.Size = new System.Drawing.Size(100, 21);
            this.SECMultiple.TabIndex = 21;
            // 
            // SECPriceTick
            // 
            this.SECPriceTick.FormattingEnabled = true;
            this.SECPriceTick.Location = new System.Drawing.Point(12, 87);
            this.SECPriceTick.Name = "SECPriceTick";
            this.SECPriceTick.Size = new System.Drawing.Size(100, 20);
            this.SECPriceTick.TabIndex = 22;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(181, 288);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 23;
            this.ok.Text = "确认";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.ok_Click);
            // 
            // cancle
            // 
            this.cancle.Location = new System.Drawing.Point(328, 288);
            this.cancle.Name = "cancle";
            this.cancle.Size = new System.Drawing.Size(75, 23);
            this.cancle.TabIndex = 24;
            this.cancle.Text = "取消";
            this.cancle.UseVisualStyleBackColor = true;
            this.cancle.Click += new System.EventHandler(this.cancle_Click);
            // 
            // SecEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 323);
            this.Controls.Add(this.cancle);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.SECPriceTick);
            this.Controls.Add(this.SECMultiple);
            this.Controls.Add(this.SECCurrency);
            this.Controls.Add(this.SECExchange);
            this.Controls.Add(this.SECType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.SECSession);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.SECName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SECMargin);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SECSymbol);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecEditForm";
            this.Text = "SecEditForm";
            ((System.ComponentModel.ISupportInitialize)(this.SECMultiple)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SECSymbol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SECMargin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox SECName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox SECSession;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox SECType;
        private System.Windows.Forms.CheckedListBox SECExchange;
        private System.Windows.Forms.ComboBox SECCurrency;
        private System.Windows.Forms.NumericUpDown SECMultiple;
        private System.Windows.Forms.ComboBox SECPriceTick;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancle;
    }
}