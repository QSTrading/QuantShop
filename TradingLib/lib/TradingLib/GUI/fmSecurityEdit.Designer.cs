namespace TradingLib.GUI
{
    partial class fmSecurityEdit
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
            this.secGrid = new System.Windows.Forms.DataGridView();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.family = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceTick = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mutiple = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.margin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.secGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // secGrid
            // 
            this.secGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.secGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.secGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.symbol,
            this.fullName,
            this.exchange,
            this.family,
            this.priceTick,
            this.mutiple,
            this.margin});
            this.secGrid.Location = new System.Drawing.Point(0, 50);
            this.secGrid.Name = "secGrid";
            this.secGrid.RowHeadersVisible = false;
            this.secGrid.RowTemplate.Height = 23;
            this.secGrid.Size = new System.Drawing.Size(700, 450);
            this.secGrid.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(27, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            // 
            // symbol
            // 
            this.symbol.HeaderText = "合约代码";
            this.symbol.Name = "symbol";
            // 
            // fullName
            // 
            this.fullName.HeaderText = "名称";
            this.fullName.Name = "fullName";
            // 
            // exchange
            // 
            this.exchange.HeaderText = "交易所";
            this.exchange.Name = "exchange";
            // 
            // family
            // 
            this.family.HeaderText = "合约集";
            this.family.Name = "family";
            // 
            // priceTick
            // 
            this.priceTick.HeaderText = "价格变动";
            this.priceTick.Name = "priceTick";
            // 
            // mutiple
            // 
            this.mutiple.HeaderText = "合约乘数";
            this.mutiple.Name = "mutiple";
            // 
            // margin
            // 
            this.margin.HeaderText = "保证金比例";
            this.margin.Name = "margin";
            // 
            // SecurityEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 466);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.secGrid);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "SecurityEditForm";
            this.Text = "SecurityEditForm";
            ((System.ComponentModel.ISupportInitialize)(this.secGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView secGrid;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchange;
        private System.Windows.Forms.DataGridViewTextBoxColumn family;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceTick;
        private System.Windows.Forms.DataGridViewTextBoxColumn mutiple;
        private System.Windows.Forms.DataGridViewTextBoxColumn margin;
    }
}