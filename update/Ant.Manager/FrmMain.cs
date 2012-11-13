using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ant.Component;
namespace Ant.Manager
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
       
        private void FrmMain_Load(object sender, EventArgs e)
        {
          
            Beetle.TcpUtils.Setup("beetle");
            try
            {    
                imageList1.Images.Add(Properties.Resources.folder);
                imageList1.Images.Add(Properties.Resources.files);
                Utils.LoadINI();
                Utils.LoadPrivateKey();
                if(!string.IsNullOrEmpty(Utils.Path))
                     Utils.UpdateInfo.Load(Utils.Path+Utils.UPDATE_FILE);
                txtSelectPath.Text = Utils.Path;
                if (!string.IsNullOrEmpty(Utils.Path))
                {
                    LoadResource();
                }
            }
            catch (Exception e_)
            {
                MessageBox.Show(this, e_.Message, "程序处理错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadChilds(TreeNode parent, IList<ResourceItem> childs)
        {
            foreach (ResourceItem item in childs)
            {
                TreeNode node = new TreeNode(Utils.FormatName(item.Name,item.ModifyDate));
                if (item.Type == ResourceType.Folder)
                {
                    node.ImageIndex = 0;
                    node.SelectedImageIndex = 0;
                    LoadChilds(node, item.Childs);
                }
                else
                {
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                }
                node.Tag = item;

                parent.Nodes.Add(node);

            }
        }
        private void LoadResource()
        {
            treeFiles.Nodes.Clear();
            IList<ResourceItem> items = Utils.GetFiles(Utils.Path);
            TreeNode root = new TreeNode("更新项目");
            treeFiles.Nodes.Add(root);
            foreach (ResourceItem item in items)
            {
                if (item.FullName.IndexOf("update_info.xml") >= 0)
                    continue;
                TreeNode node = new TreeNode(Utils.FormatName(item.Name, item.ModifyDate));
                if (item.Type == ResourceType.Folder)
                {
                    node.ImageIndex = 0;
                    LoadChilds(node, item.Childs);
                }
                else
                {
                    node.ImageIndex = 1;
                    node.SelectedImageIndex = 1;
                }
                node.Tag = item;

                root.Nodes.Add(node);

            }
            treeFiles.ExpandAll();
        }
        protected override void OnClosed(EventArgs e)
        {
            
            base.OnClosed(e);
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Utils.Path = folderBrowserDialog1.SelectedPath;
                if (Utils.Path.LastIndexOf("\\") !=Utils.Path.Length-1)
                {
                    Utils.Path += "\\";
                }
                txtSelectPath.Text = Utils.Path;
                Utils.UpdateInfo.Load(Utils.Path + Utils.UPDATE_FILE);
                LoadResource();
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckPrivateKey())
            {
                MessageBox.Show(this, "签名密钥不存在！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Utils.Path))
            {
                MessageBox.Show(this, "不存在更新目录！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (KeyValuePair<string, FileItem> item in Utils.UpdateInfo.Items)
            {
                item.Value.Status = UpdateItemStatus.None;
            }
            Utils.UpdateInfo.Upate(treeFiles.Nodes[0].Nodes);
            
            Queue<FileItem> items = Utils.UpdateInfo.GetUploads();
            if (items.Count == 0)
            {
                MessageBox.Show(this, "没有可更新数据！", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Utils.UpdateInfo.Save(Utils.Path + Utils.UPDATE_FILE);
            FrmUpload upload = new FrmUpload();
            upload.UpdateItems = items;

            FileItem ui = new FileItem();
            ui.File = "update_info.xml";
            upload.UpdateItems.Enqueue(ui);
            upload.ShowDialog(this);
        }
        private void SetChecked(bool value, TreeNodeCollection nodes)
        {
            if (nodes != null && nodes.Count > 0)
            {
                foreach (TreeNode node in nodes)
                {
                    node.Checked = value;
                    SetChecked(value, node.Nodes);
                }
            }
        }
        private void treeFiles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            SetChecked(e.Node.Checked, e.Node.Nodes);
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://www.henryfan.net");
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer", "http://www.ikende.com/");
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Utils.Path))
                LoadResource();
        }
    }
}
