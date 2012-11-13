using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Ant.Update
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(params string[] arges)
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FrmMain main = new FrmMain();
            if (arges != null)
                main.Arges = arges;
            Application.Run(main);
        }
    }
}
