using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.API;
using System.Reflection;
using System.IO;

namespace TradingLib.Core
{
    public class StrategyHelper
    {
        public static void genExitPositonStrategyList()
        {
            Response tmp = new InvalidResponse();
            //tmp = ResponseLoader.FromDLL(resname, Properties.Settings.Default.boxdll);
        }

        void LoadResponseDLL(string filename)
        {
            // make sure response library exists
            if (!System.IO.File.Exists(filename))
            {
                //status("file does not exist: " + filename);
                return;
            }

            // set response library to current library
            //Properties.Settings.Default.boxdll = filename;

            // get names of responses in library
            //List<string> list = Util.GetResponseList(filename);
            // clear list of available responses
            //_availresponses.Items.Clear();
            // add each response to user
            //foreach (string res in list)
            //    _availresponses.Items.Add(res);
            // update display
           // _availresponses.Invalidate(true);
        }

        //通过提供接口类型我们可以通过接口来查找我们实现的相应策略
        public static List<Type> GetResponseListViaType<T>()
        {
            List<Type> tmp = new List<Type>();
            foreach(Type t in GetResponseListFromPath())
            {
                if (typeof(T).IsAssignableFrom(t))
                    tmp.Add(t);
            }

            return tmp;
            
        }

        //遍历文件夹下面所有的dll文件来加载所有的Response文件
        public static List<Type> GetResponseListFromPath() { return GetResponseListFromPath(@"strategy"); }
        public static List<Type> GetResponseListFromPath(string filepath)
        {
            List<string> l = new List<string>();
            DirectoryInfo dir = new DirectoryInfo(filepath);
            List<Type> tmp = new List<Type>();
            foreach(FileInfo dchild in dir.GetFiles())
            {
                l.Add(dchild.FullName);
                tmp.AddRange(GetResponseList(dchild.FullName));
            }
            return tmp;

        }

        public static List<Type> GetResponseList(string dllfilepath)
        {
            List<Type> reslist = new List<Type>();
            if (!File.Exists(dllfilepath)) return reslist;
            Assembly a;
            try
            {
                a = Assembly.LoadFile(dllfilepath);
            }
            catch (Exception ex) { return reslist; }
            return GetResponseList(a);
        }
        /// <summary>
        /// Gets all Response names found in a given assembly.  Names are FullNames, which means namespace.FullName.  eg 'BoxExamples.BigTradeUI'
        /// </summary>
        /// <param name="boxdll">The assembly.</param>
        /// <returns>list of response names</returns>
        public static List<Type> GetResponseList(Assembly responseassembly) { return GetResponseList(responseassembly, null); }
        public static List<Type> GetResponseList(Assembly responseassembly, DebugDelegate deb)
        {
            List<Type> reslist = new List<Type>();
            Type[] t;
            try
            {
                t = responseassembly.GetTypes();
                for (int i = 0; i < t.GetLength(0); i++)
                    if (IsResponse(t[i])) reslist.Add(t[i]);
            }
            catch (Exception ex)
            {
                if (deb != null)
                {
                    deb(ex.Message + ex.StackTrace);
                }
            }

            return reslist;
        }

        //检查是否是Response
        static bool IsResponse(Type t)
        {
            return typeof(Response).IsAssignableFrom(t);
        }



    }
}
