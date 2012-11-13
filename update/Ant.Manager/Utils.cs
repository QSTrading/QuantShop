using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Ant.Component;
namespace Ant.Manager
{
    class Utils
    {
        public const string SELECTION_CONFIG = "CONFIG";
        public const string KEY_PATH = "_PATH";
        public const string KEY_PORT = "_PORT";
        public const string KEY_IPADDRESS = "_IPADDRESS";
        public static Component.INIClass INI;
        public const string UPDATE_FILE = "update_info.xml";
        public const string CONFIG_FILE = "config.ini";
        public static Smark.Core.RasCrypto Ras = new Smark.Core.RasCrypto();
        public static UpdateInfo UpdateInfo = new UpdateInfo();
        public static string GetFileFullName(string name)
        {
            return Application.StartupPath + "\\" + name;
        }
        public static void LoadPrivateKey()
        {
            string filename = GetFileFullName(Component.FileUtils.PRIVATE_FILE);
            if (System.IO.File.Exists(filename))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(
                    filename, Encoding.UTF8))
                {
                    Ras.PrivateKey = reader.ReadToEnd();
                }
            }
        }
        public static bool CheckPrivateKey()
        {
            string publickey = GetFileFullName(Component.FileUtils.PRIVATE_FILE);
            return System.IO.File.Exists(publickey);
        }
        public static void LoadINI()
        {
            INI = new Component.INIClass(GetFileFullName(CONFIG_FILE));
        }
        public static string Port
        {
            get
            {
                return INI.IniReadValue(SELECTION_CONFIG, KEY_PORT);
            }
            set
            {
                INI.IniWriteValue(SELECTION_CONFIG, KEY_PORT, value);
            }
        }
        public static string IPAddress
        {
            get
            {
                return INI.IniReadValue(SELECTION_CONFIG, KEY_IPADDRESS);
            }
            set
            {
                INI.IniWriteValue(SELECTION_CONFIG, KEY_IPADDRESS,value);
            }
        }
        public static string Path
        {
            get
            {
                return INI.IniReadValue(SELECTION_CONFIG, KEY_PATH);
            }
            set
            {
                INI.IniWriteValue(SELECTION_CONFIG, KEY_PATH, value);
            }
        }
        public static string FormatName(string name, DateTime date)
        {
            if (date == DateTime.MinValue)
                return name;
            int count = 60 - Encoding.ASCII.GetBytes(name).Length - date.ToString().Length;
            StringBuilder sb = new StringBuilder();
            sb.Append(name);
            for (int i = 0; i < count; i++)
            {
                sb.Append(" ");
            }
            sb.Append(date);
            return sb.ToString();
        }
        public static List<ResourceItem> GetFiles(string path)
        {
            List<ResourceItem> result = new List<ResourceItem>();
            ResourceItem resource;
            string[] folder = System.IO.Directory.GetDirectories(path);
            foreach (string f in folder)
            {
                resource = new ResourceItem();
                resource.FullName = f;
                resource.Childs = GetFiles(f);
                resource.Type = ResourceType.Folder;
                result.Add(resource);
            }
            string[] file = System.IO.Directory.GetFiles(path);
            foreach (string f in file)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(f);

                resource = new ResourceItem();
                resource.FullName = f.Replace(Path,"");
                if (resource.FullName.Substring(0, 1) == "\\")
                {
                    resource.FullName = resource.FullName.Substring(1, resource.FullName.Length - 1);
                }
                resource.ModifyDate = info.LastWriteTime;
                resource.Type = ResourceType.File;
                result.Add(resource);
            }
            return result;
        }
        public static Component.Progress FileProgress = new Component.Progress(400, 24);
        public static Component.Progress TotalProgress = new Component.Progress(400, 24);
    }
}
