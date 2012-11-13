﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
namespace TradingLib.GUI
{
    public partial class fmCTPLogin : Form
    {
        public event CTPLoginDel SendLoginEvent;
        public event VoidDelegate SendLogoutEvent;

        //public string FrontAddres { get { return ipaddress.SelectedItem.ToString(); } }
        //public string UserName { get { return UserName; } }
        //public string PassWord { get { return pass} }
        public fmCTPLogin()
        {
            InitializeComponent();
            ipaddress.Items.Add("tcp://180.168.212.52:41213");
            ipaddress.SelectedIndex = 0;
            name.Text = "8562000802";

        }

        private void butcancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //连接服务器
        private void butlogin_Click(object sender, EventArgs e)
        {
            if (SendLoginEvent != null)
            {
                SendLoginEvent(ipaddress.SelectedItem.ToString(), "SYWG", name.Text, pass.Text);
            }
            Close();

        }

        //退出服务器
        private void butlogout_Click(object sender, EventArgs e)
        {
            if (SendLogoutEvent != null) 
            {
                SendLogoutEvent();
            }
            Close();

        }
    }
}
