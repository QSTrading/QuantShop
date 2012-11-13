using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
namespace Ant.Update
{
    class Utils
    {
        const string CONFIG_FILE = "update_config.ini";
        const string SELECTION_CONFIG = "CONFIG";
        const string KEY_IPADDRESS = "_ADDRESS";
        const string KEY_PORT = "_PORT";
        const string KEY_APPNAME = "_APPNAME";
        const string KEY_AUTOCLOSE="_AUTOCLOSE";
        public const string UPDATE_FILE = "update_info.xml";
        public static Component.Progress FileProgress = new Component.Progress(400, 24);
        public static Component.Progress TotalProgress = new Component.Progress(400, 24);
        public static Component.INIClass INI;
        public static string IPAddress
        {
            get
            {
                return INI.IniReadValue(SELECTION_CONFIG, KEY_IPADDRESS);
            }
            set
            {
                INI.IniWriteValue(SELECTION_CONFIG, KEY_IPADDRESS, value);
            }
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
        public static string AppName
        {
            get
            {
                return INI.IniReadValue(SELECTION_CONFIG, KEY_APPNAME);
            }
            set
            {
                INI.IniWriteValue(SELECTION_CONFIG, KEY_APPNAME, value);
            }
        }
        public static string AutoClose
        {
            get
            {
                return INI.IniReadValue(SELECTION_CONFIG, KEY_AUTOCLOSE);
            }
            set
            {
                INI.IniWriteValue(SELECTION_CONFIG, KEY_AUTOCLOSE, value);
            }
        }
        public static string GetFileFullName(string name)
        {
            return Application.StartupPath + "\\" + name;
        }
        
        public static void LoadINI()
        {
            INI = new Component.INIClass(GetFileFullName(CONFIG_FILE));
        }
    }
}
