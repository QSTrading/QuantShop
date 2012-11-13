namespace TradingLib.GUI
{
    partial class ctTradeAccount
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
			this.label1 = new System.Windows.Forms.Label();
			this.labelPreBalance = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.labelWithdraw = new System.Windows.Forms.Label();
			this.labelDeposit = new System.Windows.Forms.Label();
			this.label1Static = new System.Windows.Forms.Label();
			this.labelCurrMargin = new System.Windows.Forms.Label();
			this.labelAvailable = new System.Windows.Forms.Label();
			this.labelWithdrawQuota = new System.Windows.Forms.Label();
			this.labelFrozenMargin = new System.Windows.Forms.Label();
			this.labelFrozenCommission = new System.Windows.Forms.Label();
			this.labelReserve = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.labelPositionProfit = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.labelCloseProfit = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.labelCurrent = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.labelCommission = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(28, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "上次结算";
			// 
			// labelPreBalance
			// 
			this.labelPreBalance.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelPreBalance.AutoSize = true;
			this.labelPreBalance.Location = new System.Drawing.Point(155, 14);
			this.labelPreBalance.Name = "labelPreBalance";
			this.labelPreBalance.Size = new System.Drawing.Size(17, 12);
			this.labelPreBalance.TabIndex = 1;
			this.labelPreBalance.Text = "--";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelPreBalance, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.label6, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.label11, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelWithdraw, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelDeposit, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.label1Static, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelCurrMargin, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelAvailable, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelWithdrawQuota, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelFrozenMargin, 3, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelFrozenCommission, 3, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelReserve, 3, 6);
			this.tableLayoutPanel1.Controls.Add(this.label10, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.label9, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelPositionProfit, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.label13, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelCloseProfit, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.label15, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelCurrent, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.label17, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelCommission, 3, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 7;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37278F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37277F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37277F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37277F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37277F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37277F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.76337F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 287);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(258, 14);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(29, 12);
			this.label4.TabIndex = 3;
			this.label4.Text = "出金";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(258, 55);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 12);
			this.label3.TabIndex = 2;
			this.label3.Text = "入金";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(28, 55);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(53, 12);
			this.label5.TabIndex = 2;
			this.label5.Text = "静态权益";
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(28, 219);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 3;
			this.label7.Text = "可用资金";
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(240, 260);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(65, 12);
			this.label6.TabIndex = 3;
			this.label6.Text = "最低保证金";
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(28, 260);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(53, 12);
			this.label11.TabIndex = 3;
			this.label11.Text = "可取资金";
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(22, 178);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 12);
			this.label8.TabIndex = 4;
			this.label8.Text = "占用保证金";
			// 
			// labelWithdraw
			// 
			this.labelWithdraw.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelWithdraw.AutoSize = true;
			this.labelWithdraw.Location = new System.Drawing.Point(373, 14);
			this.labelWithdraw.Name = "labelWithdraw";
			this.labelWithdraw.Size = new System.Drawing.Size(17, 12);
			this.labelWithdraw.TabIndex = 1;
			this.labelWithdraw.Text = "--";
			// 
			// labelDeposit
			// 
			this.labelDeposit.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelDeposit.AutoSize = true;
			this.labelDeposit.Location = new System.Drawing.Point(373, 55);
			this.labelDeposit.Name = "labelDeposit";
			this.labelDeposit.Size = new System.Drawing.Size(17, 12);
			this.labelDeposit.TabIndex = 1;
			this.labelDeposit.Text = "--";
			// 
			// label1Static
			// 
			this.label1Static.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label1Static.AutoSize = true;
			this.label1Static.Location = new System.Drawing.Point(155, 55);
			this.label1Static.Name = "label1Static";
			this.label1Static.Size = new System.Drawing.Size(17, 12);
			this.label1Static.TabIndex = 1;
			this.label1Static.Text = "--";
			// 
			// labelCurrMargin
			// 
			this.labelCurrMargin.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelCurrMargin.AutoSize = true;
			this.labelCurrMargin.Location = new System.Drawing.Point(155, 178);
			this.labelCurrMargin.Name = "labelCurrMargin";
			this.labelCurrMargin.Size = new System.Drawing.Size(17, 12);
			this.labelCurrMargin.TabIndex = 1;
			this.labelCurrMargin.Text = "--";
			// 
			// labelAvailable
			// 
			this.labelAvailable.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelAvailable.AutoSize = true;
			this.labelAvailable.Location = new System.Drawing.Point(155, 219);
			this.labelAvailable.Name = "labelAvailable";
			this.labelAvailable.Size = new System.Drawing.Size(17, 12);
			this.labelAvailable.TabIndex = 1;
			this.labelAvailable.Text = "--";
			// 
			// labelWithdrawQuota
			// 
			this.labelWithdrawQuota.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelWithdrawQuota.AutoSize = true;
			this.labelWithdrawQuota.Location = new System.Drawing.Point(155, 260);
			this.labelWithdrawQuota.Name = "labelWithdrawQuota";
			this.labelWithdrawQuota.Size = new System.Drawing.Size(17, 12);
			this.labelWithdrawQuota.TabIndex = 1;
			this.labelWithdrawQuota.Text = "--";
			// 
			// labelFrozenMargin
			// 
			this.labelFrozenMargin.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelFrozenMargin.AutoSize = true;
			this.labelFrozenMargin.Location = new System.Drawing.Point(373, 178);
			this.labelFrozenMargin.Name = "labelFrozenMargin";
			this.labelFrozenMargin.Size = new System.Drawing.Size(17, 12);
			this.labelFrozenMargin.TabIndex = 1;
			this.labelFrozenMargin.Text = "--";
			// 
			// labelFrozenCommission
			// 
			this.labelFrozenCommission.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelFrozenCommission.AutoSize = true;
			this.labelFrozenCommission.Location = new System.Drawing.Point(373, 219);
			this.labelFrozenCommission.Name = "labelFrozenCommission";
			this.labelFrozenCommission.Size = new System.Drawing.Size(17, 12);
			this.labelFrozenCommission.TabIndex = 1;
			this.labelFrozenCommission.Text = "--";
			// 
			// labelReserve
			// 
			this.labelReserve.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelReserve.AutoSize = true;
			this.labelReserve.Location = new System.Drawing.Point(373, 260);
			this.labelReserve.Name = "labelReserve";
			this.labelReserve.Size = new System.Drawing.Size(17, 12);
			this.labelReserve.TabIndex = 1;
			this.labelReserve.Text = "--";
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(240, 219);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(65, 12);
			this.label10.TabIndex = 5;
			this.label10.Text = "冻结手续费";
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(240, 178);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(65, 12);
			this.label9.TabIndex = 6;
			this.label9.Text = "冻结保证金";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(28, 96);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "持仓盈亏";
			// 
			// labelPositionProfit
			// 
			this.labelPositionProfit.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelPositionProfit.AutoSize = true;
			this.labelPositionProfit.Location = new System.Drawing.Point(155, 96);
			this.labelPositionProfit.Name = "labelPositionProfit";
			this.labelPositionProfit.Size = new System.Drawing.Size(17, 12);
			this.labelPositionProfit.TabIndex = 1;
			this.labelPositionProfit.Text = "--";
			// 
			// label13
			// 
			this.label13.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(246, 96);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(53, 12);
			this.label13.TabIndex = 2;
			this.label13.Text = "平仓盈亏";
			// 
			// labelCloseProfit
			// 
			this.labelCloseProfit.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelCloseProfit.AutoSize = true;
			this.labelCloseProfit.Location = new System.Drawing.Point(373, 96);
			this.labelCloseProfit.Name = "labelCloseProfit";
			this.labelCloseProfit.Size = new System.Drawing.Size(17, 12);
			this.labelCloseProfit.TabIndex = 1;
			this.labelCloseProfit.Text = "--";
			// 
			// label15
			// 
			this.label15.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(28, 137);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(53, 12);
			this.label15.TabIndex = 2;
			this.label15.Text = "动态权益";
			// 
			// labelCurrent
			// 
			this.labelCurrent.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelCurrent.AutoSize = true;
			this.labelCurrent.Location = new System.Drawing.Point(155, 137);
			this.labelCurrent.Name = "labelCurrent";
			this.labelCurrent.Size = new System.Drawing.Size(17, 12);
			this.labelCurrent.TabIndex = 1;
			this.labelCurrent.Text = "--";
			// 
			// label17
			// 
			this.label17.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(252, 137);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(41, 12);
			this.label17.TabIndex = 2;
			this.label17.Text = "手续费";
			// 
			// labelCommission
			// 
			this.labelCommission.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.labelCommission.AutoSize = true;
			this.labelCommission.Location = new System.Drawing.Point(373, 137);
			this.labelCommission.Name = "labelCommission";
			this.labelCommission.Size = new System.Drawing.Size(17, 12);
			this.labelCommission.TabIndex = 1;
			this.labelCommission.Text = "--";
			// 
			// UserControlTradeAccount
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "UserControlTradeAccount";
			this.Size = new System.Drawing.Size(436, 287);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelPreBalance;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label labelWithdraw;
		private System.Windows.Forms.Label labelDeposit;
		private System.Windows.Forms.Label label1Static;
		private System.Windows.Forms.Label labelCurrMargin;
		private System.Windows.Forms.Label labelAvailable;
		private System.Windows.Forms.Label labelWithdrawQuota;
		private System.Windows.Forms.Label labelFrozenMargin;
		private System.Windows.Forms.Label labelFrozenCommission;
		private System.Windows.Forms.Label labelReserve;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labelPositionProfit;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label labelCloseProfit;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label labelCurrent;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label labelCommission;
	}
}
