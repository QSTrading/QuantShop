namespace Strategy.GUI
{
    partial class fmTargetProfit
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
            this._last = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._targetprice = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._targetProfit = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this._size = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._targetProfit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._size)).BeginInit();
            this.SuspendLayout();
            // 
            // _last
            // 
            this._last.BackColor = System.Drawing.SystemColors.Control;
            this._last.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._last.Location = new System.Drawing.Point(1, 16);
            this._last.Name = "_last";
            this._last.Size = new System.Drawing.Size(50, 21);
            this._last.TabIndex = 32;
            this._last.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "最新";
            // 
            // _targetprice
            // 
            this._targetprice.BackColor = System.Drawing.SystemColors.Control;
            this._targetprice.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._targetprice.Location = new System.Drawing.Point(73, 16);
            this._targetprice.Name = "_targetprice";
            this._targetprice.Size = new System.Drawing.Size(50, 21);
            this._targetprice.TabIndex = 34;
            this._targetprice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(71, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 33;
            this.label2.Text = "目标价格";
            // 
            // _targetProfit
            // 
            this._targetProfit.DecimalPlaces = 1;
            this._targetProfit.Increment = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this._targetProfit.Location = new System.Drawing.Point(145, 16);
            this._targetProfit.Name = "_targetProfit";
            this._targetProfit.Size = new System.Drawing.Size(64, 21);
            this._targetProfit.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(143, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 36;
            this.label3.Text = "目标利润";
            // 
            // _size
            // 
            this._size.Location = new System.Drawing.Point(228, 16);
            this._size.Name = "_size";
            this._size.Size = new System.Drawing.Size(39, 21);
            this._size.TabIndex = 37;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(226, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 38;
            this.label4.Text = "手数";
            // 
            // fmTargetProfit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 42);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this._size);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._targetProfit);
            this.Controls.Add(this._targetprice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._last);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmTargetProfit";
            this.Text = "fmTargetProfit";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this._targetProfit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _last;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _targetprice;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown _targetProfit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown _size;
        private System.Windows.Forms.Label label4;
    }
}