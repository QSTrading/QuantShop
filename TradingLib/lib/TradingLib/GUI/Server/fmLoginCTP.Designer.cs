namespace TradingLib.GUI.Server
{
	partial class FormLoginTrade
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.Progressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.labelStat = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelState = new System.Windows.Forms.ToolStripStatusLabel();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.buttonSetServer = new System.Windows.Forms.Button();
            this.savePWDCheckBox = new System.Windows.Forms.CheckBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUserID = new System.Windows.Forms.TextBox();
            this.settingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(81, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(81, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "用户代码";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "密    码";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(83, 148);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "登  录";
            this.btnLogin.UseVisualStyleBackColor = true;
            
            // 
            // btnExit
            // 
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Location = new System.Drawing.Point(191, 148);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "退  出";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Progressbar,
            this.labelStat,
            this.labelState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 202);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(416, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Progressbar
            // 
            this.Progressbar.Name = "Progressbar";
            this.Progressbar.Size = new System.Drawing.Size(300, 16);
            // 
            // labelStat
            // 
            this.labelStat.AutoToolTip = true;
            this.labelStat.Name = "labelStat";
            this.labelStat.Size = new System.Drawing.Size(0, 17);
            // 
            // labelState
            // 
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(143, 12);
            this.labelState.Tag = "    ";
            this.labelState.Text = "                       ";
            this.labelState.ToolTipText = "双击显示事件列表";
            // 
            // cbServer
            // 
            this.cbServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(145, 41);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(121, 20);
            this.cbServer.TabIndex = 1;
            this.cbServer.SelectedIndexChanged += new System.EventHandler(this.cbServer_SelectedIndexChanged);
            // 
            // buttonSetServer
            // 
            this.buttonSetServer.Location = new System.Drawing.Point(272, 39);
            this.buttonSetServer.Name = "buttonSetServer";
            this.buttonSetServer.Size = new System.Drawing.Size(75, 23);
            this.buttonSetServer.TabIndex = 11;
            this.buttonSetServer.Text = "设置服务器";
            this.buttonSetServer.UseVisualStyleBackColor = true;
            this.buttonSetServer.Click += new System.EventHandler(this.buttonSetServer_Click);
            // 
            // savePWDCheckBox
            // 
            this.savePWDCheckBox.Checked = true;
            this.savePWDCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.savePWDCheckBox.Location = new System.Drawing.Point(272, 104);
            this.savePWDCheckBox.Name = "savePWDCheckBox";
            this.savePWDCheckBox.Size = new System.Drawing.Size(65, 24);
            this.savePWDCheckBox.TabIndex = 10;
            this.savePWDCheckBox.Text = "保存";
            this.savePWDCheckBox.UseVisualStyleBackColor = true;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(145, 104);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(121, 21);
            this.tbPassword.TabIndex = 5;
            // 
            // tbUserID
            // 
            this.tbUserID.Location = new System.Drawing.Point(145, 74);
            this.tbUserID.Name = "tbUserID";
            this.tbUserID.Size = new System.Drawing.Size(121, 21);
            this.tbUserID.TabIndex = 4;
            // 
            // settingsBindingSource
            // 
            this.settingsBindingSource.AllowNew = true;
            this.settingsBindingSource.DataSource = typeof(System.Configuration.ApplicationSettingsBase);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(301, 148);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "清除设置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormLoginTrade
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(416, 224);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSetServer);
            this.Controls.Add(this.savePWDCheckBox);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbServer);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLoginTrade";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登 录";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormLoginTrade_FormClosed);
            this.Load += new System.EventHandler(this.UserLogin_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.TextBox tbUserID;
		public System.Windows.Forms.TextBox tbPassword;
		public System.Windows.Forms.ComboBox cbServer;
		public System.Windows.Forms.Button btnLogin;
		public System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.StatusStrip statusStrip1;
		internal System.Windows.Forms.ToolStripProgressBar Progressbar;
		private System.Windows.Forms.ToolStripStatusLabel labelStat;
		private System.Windows.Forms.BindingSource settingsBindingSource;
		private System.Windows.Forms.CheckBox savePWDCheckBox;
		internal System.Windows.Forms.ToolStripStatusLabel labelState;
		private System.Windows.Forms.Button buttonSetServer;
		private System.Windows.Forms.Button button1;
	}
}