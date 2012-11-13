namespace Strategy.GUI
{
    partial class fmStopTrailing
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._stoploss = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this._favor = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this._adverse = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this._closedpl = new System.Windows.Forms.TextBox();
            this._pos = new System.Windows.Forms.TextBox();
            this._ask = new System.Windows.Forms.TextBox();
            this._bid = new System.Windows.Forms.TextBox();
            this._last = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this._osize = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this._breakEven = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this._avgcost = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this._unrealizedpl = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this._profitTakestop = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ctTimesLineChart1 = new TradingLib.GUI.ctTimesLineChart();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.debugControl1 = new TradeLink.AppKit.DebugControl();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._osize)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "最新";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "买入";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "卖出";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(181, 1);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "仓位";
            // 
            // _stoploss
            // 
            this._stoploss.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._stoploss.Location = new System.Drawing.Point(1, 90);
            this._stoploss.Margin = new System.Windows.Forms.Padding(2);
            this._stoploss.Name = "_stoploss";
            this._stoploss.ReadOnly = true;
            this._stoploss.Size = new System.Drawing.Size(50, 21);
            this._stoploss.TabIndex = 13;
            this._stoploss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(-1, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "止损";
            // 
            // _favor
            // 
            this._favor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._favor.Location = new System.Drawing.Point(61, 53);
            this._favor.Margin = new System.Windows.Forms.Padding(2);
            this._favor.Name = "_favor";
            this._favor.ReadOnly = true;
            this._favor.Size = new System.Drawing.Size(50, 21);
            this._favor.TabIndex = 15;
            this._favor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(59, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "最优价";
            // 
            // _adverse
            // 
            this._adverse.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._adverse.Location = new System.Drawing.Point(121, 53);
            this._adverse.Margin = new System.Windows.Forms.Padding(2);
            this._adverse.Name = "_adverse";
            this._adverse.ReadOnly = true;
            this._adverse.Size = new System.Drawing.Size(50, 21);
            this._adverse.TabIndex = 17;
            this._adverse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(121, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "最差价";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this._closedpl);
            this.panel1.Controls.Add(this._pos);
            this.panel1.Controls.Add(this._ask);
            this.panel1.Controls.Add(this._bid);
            this.panel1.Controls.Add(this._last);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this._osize);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this._breakEven);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this._avgcost);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this._unrealizedpl);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this._profitTakestop);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this._adverse);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this._favor);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this._stoploss);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 116);
            this.panel1.TabIndex = 4;
            this.panel1.DoubleClick += new System.EventHandler(this.panel1_DoubleClick);
            // 
            // _closedpl
            // 
            this._closedpl.BackColor = System.Drawing.SystemColors.Control;
            this._closedpl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._closedpl.Location = new System.Drawing.Point(241, 15);
            this._closedpl.Name = "_closedpl";
            this._closedpl.Size = new System.Drawing.Size(50, 21);
            this._closedpl.TabIndex = 34;
            this._closedpl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _pos
            // 
            this._pos.BackColor = System.Drawing.SystemColors.Control;
            this._pos.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._pos.Location = new System.Drawing.Point(181, 15);
            this._pos.Name = "_pos";
            this._pos.Size = new System.Drawing.Size(50, 21);
            this._pos.TabIndex = 33;
            this._pos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _ask
            // 
            this._ask.BackColor = System.Drawing.SystemColors.Control;
            this._ask.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._ask.Location = new System.Drawing.Point(121, 15);
            this._ask.Name = "_ask";
            this._ask.Size = new System.Drawing.Size(50, 21);
            this._ask.TabIndex = 32;
            this._ask.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _bid
            // 
            this._bid.BackColor = System.Drawing.SystemColors.Control;
            this._bid.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._bid.Location = new System.Drawing.Point(61, 15);
            this._bid.Name = "_bid";
            this._bid.Size = new System.Drawing.Size(50, 21);
            this._bid.TabIndex = 31;
            this._bid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // _last
            // 
            this._last.BackColor = System.Drawing.SystemColors.Control;
            this._last.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._last.Location = new System.Drawing.Point(1, 15);
            this._last.Name = "_last";
            this._last.Size = new System.Drawing.Size(50, 21);
            this._last.TabIndex = 30;
            this._last.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(241, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 29;
            this.label13.Text = "手数";
            // 
            // _osize
            // 
            this._osize.Location = new System.Drawing.Point(243, 53);
            this._osize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this._osize.Name = "_osize";
            this._osize.Size = new System.Drawing.Size(50, 21);
            this._osize.TabIndex = 28;
            this._osize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(241, 1);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 26;
            this.label12.Text = "总获利";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(58, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 24;
            this.label10.Text = "保本";
            // 
            // _breakEven
            // 
            this._breakEven.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._breakEven.Location = new System.Drawing.Point(60, 90);
            this._breakEven.Margin = new System.Windows.Forms.Padding(2);
            this._breakEven.Name = "_breakEven";
            this._breakEven.ReadOnly = true;
            this._breakEven.Size = new System.Drawing.Size(50, 21);
            this._breakEven.TabIndex = 25;
            this._breakEven.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 22;
            this.label9.Text = "成本";
            // 
            // _avgcost
            // 
            this._avgcost.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._avgcost.Location = new System.Drawing.Point(1, 53);
            this._avgcost.Margin = new System.Windows.Forms.Padding(2);
            this._avgcost.Name = "_avgcost";
            this._avgcost.ReadOnly = true;
            this._avgcost.Size = new System.Drawing.Size(50, 21);
            this._avgcost.TabIndex = 23;
            this._avgcost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(181, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "浮盈";
            // 
            // _unrealizedpl
            // 
            this._unrealizedpl.BackColor = System.Drawing.SystemColors.Control;
            this._unrealizedpl.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._unrealizedpl.ForeColor = System.Drawing.Color.Black;
            this._unrealizedpl.Location = new System.Drawing.Point(181, 53);
            this._unrealizedpl.Margin = new System.Windows.Forms.Padding(2);
            this._unrealizedpl.Name = "_unrealizedpl";
            this._unrealizedpl.ReadOnly = true;
            this._unrealizedpl.Size = new System.Drawing.Size(50, 21);
            this._unrealizedpl.TabIndex = 21;
            this._unrealizedpl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(121, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 18;
            this.label11.Text = "回调止盈";
            // 
            // _profitTakestop
            // 
            this._profitTakestop.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this._profitTakestop.Location = new System.Drawing.Point(121, 90);
            this._profitTakestop.Margin = new System.Windows.Forms.Padding(2);
            this._profitTakestop.Name = "_profitTakestop";
            this._profitTakestop.ReadOnly = true;
            this._profitTakestop.Size = new System.Drawing.Size(115, 21);
            this._profitTakestop.TabIndex = 19;
            this._profitTakestop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(245, 415);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(58, 22);
            this.button1.TabIndex = 5;
            this.button1.Text = "平仓";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 115);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(305, 300);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ctTimesLineChart1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(297, 275);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "分时图";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ctTimesLineChart1
            // 
            this.ctTimesLineChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctTimesLineChart1.Location = new System.Drawing.Point(3, 3);
            this.ctTimesLineChart1.Name = "ctTimesLineChart1";
            this.ctTimesLineChart1.Size = new System.Drawing.Size(291, 269);
            this.ctTimesLineChart1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.debugControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(297, 275);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // debugControl1
            // 
            this.debugControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.debugControl1.EnableSearching = true;
            this.debugControl1.ExternalTimeStamp = 0;
            this.debugControl1.Location = new System.Drawing.Point(3, 3);
            this.debugControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.debugControl1.Name = "debugControl1";
            this.debugControl1.Size = new System.Drawing.Size(291, 269);
            this.debugControl1.TabIndex = 0;
            this.debugControl1.TimeStamps = true;
            this.debugControl1.UseExternalTimeStamp = false;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.DarkRed;
            this.button2.Location = new System.Drawing.Point(187, 414);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(58, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "卖";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.DarkGreen;
            this.button3.Location = new System.Drawing.Point(129, 414);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "买";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.BackColor = System.Drawing.Color.DarkOrange;
            this.button4.Location = new System.Drawing.Point(71, 414);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(58, 23);
            this.button4.TabIndex = 9;
            this.button4.Text = "反手";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // fmStopTrailing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 438);
            this.ControlBox = false;
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmStopTrailing";
            this.Text = "动态止损止盈";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmStopTrailing_FormClosing);
            this.Load += new System.EventHandler(this.fmStopTrailing_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._osize)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _stoploss;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _favor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _adverse;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox _profitTakestop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox _unrealizedpl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox _avgcost;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox _breakEven;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private TradeLink.AppKit.DebugControl debugControl1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown _osize;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private TradingLib.GUI.ctTimesLineChart ctTimesLineChart1;
        private System.Windows.Forms.TextBox _last;
        private System.Windows.Forms.TextBox _bid;
        private System.Windows.Forms.TextBox _ask;
        private System.Windows.Forms.TextBox _pos;
        private System.Windows.Forms.TextBox _closedpl;
        private System.Windows.Forms.Button button4;

    }
}