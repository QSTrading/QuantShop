using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.Data;

namespace TradingLib.GUI
{
    //用于显示本地历史数据中所有保存数据的Symbol
    public partial class ctMSDataSymbolList : UserControl
    {
        QSFileDataManager _fmdm = new QSFileDataManager("d:\\data\\");
        public ctMSDataSymbolList()
        {
            InitializeComponent();

            dataGridView1.DataSource = _fmdm.GetTable();

        }
    }
}
