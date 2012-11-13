namespace TradingLib.GUI
{
    partial class fmExchEdit
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
            this.label2 = new System.Windows.Forms.Label();
            this.EXIndex = new System.Windows.Forms.Label();
            this.EXCode = new System.Windows.Forms.TextBox();
            this.EXCountry = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.EXName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.oo = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "编码:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "交易所代码:";
            // 
            // EXIndex
            // 
            this.EXIndex.AutoSize = true;
            this.EXIndex.Location = new System.Drawing.Point(69, 24);
            this.EXIndex.Name = "EXIndex";
            this.EXIndex.Size = new System.Drawing.Size(23, 12);
            this.EXIndex.TabIndex = 2;
            this.EXIndex.Text = "n/a";
            // 
            // EXCode
            // 
            this.EXCode.Location = new System.Drawing.Point(105, 44);
            this.EXCode.Name = "EXCode";
            this.EXCode.Size = new System.Drawing.Size(100, 21);
            this.EXCode.TabIndex = 3;
            // 
            // EXCountry
            // 
            this.EXCountry.FormattingEnabled = true;
            this.EXCountry.Location = new System.Drawing.Point(105, 71);
            this.EXCountry.Name = "EXCountry";
            this.EXCountry.Size = new System.Drawing.Size(100, 20);
            this.EXCountry.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "国家:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "交易所名称:";
            // 
            // EXName
            // 
            this.EXName.Location = new System.Drawing.Point(105, 104);
            this.EXName.Name = "EXName";
            this.EXName.Size = new System.Drawing.Size(129, 21);
            this.EXName.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "增加/修改";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // oo
            // 
            this.oo.AutoSize = true;
            this.oo.Location = new System.Drawing.Point(250, 24);
            this.oo.Name = "oo";
            this.oo.Size = new System.Drawing.Size(41, 12);
            this.oo.TabIndex = 9;
            this.oo.Text = "label3";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(51, 144);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "删除";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ExchEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 192);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.oo);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.EXName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EXCountry);
            this.Controls.Add(this.EXCode);
            this.Controls.Add(this.EXIndex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExchEditForm";
            this.ShowIcon = false;
            this.Text = "交易所编辑";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label EXIndex;
        private System.Windows.Forms.TextBox EXCode;
        private System.Windows.Forms.ComboBox EXCountry;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox EXName;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label oo;
        private System.Windows.Forms.Button button2;
    }
}