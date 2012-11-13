using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.Common;

namespace TradingLib.GUI.SQLReport
{
    public partial class ctsqlTradeView : UserControl
    {
        DataTable qt = new DataTable();

        
        //const string DATE="日期";
        const string TIME = "时间";
        const string SIDE = "买/卖";
        const string SYMBOL = "代码";
        const string SIZE = "数量";
        const string PRICE = "价格";
        const string ORDERID = "委托号";
        const string ACCOUNT = "账户";
        public ctsqlTradeView()
        {
            InitializeComponent();
            initGrid();
        }

        private void initGrid()
        {
            
            //qt.Columns.Add(DATE);
            qt.Columns.Add(TIME);
            qt.Columns.Add(SIDE);
            qt.Columns.Add(SYMBOL);
            qt.Columns.Add(SIZE);
            qt.Columns.Add(PRICE);
            qt.Columns.Add(ORDERID);
            qt.Columns.Add(ACCOUNT);
            
            

            dg.DataSource = qt;


        }

        private DataSet _ds;
        public DataSet DataSource
        {
            set
            {
                _ds = value;
                qt.Clear();
                DataTable dt = _ds.Tables["trades"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    DateTime date = Convert.ToDateTime(dr["xdate"]);
                    //DateTime time = Convert.ToDateTime(dr["time"]);
                    string timestr = Util.ToTLDate(date).ToString() + "/" + dr["xtime"].ToString();

                    string side = string.Empty;
                    if (Convert.ToInt16(dr["xsize"].ToString()) > 0)
                        side = "买";
                    else
                        side = "卖";


                    qt.Rows.Add(timestr,side,dr["symbol"],dr["xsize"],dr["xprice"],dr["ordid"],dr["account"]);
                
                }
            }
        }


    }
}
