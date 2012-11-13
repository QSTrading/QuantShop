using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.GUI.Server
{   //用于编辑每个交易所的交易时段,每个交易所有一组交易时间段构成
    //Start-End. 9:15-11:30 13:30-15:15
    public partial class fmSessionEdit : Form
    {
        public fmSessionEdit()
        {
            InitializeComponent();
        }
    }
}
