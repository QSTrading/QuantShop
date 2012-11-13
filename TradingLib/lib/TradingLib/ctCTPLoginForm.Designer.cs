namespace TradingLib
{
    partial class ctCTPLoginForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ipaddress = new System.Windows.Forms.ComboBox();
            this.name = new System.Windows.Forms.TextBox();
            this.pass = new System.Windows.Forms.TextBox();
            this.butlogin = new System.Windows.Forms.Button();
            this.butcancel = new System.Windows.Forms.Button();
            this.butlogout = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "用户名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "密码";
            // 
            // ipaddress
            // 
            this.ipaddress.FormattingEnabled = true;
            this.ipaddress.Location = new System.Drawing.Point(82, 24);
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(161, 20);
            this.ipaddress.TabIndex = 3;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(82, 54);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(121, 21);
            this.name.TabIndex = 4;
            // 
            // pass
            // 
            this.pass.Location = new System.Drawing.Point(82, 83);
            this.pass.Name = "pass";
            this.pass.Size = new System.Drawing.Size(121, 21);
            this.pass.TabIndex = 5;
            // 
            // butlogin
            // 
            this.butlogin.Location = new System.Drawing.Point(19, 139);
            this.butlogin.Name = "butlogin";
            this.butlogin.Size = new System.Drawing.Size(75, 23);
            this.butlogin.TabIndex = 6;
            this.butlogin.Text = "登入";
            this.butlogin.UseVisualStyleBackColor = true;
            this.butlogin.Click += new System.EventHandler(this.butlogin_Click);
            // 
            // butcancel
            // 
            this.butcancel.Location = new System.Drawing.Point(205, 139);
            this.butcancel.Name = "butcancel";
            this.butcancel.Size = new System.Drawing.Size(75, 23);
            this.butcancel.TabIndex = 7;
            this.butcancel.Text = "取消";
            this.butcancel.UseVisualStyleBackColor = true;
            this.butcancel.Click += new System.EventHandler(this.butcancel_Click);
            // 
            // butlogout
            // 
            this.butlogout.Location = new System.Drawing.Point(113, 139);
            this.butlogout.Name = "butlogout";
            this.butlogout.Size = new System.Drawing.Size(75, 23);
            this.butlogout.TabIndex = 8;
            this.butlogout.Text = "注销";
            this.butlogout.UseVisualStyleBackColor = true;
            this.butlogout.Click += new System.EventHandler(this.butlogout_Click);
            // 
            // ctCTPLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 186);
            this.Controls.Add(this.butlogout);
            this.Controls.Add(this.butcancel);
            this.Controls.Add(this.butlogin);
            this.Controls.Add(this.pass);
            this.Controls.Add(this.name);
            this.Controls.Add(this.ipaddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ctCTPLoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ctCTPLogin";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ipaddress;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox pass;
        private System.Windows.Forms.Button butlogin;
        private System.Windows.Forms.Button butcancel;
        private System.Windows.Forms.Button butlogout;
    }
}