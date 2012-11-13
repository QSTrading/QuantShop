namespace TradingLib.GUI
{
    partial class ctTimeSales
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
            this.timeAndSales1 = new TradingLib.TimeAndSales.TimeAndSales();
            this.ctQuotePanel1 = new Responses.ctQuotePanel();
            this.SuspendLayout();
            // 
            // timeAndSales1
            // 
            this.timeAndSales1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.timeAndSales1.BackColor = System.Drawing.Color.Black;
            this.timeAndSales1.HeaderFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeAndSales1.HeaderFontColor = System.Drawing.Color.Khaki;
            this.timeAndSales1.Location = new System.Drawing.Point(0, 200);
            this.timeAndSales1.Margin = new System.Windows.Forms.Padding(0);
            this.timeAndSales1.Name = "timeAndSales1";
            this.timeAndSales1.QuoteViewWidth = 160;
            this.timeAndSales1.Size = new System.Drawing.Size(200, 350);
            this.timeAndSales1.TabIndex = 3;
            this.timeAndSales1.TableBackColor = System.Drawing.Color.Black;
            this.timeAndSales1.Text = "timeAndSales1";
            this.timeAndSales1.TickFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.timeAndSales1.TickFontColor = System.Drawing.Color.Green;
            // 
            // ctQuotePanel1
            // 
            this.ctQuotePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctQuotePanel1.BackColor = System.Drawing.Color.DimGray;
            this.ctQuotePanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctQuotePanel1.Location = new System.Drawing.Point(0, 0);
            this.ctQuotePanel1.Margin = new System.Windows.Forms.Padding(0);
            this.ctQuotePanel1.Name = "ctQuotePanel1";
            this.ctQuotePanel1.Security = null;
            this.ctQuotePanel1.Size = new System.Drawing.Size(200, 200);
            this.ctQuotePanel1.TabIndex = 2;
            this.ctQuotePanel1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ctQuotePanel1_MouseDoubleClick);
            // 
            // ctTimeSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeAndSales1);
            this.Controls.Add(this.ctQuotePanel1);
            this.Name = "ctTimeSales";
            this.Size = new System.Drawing.Size(200, 550);
            this.ResumeLayout(false);

        }

        #endregion

        private Responses.ctQuotePanel ctQuotePanel1;
        private TimeAndSales.TimeAndSales timeAndSales1;
    }
}
