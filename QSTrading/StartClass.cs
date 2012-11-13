using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ant.Component;
namespace QSTrading
{
    public class Starter : TradingLib.GUI.SplashScreenApplicationContext
    {
        startForm sfm;
        Updater _update = new Updater();

        //用于调用升级逻辑,然后再显示启动窗口与主窗口
        protected override bool onUpdate()
        {
            //MessageBox.Show("hello world we are updating");

            if (_update.Detect())
            {
                MessageBox.Show("有新版本,请更新后再运行程序");
                _update.Update("QSTrading",true);
                return true;
            }
            //没有更新我们返回false 程序正常运行
            return false;

        }
        protected override void OnCreateSplashScreenForm()
        {
            sfm = new startForm();
            this.SplashScreenForm = sfm;//启动窗体
        }

        protected override void OnCreateMainForm()
        {
            //将主窗体加载进行信息输出到启动窗体
            this.PrimaryForm = new mainForm(sfm.setInfo);//主窗体
        }

        protected override void SetSeconds()
        {
            this.SecondsShow = 2;//启动窗体显示的时间(秒)
        }
    }
}
