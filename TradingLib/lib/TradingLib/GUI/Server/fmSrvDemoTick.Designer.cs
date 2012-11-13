namespace TradingLib.GUI.Server
{
    partial class fmSrvDemoTick
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.sym = new System.Windows.Forms.TextBox();
            this.asksize = new System.Windows.Forms.TextBox();
            this.bidsize = new System.Windows.Forms.TextBox();
            this.size = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.askprice = new System.Windows.Forms.NumericUpDown();
            this.bidprice = new System.Windows.Forms.NumericUpDown();
            this.tradeprice = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.askprice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bidprice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradeprice)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ask";
            // 
            // sym
            // 
            this.sym.Location = new System.Drawing.Point(12, 24);
            this.sym.Name = "sym";
            this.sym.Size = new System.Drawing.Size(57, 21);
            this.sym.TabIndex = 1;
            this.sym.Text = "IF1210";
            // 
            // asksize
            // 
            this.asksize.Location = new System.Drawing.Point(187, 24);
            this.asksize.Name = "asksize";
            this.asksize.Size = new System.Drawing.Size(33, 21);
            this.asksize.TabIndex = 4;
            this.asksize.Text = "2";
            // 
            // bidsize
            // 
            this.bidsize.Location = new System.Drawing.Point(187, 51);
            this.bidsize.Name = "bidsize";
            this.bidsize.Size = new System.Drawing.Size(33, 21);
            this.bidsize.TabIndex = 5;
            this.bidsize.Text = "2";
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(330, 23);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(31, 21);
            this.size.TabIndex = 7;
            this.size.Text = "2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Bid";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(122, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "price";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(185, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "size";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(247, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "trade";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(328, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "size";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "symbol";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(385, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "发送";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // askprice
            // 
            this.askprice.DecimalPlaces = 2;
            this.askprice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.askprice.Location = new System.Drawing.Point(106, 24);
            this.askprice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.askprice.Name = "askprice";
            this.askprice.Size = new System.Drawing.Size(75, 21);
            this.askprice.TabIndex = 15;
            this.askprice.Value = new decimal(new int[] {
            2301,
            0,
            0,
            0});
            // 
            // bidprice
            // 
            this.bidprice.DecimalPlaces = 1;
            this.bidprice.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.bidprice.Location = new System.Drawing.Point(106, 51);
            this.bidprice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.bidprice.Name = "bidprice";
            this.bidprice.Size = new System.Drawing.Size(75, 21);
            this.bidprice.TabIndex = 16;
            this.bidprice.Value = new decimal(new int[] {
            2300,
            0,
            0,
            0});
            // 
            // tradeprice
            // 
            this.tradeprice.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.tradeprice.Location = new System.Drawing.Point(249, 24);
            this.tradeprice.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.tradeprice.Name = "tradeprice";
            this.tradeprice.Size = new System.Drawing.Size(75, 21);
            this.tradeprice.TabIndex = 17;
            this.tradeprice.Value = new decimal(new int[] {
            23002,
            0,
            0,
            65536});
            // 
            // fmSrvDemoTick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 79);
            this.Controls.Add(this.tradeprice);
            this.Controls.Add(this.bidprice);
            this.Controls.Add(this.askprice);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.size);
            this.Controls.Add(this.bidsize);
            this.Controls.Add(this.asksize);
            this.Controls.Add(this.sym);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fmSrvDemoTick";
            this.Text = "Tick发送";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.askprice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bidprice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tradeprice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sym;
        private System.Windows.Forms.TextBox asksize;
        private System.Windows.Forms.TextBox bidsize;
        private System.Windows.Forms.TextBox size;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown askprice;
        private System.Windows.Forms.NumericUpDown bidprice;
        private System.Windows.Forms.NumericUpDown tradeprice;
    }
}