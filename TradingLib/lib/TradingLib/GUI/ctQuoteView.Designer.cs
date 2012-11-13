namespace TradingLib.GUI
{
    partial class QuoteView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.qg = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.qg)).BeginInit();
            this.SuspendLayout();
            // 
            // qg
            // 
            this.qg.AllowUserToAddRows = false;
            this.qg.AllowUserToDeleteRows = false;
            this.qg.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSlateGray;
            dataGridViewCellStyle1.Format = "{0:F1}";
            dataGridViewCellStyle1.NullValue = "Nan";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.CadetBlue;
            this.qg.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.qg.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.qg.BackgroundColor = System.Drawing.Color.SlateGray;
            this.qg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GrayText;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.PowderBlue;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.qg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.qg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.SlateGray;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Format = "{0:F1}";
            dataGridViewCellStyle3.NullValue = "Nan";
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.CadetBlue;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Lavender;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.qg.DefaultCellStyle = dataGridViewCellStyle3;
            this.qg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.qg.Location = new System.Drawing.Point(0, 0);
            this.qg.MultiSelect = false;
            this.qg.Name = "qg";
            this.qg.ReadOnly = true;
            this.qg.RowTemplate.Height = 23;
            this.qg.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.qg.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.qg.Size = new System.Drawing.Size(700, 250);
            this.qg.TabIndex = 0;
            this.qg.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.qg_CellMouseDoubleClick);
            // 
            // QuoteView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.qg);
            this.Name = "QuoteView";
            this.Size = new System.Drawing.Size(700, 250);
            ((System.ComponentModel.ISupportInitialize)(this.qg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView qg;
    }
}
