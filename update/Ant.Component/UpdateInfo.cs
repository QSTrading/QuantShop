using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace Ant.Component
{
    public class UpdateInfo
    {
        Dictionary<string, FileItem> mItems = new Dictionary<string, FileItem>();
        public Dictionary<string, FileItem> Items
        {
            get
            {
                return mItems;
            }
        }
        public void LoadXml(string xmlstring)
        {
            Items.Clear();
            if (!string.IsNullOrEmpty(xmlstring))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstring);
                XmlNode node = doc.SelectSingleNode("/updates");
                if (node != null)
                {
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        FileItem ui = new FileItem();
                        ui.File = item.Attributes["file"].Value;
                        ui.UID = item.Attributes["uid"].Value;
                        ui.Status = UpdateItemStatus.None;
                        mItems.Add(ui.File, ui);
                    }
                }
            }
        }
        public void Load(string file)
        {
            Items.Clear();
            if (System.IO.File.Exists(file))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                XmlNode node = doc.SelectSingleNode("/updates");
                if (node != null)
                {
                    foreach (XmlNode item in node.ChildNodes)
                    {
                        FileItem ui = new FileItem();
                        ui.File = item.Attributes["file"].Value;
                        ui.UID = item.Attributes["uid"].Value;
                        ui.Status = UpdateItemStatus.None;
                        mItems.Add(ui.File, ui);
                    }
                }
            }
        }
        public Queue<FileItem> GetUploads()
        {
            Queue<FileItem> result = new Queue<FileItem>();
            foreach (KeyValuePair<string, FileItem> item in mItems)
            {
                if (item.Value.Status == UpdateItemStatus.Update)
                {
                    result.Enqueue(item.Value);
                }
            }
            return result;
        }
        public void Upate(System.Windows.Forms.TreeNodeCollection nodes)
        {
           
            if (nodes != null && nodes.Count > 0)
            {
                foreach (TreeNode node in nodes)
                {
                    ResourceItem ri = (ResourceItem)node.Tag;
                    if (ri.Type == ResourceType.File && node.Checked)
                    {
                        if (mItems.ContainsKey(ri.FullName))
                        {
                            if (node.Checked)
                            {
                                mItems[ri.FullName].UID = Guid.NewGuid().ToString("N");
                                mItems[ri.FullName].Status = UpdateItemStatus.Update;
                            }
                            else
                            {
                                mItems[ri.FullName].Status = UpdateItemStatus.Old;
                            }
                        }
                        else
                        {
                            FileItem ui = new FileItem();
                            ui.Status = UpdateItemStatus.Update;
                            ui.UID = Guid.NewGuid().ToString("N");
                            ui.File = ri.FullName;
                            mItems.Add(ui.File, ui);
                        }
                    }
                   
                        Upate(node.Nodes);
                    

                }
            }
        }
        public void Save(string file)
        {
            using (System.IO.FileStream stream = System.IO.File.Open(file, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.UTF8))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    writer.WriteLine("<updates>");
                    foreach (KeyValuePair<string, FileItem> item in mItems)
                    {
                        if (item.Value.Status != UpdateItemStatus.None)
                        {
                            writer.WriteLine("<item file=\"{0}\" uid=\"{1}\"/>", item.Value.File, item.Value.UID);
                        }
                    }
                    writer.WriteLine("</updates>");
                }
            }
        }
        public Queue<FileItem> Comparable(UpdateInfo info)
        {
            Queue<FileItem> result = new Queue<FileItem>();
            foreach (KeyValuePair<string, FileItem> item in Items)
            {
                item.Value.Status = UpdateItemStatus.Update;
            }
            foreach (KeyValuePair<string, FileItem> item in info.Items)
            {
                
                if (Items.ContainsKey(item.Key))
                {
                    if (Items[item.Key].UID != item.Value.UID)
                    {
                        result.Enqueue(item.Value);
                        Items[item.Key].UID = item.Value.UID;
                        Items[item.Key].Status = UpdateItemStatus.Update;
                    }
                }
                else
                {
                    Items.Add(item.Key, item.Value);
                    result.Enqueue(item.Value);
                    Items[item.Key].Status = UpdateItemStatus.Update;
                    
                }
            }
            return result;
        }
    }
    public class FileItem
    {
        public string File
        {
            get;
            set;
        }
        public string UID
        {
            get;
            set;
        }
        public long Size
        {
            get;
            set;
        }
        public int PackageSize
        {
            get;
            set;
        }
        public int Packages
        {
            get;
            set;
        }
        public UpdateItemStatus Status
        {
            get;
            set;
        }
        
    }
    public enum UpdateItemStatus
    {
        None,
        Old,
        Update
    }
}
