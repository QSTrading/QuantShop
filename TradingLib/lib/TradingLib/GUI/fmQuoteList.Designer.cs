namespace TradingLib.GUI
{
    partial class fmQuoteList
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_shanghai = new System.Windows.Forms.TabPage();
            this.tab_dalian = new System.Windows.Forms.TabPage();
            this.tab_zhengzhou = new System.Windows.Forms.TabPage();
            this.tab_zjs = new System.Windows.Forms.TabPage();
            this.quote_shfe = new TradingLib.GDIControl.ViewQuoteList();
            this.quote_dce = new TradingLib.GDIControl.ViewQuoteList();
            this.quote_czce = new TradingLib.GDIControl.ViewQuoteList();
            this.quote_cffex = new TradingLib.GDIControl.ViewQuoteList();
            this.tabControl1.SuspendLayout();
            this.tab_shanghai.SuspendLayout();
            this.tab_dalian.SuspendLayout();
            this.tab_zhengzhou.SuspendLayout();
            this.tab_zjs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_shanghai);
            this.tabControl1.Controls.Add(this.tab_dalian);
            this.tabControl1.Controls.Add(this.tab_zhengzhou);
            this.tabControl1.Controls.Add(this.tab_zjs);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(728, 329);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tab_shanghai
            // 
            this.tab_shanghai.Controls.Add(this.quote_shfe);
            this.tab_shanghai.Location = new System.Drawing.Point(4, 21);
            this.tab_shanghai.Name = "tab_shanghai";
            this.tab_shanghai.Padding = new System.Windows.Forms.Padding(3);
            this.tab_shanghai.Size = new System.Drawing.Size(720, 304);
            this.tab_shanghai.TabIndex = 0;
            this.tab_shanghai.Text = "上海";
            this.tab_shanghai.UseVisualStyleBackColor = true;
            // 
            // tab_dalian
            // 
            this.tab_dalian.Controls.Add(this.quote_dce);
            this.tab_dalian.Location = new System.Drawing.Point(4, 21);
            this.tab_dalian.Name = "tab_dalian";
            this.tab_dalian.Padding = new System.Windows.Forms.Padding(3);
            this.tab_dalian.Size = new System.Drawing.Size(720, 304);
            this.tab_dalian.TabIndex = 1;
            this.tab_dalian.Text = "大连";
            this.tab_dalian.UseVisualStyleBackColor = true;
            // 
            // tab_zhengzhou
            // 
            this.tab_zhengzhou.Controls.Add(this.quote_czce);
            this.tab_zhengzhou.Location = new System.Drawing.Point(4, 21);
            this.tab_zhengzhou.Name = "tab_zhengzhou";
            this.tab_zhengzhou.Padding = new System.Windows.Forms.Padding(3);
            this.tab_zhengzhou.Size = new System.Drawing.Size(720, 304);
            this.tab_zhengzhou.TabIndex = 2;
            this.tab_zhengzhou.Text = "郑州";
            this.tab_zhengzhou.UseVisualStyleBackColor = true;
            // 
            // tab_zjs
            // 
            this.tab_zjs.Controls.Add(this.quote_cffex);
            this.tab_zjs.Location = new System.Drawing.Point(4, 21);
            this.tab_zjs.Name = "tab_zjs";
            this.tab_zjs.Padding = new System.Windows.Forms.Padding(3);
            this.tab_zjs.Size = new System.Drawing.Size(720, 304);
            this.tab_zjs.TabIndex = 3;
            this.tab_zjs.Text = "中金所";
            this.tab_zjs.UseVisualStyleBackColor = true;
            // 
            // quote_shfe
            // 
            this.quote_shfe.BackColor = System.Drawing.Color.Black;
            this.quote_shfe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quote_shfe.HeaderBackColor = System.Drawing.Color.Black;
            this.quote_shfe.HeaderFont = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_shfe.HeaderFontColor = System.Drawing.Color.Turquoise;
            this.quote_shfe.Location = new System.Drawing.Point(3, 3);
            this.quote_shfe.Name = "quote_shfe";
            this.quote_shfe.QuoteBackColor1 = System.Drawing.Color.Black;
            this.quote_shfe.QuoteBackColor2 = System.Drawing.Color.Black;
            this.quote_shfe.QuoteFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_shfe.Size = new System.Drawing.Size(714, 298);
            this.quote_shfe.SymbolFont = new System.Drawing.Font("Times New Roman", 12F);
            this.quote_shfe.SymbolFontColor = System.Drawing.Color.Orange;
            this.quote_shfe.TabIndex = 0;
            this.quote_shfe.TableLineColor = System.Drawing.Color.Black;
            this.quote_shfe.Text = "viewQuoteList2";
            // 
            // quote_dce
            // 
            this.quote_dce.BackColor = System.Drawing.Color.Black;
            this.quote_dce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quote_dce.HeaderBackColor = System.Drawing.Color.Black;
            this.quote_dce.HeaderFont = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_dce.HeaderFontColor = System.Drawing.Color.Turquoise;
            this.quote_dce.Location = new System.Drawing.Point(3, 3);
            this.quote_dce.Name = "quote_dce";
            this.quote_dce.QuoteBackColor1 = System.Drawing.Color.Black;
            this.quote_dce.QuoteBackColor2 = System.Drawing.Color.Black;
            this.quote_dce.QuoteFont = new System.Drawing.Font("Arial", 9.75F);
            this.quote_dce.Size = new System.Drawing.Size(714, 298);
            this.quote_dce.SymbolFont = new System.Drawing.Font("Times New Roman", 12F);
            this.quote_dce.SymbolFontColor = System.Drawing.Color.Green;
            this.quote_dce.TabIndex = 0;
            this.quote_dce.TableLineColor = System.Drawing.Color.Black;
            this.quote_dce.Text = "viewQuoteList1";
            // 
            // quote_czce
            // 
            this.quote_czce.BackColor = System.Drawing.Color.Black;
            this.quote_czce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quote_czce.HeaderBackColor = System.Drawing.Color.Black;
            this.quote_czce.HeaderFont = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_czce.HeaderFontColor = System.Drawing.Color.Turquoise;
            this.quote_czce.Location = new System.Drawing.Point(3, 3);
            this.quote_czce.Name = "quote_czce";
            this.quote_czce.QuoteBackColor1 = System.Drawing.Color.Black;
            this.quote_czce.QuoteBackColor2 = System.Drawing.Color.Black;
            this.quote_czce.QuoteFont = new System.Drawing.Font("Arial", 9.75F);
            this.quote_czce.Size = new System.Drawing.Size(714, 298);
            this.quote_czce.SymbolFont = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_czce.SymbolFontColor = System.Drawing.Color.Green;
            this.quote_czce.TabIndex = 0;
            this.quote_czce.TableLineColor = System.Drawing.Color.Black;
            this.quote_czce.Text = "viewQuoteList1";
            // 
            // quote_cffex
            // 
            this.quote_cffex.BackColor = System.Drawing.Color.Black;
            this.quote_cffex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quote_cffex.HeaderBackColor = System.Drawing.Color.Black;
            this.quote_cffex.HeaderFont = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_cffex.HeaderFontColor = System.Drawing.Color.Turquoise;
            this.quote_cffex.Location = new System.Drawing.Point(3, 3);
            this.quote_cffex.Name = "quote_cffex";
            this.quote_cffex.QuoteBackColor1 = System.Drawing.Color.Black;
            this.quote_cffex.QuoteBackColor2 = System.Drawing.Color.Black;
            this.quote_cffex.QuoteFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quote_cffex.Size = new System.Drawing.Size(714, 298);
            this.quote_cffex.SymbolFont = new System.Drawing.Font("Times New Roman", 12F);
            this.quote_cffex.SymbolFontColor = System.Drawing.Color.Green;
            this.quote_cffex.TabIndex = 0;
            this.quote_cffex.TableLineColor = System.Drawing.Color.Black;
            this.quote_cffex.Text = "viewQuoteList1";
            // 
            // fmQuoteList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 329);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "fmQuoteList";
            this.Text = "报价中心";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmQuoteList_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tab_shanghai.ResumeLayout(false);
            this.tab_dalian.ResumeLayout(false);
            this.tab_zhengzhou.ResumeLayout(false);
            this.tab_zjs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_shanghai;
        private System.Windows.Forms.TabPage tab_dalian;
        private System.Windows.Forms.TabPage tab_zhengzhou;
        private System.Windows.Forms.TabPage tab_zjs;
        private GDIControl.ViewQuoteList quote_cffex;
        private GDIControl.ViewQuoteList quote_shfe;
        private GDIControl.ViewQuoteList quote_czce;
        private GDIControl.ViewQuoteList quote_dce;
    }
}