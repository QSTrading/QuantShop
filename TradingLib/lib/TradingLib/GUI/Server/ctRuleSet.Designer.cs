namespace TradingLib.GUI.Server
{
    partial class ctRuleSet
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.checkedListBoxRuleSet = new System.Windows.Forms.CheckedListBox();
            this.checkedListBoxAccountRule = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "账户规则列表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "风险控制规则集";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(315, 80);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "<---";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(315, 125);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 5;
            this.btnDel.Text = "-->";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(615, 282);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "应用";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // checkedListBoxRuleSet
            // 
            this.checkedListBoxRuleSet.FormattingEnabled = true;
            this.checkedListBoxRuleSet.Location = new System.Drawing.Point(396, 31);
            this.checkedListBoxRuleSet.Name = "checkedListBoxRuleSet";
            this.checkedListBoxRuleSet.Size = new System.Drawing.Size(294, 244);
            this.checkedListBoxRuleSet.TabIndex = 7;
            this.checkedListBoxRuleSet.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxRuleSet_ItemCheck);
            // 
            // checkedListBoxAccountRule
            // 
            this.checkedListBoxAccountRule.FormattingEnabled = true;
            this.checkedListBoxAccountRule.Location = new System.Drawing.Point(15, 31);
            this.checkedListBoxAccountRule.Name = "checkedListBoxAccountRule";
            this.checkedListBoxAccountRule.Size = new System.Drawing.Size(294, 244);
            this.checkedListBoxAccountRule.TabIndex = 8;
            this.checkedListBoxAccountRule.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxAccountRule_ItemCheck);
            // 
            // ctRuleSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkedListBoxAccountRule);
            this.Controls.Add(this.checkedListBoxRuleSet);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ctRuleSet";
            this.Size = new System.Drawing.Size(700, 309);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckedListBox checkedListBoxRuleSet;
        private System.Windows.Forms.CheckedListBox checkedListBoxAccountRule;
    }
}
