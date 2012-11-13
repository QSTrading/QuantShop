using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows;
namespace Ant.Server
{
    class Utils
    {
        public const string SELECTION_CONFIG = "CONFIG";
        public const string KEY_PORT = "_PORT";
        public const string CONFIG_FILE = "config.ini";
        public const string UPDATE_FILE = "update_info.xml";
        public static Smark.Core.RasCrypto Ras = new Smark.Core.RasCrypto();
        public static Component.INIClass INI;
        public static string GetFileFullName(string name)
        {
            return Application.StartupPath + "\\" + name;
        }
        public static void LoadPublicKey()
        {
            string filename = GetFileFullName(Component.FileUtils.PUBLIC_FILE);
            if (System.IO.File.Exists(filename))
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(
                    filename, Encoding.UTF8))
                {
                    Ras.PublicKey = reader.ReadToEnd();
                }
            }
        }
        public static bool CheckPublicKey()
        {
            string publickey = GetFileFullName(Component.FileUtils.PUBLIC_FILE);
            return System.IO.File.Exists(publickey);
        }
        public static void CreateKeyFiles(Smark.Core.RasCrypto ras)
        {
            string privatekey = GetFileFullName(Component.FileUtils.PRIVATE_FILE);
            string publickey = GetFileFullName(Component.FileUtils.PUBLIC_FILE);
            using (System.IO.FileStream stream = System.IO.File.Open(privatekey, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(ras.PrivateKey);
                }
            }
            using (System.IO.FileStream stream = System.IO.File.Open(publickey, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(ras.PublicKey);
                }
            }
        }
        public static void CreateFolder()
        {
            string path = GetFileFullName("data");
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

        }
        public static void LoadINI()
        {
            INI = new Component.INIClass(GetFileFullName(CONFIG_FILE));
        }
        public static string Port
        {
            get
            {
                return INI[SELECTION_CONFIG,KEY_PORT];
            }
            set
            {
                INI[SELECTION_CONFIG, KEY_PORT] = value;
            }
        }
    }
}
