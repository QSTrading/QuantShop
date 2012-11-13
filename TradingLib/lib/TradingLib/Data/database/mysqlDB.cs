using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using TradeLink.API;
using System.Data;

namespace TradingLib.Data.database
{
    public class mysqlDB
    {
        string connectionString = "server=127.0.0.1;user id=root; password=123456; database=quantshop; pooling=false;charset=utf8";
        MySqlConnection conn;
        MySqlCommand cmd;
        //增加容错机制,保证数据正确的记录到数据库
        public mysqlDB()
        {
            conn = new MySqlConnection(connectionString);
            cmd = conn.CreateCommand();
            conn.Open();
        }

        private bool isConnOk()
        {
            if (conn.State == System.Data.ConnectionState.Open)
                return true;
            return false;
        }

        private bool readyToWrite()
        {
            if (isConnOk())
                return true;
            else
            {
                try
                {
                    conn.Close();
                }
                catch (Exception ex)
                { 
                
                }
                try
                {
                    conn = new MySqlConnection(connectionString);
                    cmd = conn.CreateCommand();
                    conn.Open();
                    return true;
                }
                catch (Exception ex)
                { 
                
                }
            }
            return false;
        }

        //插入Order委托指令
        public bool insertOrder(Order o)
        {
            //if (!readyToWrite()) return false;
            string sql = String.Format("Insert into orders (`symbol`,`size`,`price`,`stop`,`comment`,`ex`,`account`,`security`,`currency`,`localsymbol`,`ordid`,`tif`,`date`,`time`,`trail`,`side`) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')", o.symbol, (o.UnsignedSize * (o.side ? 1 : -1)).ToString(), o.price.ToString(System.Globalization.CultureInfo.InvariantCulture), o.stopp.ToString(System.Globalization.CultureInfo.InvariantCulture), o.comment, o.ex, o.Account, o.Security.ToString(), o.Currency.ToString(), o.LocalSymbol, o.id.ToString(), o.TIF, o.date.ToString(), o.time.ToString(), o.trail.ToString(System.Globalization.CultureInfo.InvariantCulture), (o.side ? "1" : "-1"));
            cmd.CommandText = sql;
            return (cmd.ExecuteNonQuery()>0);
        }
        public bool insertTrade(Trade f)
        {
            //if (!readyToWrite()) return false;
            string sql = String.Format("Insert into trades (`ordid`,`xsize`,`xprice`,`xdate`,`xtime`,`symbol`,`account`,`fillid`) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", f.id.ToString(),f.xsize.ToString(),f.xprice.ToString(),f.xdate.ToString(),f.xtime.ToString(),f.symbol.ToString(),f.Account.ToString(),f.id.ToString());
            cmd.CommandText = sql;
            return ((cmd.ExecuteNonQuery() > 0) && (fillOrder(f)>0));
        }

        public bool insertCancel(int date,int time,long oid)
        {
            //if (!readyToWrite()) return false;
            string sql = String.Format("Insert into cancles (`date`,`time`,`ordid`) values('{0}','{1}','{2}')",date.ToString(),time.ToString(), oid.ToString());
            cmd.CommandText = sql;
            return ((cmd.ExecuteNonQuery() > 0) && (cancelOrder(oid)>0));

        }

        int fillOrder(Trade f)
        {
            //if (!readyToWrite()) return -1;
            string sql = String.Format("UPDATE orders SET filled = filled + '{0}' WHERE ordid = '{1}'", f.xsize.ToString(),f.id);
            cmd.CommandText = sql;
            return(cmd.ExecuteNonQuery());
        }

        int cancelOrder(long oid)
        {
            //if (!readyToWrite()) return -1;
            string sql = String.Format("UPDATE orders SET cancled = 1 WHERE ordid = '{0}'",oid.ToString());
            cmd.CommandText = sql;
            return(cmd.ExecuteNonQuery());
        }


        //读取数据库操作
        public DataSet getOrderSet()
        {
            string sql = "select * from orders";
            // 创建一个适配器
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            // 创建DataSet，用于存储数据.
            DataSet retSet = new DataSet();
            // 执行查询，并将数据导入DataSet.
            adapter.Fill(retSet,"orders");
            return retSet;
        }

        //读取数据库操作
        public DataSet getTradeSet()
        {
            string sql = "select * from trades";
            // 创建一个适配器
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            // 创建DataSet，用于存储数据.
            DataSet retSet = new DataSet();
            // 执行查询，并将数据导入DataSet.
            adapter.Fill(retSet, "trades");
            return retSet;
        }


        //查询某个账户 某个段时间 某个symbol的委托记录
        public DataSet getOrderSet(string account,int starttime,int endtime,string symbol)
        {
            string sql = "select * from orders";
            // 创建一个适配器
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            // 创建DataSet，用于存储数据.
            DataSet retSet = new DataSet();
            // 执行查询，并将数据导入DataSet.
            adapter.Fill(retSet, "orders");
            return retSet;
        }

        public DataSet getAccounts()
        {
            string sql = "select * from accounts";
            // 创建一个适配器
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            // 创建DataSet，用于存储数据.
            DataSet retSet = new DataSet();
            // 执行查询，并将数据导入DataSet.
            adapter.Fill(retSet, "accounts");
            return retSet;
        
        }

        public bool validAccount(string acc, string pass)
        {
            string sql = String.Format("select * from accounts where `account` = '{0}'", acc);

            cmd.CommandText = sql;
            MySqlDataReader myReader = cmd.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    return pass.Equals(myReader.GetString("pass"));
                }
            }
            finally
            {
                myReader.Close();
                
            }
            return false;
        }
    }
}
