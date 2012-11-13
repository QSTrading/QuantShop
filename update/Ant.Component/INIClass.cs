using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
namespace Ant.Component
{
    public class INIClass
    {
        public string inipath;
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INIClass(string INIPath)
        {
            inipath = INIPath;
            if (!System.IO.File.Exists(INIPath))
            {
                using (System.IO.FileStream stream = System.IO.File.Open(inipath, FileMode.Create, FileAccess.Write))
                {
                }
            }
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }
        public string this[string section, string key]
        {
            get
            {
                return IniReadValue(section, key);
            }
            set
            {
                IniWriteValue(section, key, value);
            }
        }
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, this.inipath);
            return temp.ToString();
        }

        
    }
}
