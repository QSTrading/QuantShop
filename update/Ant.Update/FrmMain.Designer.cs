namespace Ant.Update
{
    partial class FrmMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.txtStatus = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.imgFile = new System.Windows.Forms.PictureBox();
            this.imtTotal = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.imgFile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imtTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStatus
            // 
            this.txtStatus.AutoSize = true;
            this.txtStatus.Location = new System.Drawing.Point(12, 11);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(107, 12);
            this.txtStatus.TabIndex = 8;
            this.txtStatus.Text = "连接更新服务器...";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // imgFile
            // 
            this.imgFile.Location = new System.Drawing.Point(13, 38);
            this.imgFile.Name = "imgFile";
            this.imgFile.Size = new System.Drawing.Size(400, 24);
            this.imgFile.TabIndex = 10;
            this.imgFile.TabStop = false;
            // 
            // imtTotal
            // 
            this.imtTotal.Location = new System.Drawing.Point(13, 68);
            this.imtTotal.Name = "imtTotal";
            this.imtTotal.Size = new System.Drawing.Size(400, 24);
            this.imtTotal.TabIndex = 9;
            this.imtTotal.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 109);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(427, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 131);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.imgFile);
            this.Controls.Add(this.imtTotal);
            this.Controls.Add(this.txtStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QSTrading自动更新";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgFile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imtTotal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtStatus;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox imgFile;
        private System.Windows.Forms.PictureBox imtTotal;
        private System.Windows.Forms.StatusStrip statusStrip1;
    }
}

