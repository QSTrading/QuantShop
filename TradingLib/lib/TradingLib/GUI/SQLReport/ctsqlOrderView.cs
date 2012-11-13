using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradeLink.API;
using TradeLink.Common;

namespace TradingLib.GUI.SQLReport
{
    public partial class ctsqlOrderView : UserControl
    {
        public event DebugDelegate SendDebugEvent;
        DataTable qt = new DataTable();
        
        const string ORDERID="委托号";
        //const string DATE="日期";
        const string TIME="时间";
        const string SIDE = "买/卖";
        const string SYMBOL = "代码";
        const string SIZE = "数量";
        const string TYPE = "类型";//MARKET,STOP,LIMIT
        const string PRICE = "价格";
        const string EXCHANGE = "交易所";
        const string TIF="时间条件";
        const string FILLED = "成交";
        const string STATUS = "状态";
        const string ACCOUNT = "账户";
        


        

        
        public ctsqlOrderView()
        {
            InitializeComponent();
            initGrid();
        }
        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }


        private void initGrid()
        {
            qt.Columns.Add(ORDERID);
            //qt.Columns.Add(DATE);
            qt.Columns.Add(TIME);
            qt.Columns.Add(SIDE);
            qt.Columns.Add(SYMBOL);
            qt.Columns.Add(SIZE);
            qt.Columns.Add(TYPE);
            qt.Columns.Add(PRICE);
            qt.Columns.Add(EXCHANGE);
            qt.Columns.Add(TIF);
            qt.Columns.Add(FILLED);
            qt.Columns.Add(STATUS);
            qt.Columns.Add(ACCOUNT);

            dg.DataSource = qt;
        
        
        }
        private DataSet _ds;
        public DataSet DataSource 
        {  set
            {
                debug("set data source");
                _ds = value;
                qt.Clear();
                DataTable dt = _ds.Tables["orders"];
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    DataRow dr = dt.Rows[i];

                    string type = "";
                    string status = "";
                    int filled = Convert.ToInt16(dr["filled"]);
                    decimal price = Convert.ToDecimal(dr["price"]);
                    decimal stop = Convert.ToDecimal(dr["stop"]);
                    int side = Convert.ToInt16(dr["side"]);
                    DateTime date = Convert.ToDateTime(dr["date"]);
                    //DateTime time = Convert.ToDateTime(dr["time"]);
                    string timestr = Util.ToTLDate(date).ToString() + "/"+dr["time"].ToString();

                    if((price==0) && (stop ==0))
                        type = "市价";
                    if (price > 0 && stop == 0)
                        type = "限价";
                    if(stop>0 && price==0)
                        type="追价";
                        price = stop;
                    
                    bool cancled = Convert.ToBoolean(dr["cancled"]);
                    if (cancled)
                        status = "已取消";
                    if(cancled)
                        if(filled != 0)
                            status = "部分成交";
                            if(filled==Convert.ToInt16(dr["size"].ToString()))
                                status = "已全成";
                    //(side==true ? "买":"卖")
                    qt.Rows.Add(dr["ordid"],timestr,(side ==1?"买":"卖"), dr["symbol"], dr["size"], type, price.ToString(), dr["ex"], dr["tif"],filled.ToString(),status,dr["account"]);
                }
                
            }
        }

        private void ctsqlOrderView_Load(object sender, EventArgs e)
        {

        }
        

        
       
    }
}
