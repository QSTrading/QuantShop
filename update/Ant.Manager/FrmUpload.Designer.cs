namespace Ant.Manager
{
    partial class FrmUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUpload));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtIPAddress = new System.Windows.Forms.ToolStripTextBox();
            this.txtPort = new System.Windows.Forms.ToolStripTextBox();
            this.cmdConnection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdUpload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdClose = new System.Windows.Forms.ToolStripButton();
            this.imgFile = new System.Windows.Forms.PictureBox();
            this.imtTotal = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtError = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imtTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtIPAddress,
            this.txtPort,
            this.cmdConnection,
            this.toolStripSeparator1,
            this.cmdUpload,
            this.toolStripSeparator2,
            this.cmdClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(445, 31);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(71, 28);
            this.toolStripLabel1.Text = "服务端地址:";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(190, 31);
            this.txtIPAddress.Text = "127.0.0.1";
            // 
            // txtPort
            // 
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(50, 31);
            this.txtPort.Text = "9560";
            // 
            // cmdConnection
            // 
            this.cmdConnection.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdConnection.Image = global::Ant.Manager.Properties.Resources._1330346151_stock_connect;
            this.cmdConnection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdConnection.Name = "cmdConnection";
            this.cmdConnection.Size = new System.Drawing.Size(28, 28);
            this.cmdConnection.Text = "连接到服务器";
            this.cmdConnection.Click += new System.EventHandler(this.cmdConnection_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // cmdUpload
            // 
            this.cmdUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdUpload.Enabled = false;
            this.cmdUpload.Image = global::Ant.Manager.Properties.Resources._1330163992_Update;
            this.cmdUpload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdUpload.Name = "cmdUpload";
            this.cmdUpload.Size = new System.Drawing.Size(28, 28);
            this.cmdUpload.Text = "更新";
            this.cmdUpload.Click += new System.EventHandler(this.cmdUpload_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // cmdClose
            // 
            this.cmdClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdClose.Image = global::Ant.Manager.Properties.Resources._1330161383_exit;
            this.cmdClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(28, 28);
            this.cmdClose.Text = "关闭";
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click_1);
            // 
            // imgFile
            // 
            this.imgFile.Location = new System.Drawing.Point(21, 39);
            this.imgFile.Name = "imgFile";
            this.imgFile.Size = new System.Drawing.Size(400, 24);
            this.imgFile.TabIndex = 1;
            this.imgFile.TabStop = false;
            // 
            // imtTotal
            // 
            this.imtTotal.Location = new System.Drawing.Point(21, 69);
            this.imtTotal.Name = "imtTotal";
            this.imtTotal.Size = new System.Drawing.Size(400, 24);
            this.imtTotal.TabIndex = 0;
            this.imtTotal.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtError
            // 
            this.txtError.AutoSize = true;
            this.txtError.ForeColor = System.Drawing.Color.Red;
            this.txtError.Location = new System.Drawing.Point(12, 102);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(0, 12);
            this.txtError.TabIndex = 3;
            // 
            // FrmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 128);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.imgFile);
            this.Controls.Add(this.imtTotal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUpload";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "更新文件";
            this.Load += new System.EventHandler(this.FrmUpload_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imtTotal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imtTotal;
        private System.Windows.Forms.PictureBox imgFile;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtIPAddress;
        private System.Windows.Forms.ToolStripButton cmdConnection;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton cmdUpload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton cmdClose;
        private System.Windows.Forms.ToolStripTextBox txtPort;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label txtError;
    }
}