using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TradingLib
{
    public class CfgConstBF
    {
        public const string XMLFN = "BrokerFeed";
        public const string IPAddress = "IPAddress";
        public const string ServerPort = "ServerPort";
        public const string BrokerTimeoutSec = "BrokerTimeoutSec";
        public const string PreferredExec = "PreferredExec";
        public const string PreferredQuote = "PreferredQuote";
        public const string FallbackToAnyProvider = "FallbackToAnyProvider";

    }
    public class ConfigHelper
    {
        /// <summary>
        /// 根据键值获取配置文件
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>

        private Configuration _conf = null;
        private string fname = null;

        public ConfigHelper()
        {
            fname = @"config\"+"APP"+".xml";
            _conf = ConfigurationManager.OpenExeConfiguration(@"config\"+"APP"+".xml" );
        }

        public ConfigHelper(string pname)
        { 
            fname = @"config\"+pname+".xml";
            _conf =ConfigurationManager.OpenExeConfiguration(fname);
            
        }
        public  string GetConfig(string key)
        {
            string val = string.Empty;
            if (_conf.AppSettings.Settings.AllKeys.Contains(key))
                val = _conf.AppSettings.Settings[key].Value;
            return val;
        }

        /// <summary>
        /// 获取所有配置文件
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetConfig()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string key in _conf.AppSettings.Settings.AllKeys)
                dict.Add(key, _conf.AppSettings.Settings[key].Value);
            return dict;
        }

        /// <summary>
        /// 根据键值获取配置文件
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public string GetConfig(string key, string defaultValue)
        {
            string val = defaultValue;
            if (_conf.AppSettings.Settings.AllKeys.Contains(key))
                val = _conf.AppSettings.Settings[key].Value;
            if (val == null)
                val = defaultValue;
            return val;
        }

        /// <summary>
        /// 写配置文件,如果节点不存在则自动创建
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public bool SetConfig(string key, string value)
        {
            try
            {
                if (!_conf.AppSettings.Settings.AllKeys.Contains(key))
                    _conf.AppSettings.Settings.Add(key, value);
                else
                    _conf.AppSettings.Settings[key].Value = value;
                _conf.Save();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 写配置文件(用键值创建),如果节点不存在则自动创建
        /// </summary>
        /// <param name="dict">键值集合</param>
        /// <returns></returns>
        public bool SetConfig(Dictionary<string, string> dict)
        {
            try
            {
                if (dict == null || dict.Count == 0)
                    return false;
                foreach (string key in dict.Keys)
                {
                    if (!_conf.AppSettings.Settings.AllKeys.Contains(key))
                        _conf.AppSettings.Settings.Add(key, dict[key]);
                    else
                        _conf.AppSettings.Settings[key].Value = dict[key];
                }
                _conf.Save();
                return true;
            }
            catch { return false; }
        }
    }
}
