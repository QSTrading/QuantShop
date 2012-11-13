namespace TradingLib.GUI.Server
{
    partial class fmAccountMoniter
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup13 = new System.Windows.Forms.ListViewGroup("为成交在队列中", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup14 = new System.Windows.Forms.ListViewGroup("未成交不在队列中", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup15 = new System.Windows.Forms.ListViewGroup("部分成交还在队列中", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup16 = new System.Windows.Forms.ListViewGroup("部分成交不在队列中", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup17 = new System.Windows.Forms.ListViewGroup("已撤单", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup18 = new System.Windows.Forms.ListViewGroup("全部成交", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup19 = new System.Windows.Forms.ListViewGroup("尚未触发", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup20 = new System.Windows.Forms.ListViewGroup("已经触发", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup21 = new System.Windows.Forms.ListViewGroup("未知", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup22 = new System.Windows.Forms.ListViewGroup("今日", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup23 = new System.Windows.Forms.ListViewGroup("历史持仓", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup24 = new System.Windows.Forms.ListViewGroup("套利", System.Windows.Forms.HorizontalAlignment.Left);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.radioButtonTrade = new System.Windows.Forms.RadioButton();
            this.radioButtonMd = new System.Windows.Forms.RadioButton();
            this.labelSHFE = new System.Windows.Forms.Label();
            this.labelCZCE = new System.Windows.Forms.Label();
            this.labelDCE = new System.Windows.Forms.Label();
            this.labelFFEX = new System.Windows.Forms.Label();
            this.comboBoxErrMsg = new System.Windows.Forms.ComboBox();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.btn_freshAccount = new System.Windows.Forms.Button();
            this.listViewOrder = new TradingLib.GUI.QSListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewPosition = new TradingLib.GUI.QSListView();
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader24 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctTradeAccount1 = new TradingLib.GUI.ctTradeAccount();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(964, 521);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listViewOrder);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(956, 496);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "委 托";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewPosition);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(956, 496);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "持 仓";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_freshAccount);
            this.tabPage3.Controls.Add(this.ctTradeAccount1);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(956, 496);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "资 金";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 21);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(956, 496);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "合 约";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 21);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(956, 496);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 21);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(956, 496);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "tabPage6";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // radioButtonTrade
            // 
            this.radioButtonTrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonTrade.AutoCheck = false;
            this.radioButtonTrade.AutoSize = true;
            this.radioButtonTrade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonTrade.ForeColor = System.Drawing.Color.Red;
            this.radioButtonTrade.Location = new System.Drawing.Point(644, 527);
            this.radioButtonTrade.Name = "radioButtonTrade";
            this.radioButtonTrade.Size = new System.Drawing.Size(46, 16);
            this.radioButtonTrade.TabIndex = 18;
            this.radioButtonTrade.Text = "交易";
            this.radioButtonTrade.UseVisualStyleBackColor = true;
            // 
            // radioButtonMd
            // 
            this.radioButtonMd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonMd.AutoCheck = false;
            this.radioButtonMd.AutoSize = true;
            this.radioButtonMd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButtonMd.ForeColor = System.Drawing.Color.Red;
            this.radioButtonMd.Location = new System.Drawing.Point(595, 527);
            this.radioButtonMd.Name = "radioButtonMd";
            this.radioButtonMd.Size = new System.Drawing.Size(46, 16);
            this.radioButtonMd.TabIndex = 19;
            this.radioButtonMd.Text = "行情";
            this.radioButtonMd.UseVisualStyleBackColor = true;
            // 
            // labelSHFE
            // 
            this.labelSHFE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSHFE.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSHFE.Location = new System.Drawing.Point(695, 527);
            this.labelSHFE.Name = "labelSHFE";
            this.labelSHFE.Size = new System.Drawing.Size(65, 19);
            this.labelSHFE.TabIndex = 17;
            this.labelSHFE.Text = "hh:mm:ss";
            // 
            // labelCZCE
            // 
            this.labelCZCE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCZCE.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCZCE.Location = new System.Drawing.Point(760, 527);
            this.labelCZCE.Name = "labelCZCE";
            this.labelCZCE.Size = new System.Drawing.Size(65, 19);
            this.labelCZCE.TabIndex = 16;
            this.labelCZCE.Text = "hh:mm:ss";
            // 
            // labelDCE
            // 
            this.labelDCE.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDCE.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDCE.Location = new System.Drawing.Point(825, 527);
            this.labelDCE.Name = "labelDCE";
            this.labelDCE.Size = new System.Drawing.Size(65, 19);
            this.labelDCE.TabIndex = 14;
            this.labelDCE.Text = "hh:mm:ss";
            // 
            // labelFFEX
            // 
            this.labelFFEX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFFEX.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFFEX.Location = new System.Drawing.Point(886, 527);
            this.labelFFEX.Name = "labelFFEX";
            this.labelFFEX.Size = new System.Drawing.Size(65, 19);
            this.labelFFEX.TabIndex = 15;
            this.labelFFEX.Text = "hh:mm:ss";
            // 
            // comboBoxErrMsg
            // 
            this.comboBoxErrMsg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxErrMsg.FormattingEnabled = true;
            this.comboBoxErrMsg.Location = new System.Drawing.Point(1, 524);
            this.comboBoxErrMsg.Name = "comboBoxErrMsg";
            this.comboBoxErrMsg.Size = new System.Drawing.Size(588, 20);
            this.comboBoxErrMsg.TabIndex = 13;
            // 
            // toolTipInfo
            // 
            this.toolTipInfo.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // btn_freshAccount
            // 
            this.btn_freshAccount.Location = new System.Drawing.Point(157, 19);
            this.btn_freshAccount.Name = "btn_freshAccount";
            this.btn_freshAccount.Size = new System.Drawing.Size(75, 23);
            this.btn_freshAccount.TabIndex = 1;
            this.btn_freshAccount.Text = "刷新";
            this.btn_freshAccount.UseVisualStyleBackColor = true;
            this.btn_freshAccount.Click += new System.EventHandler(this.btn_freshAccount_Click);
            // 
            // listViewOrder
            // 
            this.listViewOrder.AllowColumnReorder = true;
            this.listViewOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewOrder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12});
            this.listViewOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewOrder.FullRowSelect = true;
            listViewGroup13.Header = "为成交在队列中";
            listViewGroup13.Name = "NoTradeQueueing";
            listViewGroup14.Header = "未成交不在队列中";
            listViewGroup14.Name = "NoTradeNotQueueing";
            listViewGroup15.Header = "部分成交还在队列中";
            listViewGroup15.Name = "PartTradedQueueing";
            listViewGroup16.Header = "部分成交不在队列中";
            listViewGroup16.Name = "PartTradedNotQueueing";
            listViewGroup17.Header = "已撤单";
            listViewGroup17.Name = "Canceled";
            listViewGroup18.Header = "全部成交";
            listViewGroup18.Name = "AllTraded";
            listViewGroup19.Header = "尚未触发";
            listViewGroup19.Name = "NotTouched";
            listViewGroup20.Header = "已经触发";
            listViewGroup20.Name = "Touched";
            listViewGroup21.Header = "未知";
            listViewGroup21.Name = "Unknown";
            this.listViewOrder.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup13,
            listViewGroup14,
            listViewGroup15,
            listViewGroup16,
            listViewGroup17,
            listViewGroup18,
            listViewGroup19,
            listViewGroup20,
            listViewGroup21});
            this.listViewOrder.HideSelection = false;
            this.listViewOrder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.listViewOrder.Location = new System.Drawing.Point(3, 3);
            this.listViewOrder.MultiSelect = false;
            this.listViewOrder.Name = "listViewOrder";
            this.listViewOrder.Size = new System.Drawing.Size(950, 490);
            this.listViewOrder.SortColumn = 0;
            this.listViewOrder.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listViewOrder.TabIndex = 0;
            this.listViewOrder.UseCompatibleStateImageBehavior = false;
            this.listViewOrder.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "合约";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "买卖";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "开平";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "报单数量";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "报单价格";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "成交数量";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "成交价格";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "报单时间";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "响应时间";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "报单编号";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "本地编号";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "报单状态";
            // 
            // listViewPosition
            // 
            this.listViewPosition.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader13,
            this.columnHeader14,
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18,
            this.columnHeader19,
            this.columnHeader20,
            this.columnHeader21,
            this.columnHeader22,
            this.columnHeader23,
            this.columnHeader24});
            this.listViewPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup22.Header = "今日";
            listViewGroup22.Name = "Today";
            listViewGroup23.Header = "历史持仓";
            listViewGroup23.Name = "History";
            listViewGroup24.Header = "套利";
            listViewGroup24.Name = "Arbitrage";
            this.listViewPosition.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup22,
            listViewGroup23,
            listViewGroup24});
            this.listViewPosition.Location = new System.Drawing.Point(3, 3);
            this.listViewPosition.Name = "listViewPosition";
            this.listViewPosition.Size = new System.Drawing.Size(950, 490);
            this.listViewPosition.SortColumn = 0;
            this.listViewPosition.TabIndex = 0;
            this.listViewPosition.UseCompatibleStateImageBehavior = false;
            this.listViewPosition.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "合约";
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "多空";
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "昨持";
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "今持";
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "持仓均价";
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "持仓盈亏";
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "开仓均价";
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "手数";
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "平仓均价";
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "手数";
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "平仓盈亏";
            // 
            // columnHeader24
            // 
            this.columnHeader24.Text = "手数";
            // 
            // ctTradeAccount1
            // 
            this.ctTradeAccount1.IsActive = false;
            this.ctTradeAccount1.Location = new System.Drawing.Point(3, 50);
            this.ctTradeAccount1.Name = "ctTradeAccount1";
            this.ctTradeAccount1.Size = new System.Drawing.Size(436, 287);
            this.ctTradeAccount1.TabIndex = 0;
            // 
            // fmAccountMoniter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 545);
            this.Controls.Add(this.radioButtonTrade);
            this.Controls.Add(this.radioButtonMd);
            this.Controls.Add(this.labelSHFE);
            this.Controls.Add(this.labelCZCE);
            this.Controls.Add(this.labelDCE);
            this.Controls.Add(this.labelFFEX);
            this.Controls.Add(this.comboBoxErrMsg);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "fmAccountMoniter";
            this.Text = "fmAccountMoniter";
            this.Load += new System.EventHandler(this.fmAccountMoniter_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage6;
        private TradingLib.GUI.QSListView listViewOrder;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.TabPage tabPage2;
        private QSListView listViewPosition;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.ColumnHeader columnHeader23;
        private System.Windows.Forms.ColumnHeader columnHeader24;
        private System.Windows.Forms.RadioButton radioButtonTrade;
        private System.Windows.Forms.RadioButton radioButtonMd;
        private System.Windows.Forms.Label labelSHFE;
        private System.Windows.Forms.Label labelCZCE;
        private System.Windows.Forms.Label labelDCE;
        private System.Windows.Forms.Label labelFFEX;
        private System.Windows.Forms.ComboBox comboBoxErrMsg;
        private System.Windows.Forms.ToolTip toolTipInfo;
        private System.Windows.Forms.Button btn_freshAccount;
        private ctTradeAccount ctTradeAccount1;
    }
}