using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Data;
using TradeLink.API;
using TradeLink.Common;
using TradingLib.GUI;
using System.Reflection;
using TradeLink.AppKit;

namespace TradingLib.Core
{
    //关于数据驱动
    //策略如果没有生效,那么我们tick数据就没有必要由策略去运算.只有当策略被激活的时候 策略才可以相应数据
    //仓位策略运行的容器,仓位策略激活，修改的内存数据均在这里进行保存并应用
    //当某个symbol有仓位时,右键出现的可用策略也是从这里获取
    //
    public class PositionCheckCentre 
    {
        public event DebugDelegate SendDebugEvent;
        private Basket _defaultBasket;
        private CoreCentre _coreCentre;
        private TradingTrackerCentre _ttc;

        private bool handleresponseexception=true;
        public bool HandleResponseException { get { return handleresponseexception; } set { handleresponseexception = value; } }
        public bool disableresponseonexception = false;
        public bool DisableResponseOnException { get { return disableresponseonexception; } set { disableresponseonexception = value; } }
        //strategyTemple数据结构
        //名称(类的全名 命名空间+类名)与策略类型的对应关系
        private Dictionary<string, Type> poscheckTypeMap = new Dictionary<string, Type>();
        //类型到某个类型positioncheck的中文名 
        private Dictionary<Type, string> poscheckTypeTitleMap = new Dictionary<Type, string>();
        //中文名称->类型的对应 用于菜单调用时候的策略索引
        private Dictionary<string, Type> poscheckTitleTypeMap = new Dictionary<string, Type>();

        //response实例数据结构
        //用于储存response列表,其他对response的调用均通过列表与ID进行
        private List<Response> _reslist = new List<Response>();
        //从配置文件加载上来的string - >仓位策略Response之间的映射关系
        private Dictionary<string, List<int>> symPositionCheckMap = new Dictionary<string, List<int>>();
        //每个response中文名(参数名) - response的映射关系 用于从菜单标题缩影到该Response然后进行操作
        private Dictionary<string,int> _menuTitle2Idx = new Dictionary<string,int>();
        //private Dictionary<string,>
        private Dictionary<int, string> _Idx2menuTitle = new Dictionary<int, string>();
        private Dictionary<int, string> _Idx2Symbol = new Dictionary<int, string>();

        //本地list index与response ID的映射关系
        private Dictionary<int,int> _rid2LocalIdx = new Dictionary<int,int>();
        //private Dictionary<int,string>


        const int MAXRESPONSEPERASP = 100;
        const int MAXASPINSTANCE = 1;
        int _ASPINSTANCE = 0;
        int _INITIALRESPONSEID = 0;
        int _NEXTRESPONSEID = 0;

        //仓位检查策略 构造时需要提供CoreCentre TradingTrackerCentere
        public PositionCheckCentre(CoreCentre cc,TradingTrackerCentre ttc)
        {
            _coreCentre = cc;
            _ttc = ttc;
            // count instances of program
            _ASPINSTANCE =  1;
            // set next response id
            _NEXTRESPONSEID = _ASPINSTANCE * MAXRESPONSEPERASP;
            _INITIALRESPONSEID = _NEXTRESPONSEID;

            InitPositionCheckCentre();


        }
        private void debug(string msg)
        {
            if (SendDebugEvent != null)
                SendDebugEvent(msg);
        }

        
        

        
        //关闭或者打开某个response
        public void switchResponse(string menutitle)
        {
            //debug("switch r:" + menutitle);
            switchResponse(menuTitle2LocalIdx(menutitle));
        }
        public void switchResponse(int idx)
        {
            
            if (!isBadResponse(idx))
            {

                bool valid = !_reslist[idx].isValid;
                if (valid)
                {
                    //将最新的position信息以及bar信息绑定给positioncheck
                    if (((IPositionCheck)_reslist[idx]).BarListTracker==null)
                        ((IPositionCheck)_reslist[idx]).BarListTracker = _ttc.BarListTracker;
                    //debug("why why");
                    if (((IPositionCheck)_reslist[idx]).myPosition==null)
                    ((IPositionCheck)_reslist[idx]).myPosition = _ttc.getPosition(Idx2Symbol(idx));//这里会触发tradingtracker got a position需要查看下函数是如何运行的
                    ((PositionCheckTemplate)_reslist[idx]).DataManager = _ttc.DataManager;

                    //先初始化策略 然后再设置为valid
                    _reslist[idx].Reset();
                    _reslist[idx].isValid = valid;
                }
                else
                {
                    //((IPositionCheck)_reslist[idx]).BarListTracker = null;
                    //((IPositionCheck)_reslist[idx]).myPosition = null;
                    _reslist[idx].Shutdown();
                    _reslist[idx].isValid = valid;
                }
                debug(_reslist[idx].Name +" ["+LocalIdx2MenuTitle(idx) +"] " + (valid ? "激活." : "关闭."));
            }
        }
        //response index转换到对应的symbol
        private string Idx2Symbol(int idx)
        {
            string s = string.Empty;
            if (_Idx2Symbol.TryGetValue(idx, out s))
                return s;
            return string.Empty;
        }
        //response分配的ID到本地index的转换
        private int rid2localIdx(long responseid)
        {
            int idx = -1;
            if (_rid2LocalIdx.TryGetValue((int)responseid, out idx))
                return idx;
            return -1;
        }
        //菜单标题项到本地index的转换
        private int menuTitle2LocalIdx(string menutitle)
        {
            int idx = -1;
            if (_menuTitle2Idx.TryGetValue(menutitle, out idx))
                return idx;
            return -1;
        }
        //本地index项到菜单项的转换
        private string LocalIdx2MenuTitle(int idx)
        {
            string s = string.Empty;
            if (_Idx2menuTitle.TryGetValue(idx, out s))
                return s;
            return string.Empty;
        }
        //检查某个response index是否有效
        private bool isBadResponse(int idx)
        {
            return ((idx < 0) || (idx >= _reslist.Count) || (_reslist[idx] == null));// || !_reslist[idx].isValid);
        }
        //当策略出现异常的时候 我们执行通知
        void notifyresponseexception(int idx, int time, string ondata, Exception ex)
        {
            string ridn = _reslist[idx].ID + " " + _reslist[idx].FullName + " at: " + time + " on: " + ondata;
            debug(ridn + " had a user code error: " + ex.Message + ex.StackTrace + ".  Purchase a support contract at http://www.pracplay.com or ask community at http://community.tradelink.org if you need help resolving your error.");
            if (disableresponseonexception)
            {
                _reslist[idx].isValid = false;
                debug(ridn + " was marked invalid because of user code error.");
            }
        }

        #region response 方法 事件 处理
        //往本地reslist增加一个response实例,并分配virtural ID
        private int addResponse(Response r)
        {
            int id = _NEXTRESPONSEID;
            try
            {
                // set the id
                r.ID = id;
                // ensure it has a full name
                r.Name = r.GetType().Name;
                r.FullName = r.GetType().FullName;

                //if (r.FullName == string.Empty)
                //    r.FullName = r.GetType().FullName;
                // get local response index
                int idx = _reslist.Count;//获得本地reslist的idx序列 默认将response加到list列表中
                //绑定response事件 从而response可以触发事件
                bindResponsEevents(r);
                //将response加入到本地response列表
                lock (_reslist)
                {
                    _reslist.Add(r);
                }

                // prepare log entry for it
                //_indlog.Add(null);
                // save id to local relationship
                _rid2LocalIdx.Add(r.ID, idx);
                //递增responseid
                _NEXTRESPONSEID++;
                return idx;//返回本地indx用于绑定其他映射
            }
            catch (Exception ex)
            {
                return -1;
            }
            


        }
        //绑定response事件
        private void bindResponsEevents(Response tmp)
        {
            // handle all the outgoing events from the response
            tmp.SendOrderEvent += new OrderSourceDelegate(r_SendOrder);
            tmp.SendDebugEvent += new DebugDelegate(r_GotDebug);
            tmp.SendCancelEvent += new LongSourceDelegate(r_CancelOrderSource);
            tmp.SendBasketEvent += new BasketDelegate(r_SendBasket);
            tmp.SendMessageEvent += new MessageDelegate(r_SendMessage);
            tmp.SendChartLabelEvent += new ChartLabelDelegate(r_SendChartLabel);
            tmp.SendIndicatorsEvent += new ResponseStringDel(r_SendIndicators);
            tmp.SendTicketEvent += new TicketDelegate(r_SendTicketEvent);
        }

        #region Response输入操作
        //当服务器返回fill回报时进行的操作
        public void GotFill(Trade t)
        {
            // track results
            //_rs.GotFill(t);
            // keep track of position
            //_pt.Adjust(t);

            // send trade notification to any valid requesting responses
            //
            string sym = t.symbol;
            List<int> res = new List<int>();
            //找到对应symbol所对应的response列表
            if(symPositionCheckMap.TryGetValue(sym,out res))
            {
                foreach (int i in res)
                {
                    //_reslist[i].GotFill(t);
                }
            }

        }
        //当服务端返回order取消回报
        void tl_gotOrderCancel(long number)
        {
            // send order cancel notification to every valid box
            for (int idx = 0; idx < _reslist.Count; idx++)
                if (!isBadResponse(idx))
                {
                    if (handleresponseexception)
                    {
                        try
                        {
                            _reslist[idx].GotOrderCancel(number);
                        }
                        catch (Exception ex)
                        {
                            //notifyresponseexception(idx, _lasttime, "cancel: " + number, ex);
                        }
                    }
                    else
                        _reslist[idx].GotOrderCancel(number);
                }
        }
        //当服务端返回Tick信息
        public void GotTick(Tick k)
        {
            try
            {
                //获得与tick symbol 对应的response id然后用于执行驱动
                List<int> l;
                //查找策略缓存中是否有对应symbol的策略
                if (!symPositionCheckMap.TryGetValue(k.symbol, out l)) return;//没有对应的策略列表
                if (l.Count == 0) return;//有对应的策略列表 列表中没有具体的策略 返回
                foreach (int i in l) //遍历每个index对应的策略response并用Tick进行驱动
                {
                    //只有当response有些并且状态为valid的时候我们才用数据区驱动该response
                    if (!isBadResponse(i) && _reslist[i].isValid)
                    {
                        _reslist[i].GotTick(k);
                    }
                    //debug("tick arrived into positioncheck:" + k.ToString());
                }
            }
            catch (Exception ex)
            {
                debug(ex.ToString());
            }
        }


        #endregion

        #region Response输出操作
        //response对外委托Order
        void r_SendOrder(Order o, int id)
        {
            int rid = rid2localIdx(id);
            if (rid < 0)
            {
                debug("Ignoring order from response with invalid id: " + id + " index not found. order: " + o.ToString());
                return;
            }
            if (!_reslist[rid].isValid)
            {
                debug("Ignoring order from disabled response: " + _reslist[rid].Name + " order: " + o.ToString());
                return;
            }
            //if (_enableoversellprotect)
            //    _ost.sendorder(o);
            else
                _coreCentre.SendOrder(o);
               
        }
        //response对外取消Order
        void r_CancelOrderSource(long number, int id)
        {
            int rid = rid2localIdx(id);
            if (!_reslist[rid].isValid)
            {
                debug("Ignoring cancel from disabled response: " + _reslist[rid].Name + " orderid: " + number);
                return;
            }
            // pass cancels along to tradelink
            _coreCentre.CancelOrder((long)number);
            
        }
        //response发送debug信息输出
        void r_GotDebug(string msg)
        {
            // display to screen
            debug(msg);
        }
        //发送basket信息
        void r_SendBasket(Basket b, int id)
        {
            // get storage index of response from response id
            int idx = rid2localIdx(id);
            // update symbols for response
            //newsyms(b.ToString().Split(','), idx);
        }

        void r_SendTicketEvent(string space, string user, string password, string summary, string description, Priority pri, TicketStatus stat)
        {
            //_rt.Track(space, user, password, summary, description, pri, stat);
        }

        void r_SendMessage(MessageTypes type, long source, long dest, long msgid, string request, ref string response)
        {
            //_bf.TLSend(type, source, dest, msgid, request, ref response);
        }

        //bool _inderror = false;
        //RingBuffer<inddata> bufind = new RingBuffer<inddata>(5000);
        //从response得到输出的调式结果
        void r_SendIndicators(int idx, string param)
        {
            //if (!_ao._saveinds.Checked)
            //    return;
            //bufind.Write(new inddata(idx, param));
            debug(idx.ToString() + ":" + param);
        }

        bool _charterror = false;
        void r_SendChartLabel(decimal price, int bar, string label, System.Drawing.Color c)
        {
            if (!_charterror)
            {
                //debug(PROGRAM + " does not support sendchart.");
                _charterror = true;
            }
        }
        #endregion

        #endregion



        //获得某个空的Response instance
        //通过type全名来得到response instance
        public Response getPositionStrategyInstanceByFullName(string typefullname)
        { 
            Type t =null;
            if(poscheckTypeMap.TryGetValue(typefullname,out t))
                return (Response)Activator.CreateInstance(t,new object[] { });
            return null;
        }
        //通过positioncheck的中文标题名称来获得对应的response instance
        public Response getPositionStrategyInstanceByTitle(string cnname)
        {
            Type t = null;
            if(poscheckTitleTypeMap.TryGetValue(cnname,out t))
                return (Response)Activator.CreateInstance(t, new object[] { });
            return null;
        }

        //获得仓位策略模板
        public List<Type> getPositionStrategyTemple()
        {   
            return poscheckTypeMap.Values.ToList();
        }

        //更新某个sym对应的response列表
        public void updateSymbolPositionStrategy(string sym,List<Response> rlist)
        {
            foreach (Response r in rlist)
            {
                addResponseIntoCache(sym, r);
            } 
        }

        //根据response返回其中文标题
        public string getPositionStrategyTitle(Response r)
        {
            return getPositionStrategyTitle(r.GetType());
        }

        public string getPositionStrategyTitle(Type t)
        {
            string s = string.Empty;
            if (poscheckTypeTitleMap.TryGetValue(t, out s))
                return s;
            return s;
        }

        //通过symbol 返回策略id list.或者null无策略集
        public List<int> getResponseIdxViaSymbol(string sym)
        {
           
            List<int> a;
            if (symPositionCheckMap.TryGetValue(sym, out a))
                return a;
            return null;
            
        }

        //返回某个symbol所存在的positioncheck strategy
        public List<Response> getPositionStrategy(string sym)
        {
            List<Response> res = new List<Response>();
            List<int> a  = new List<int>();
            if(!symPositionCheckMap.TryGetValue(sym,out a))
                return res;
            foreach (int i in a)
            {
                res.Add(_reslist[i]);
            }
            return res;
        }

        public void InitPositionCheckCentre()
        {
            //加载策略模板(用于通过全名得到Type进而实例化成可运行的策略)
            LoadPositionCheckStrategyTemple();
            //加载symbol对应的配置好的可运行策略,通过加载配置文件将默认策略配置成该Symbol特有的策略
            LoadPositionStrategyForSymbols();
        }

        //加载所有的positioncheckstrategy
        private void LoadPositionCheckStrategyTemple()
        {
            //Dictionary<string, Type> tmp = new Dictionary<string, Type>();
            foreach (Type t in StrategyHelper.GetResponseListViaType<IPositionCheck>())
            {
                //object[] attributes = t.GetCustomAttributes(typeof(Tablename), false);
                //debug(t.ToString());
                //得到RuleSet的名称与描述
                string rsname = (string)t.InvokeMember("Title",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                string rsdescription = (string)t.InvokeMember("Description",
                    BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetProperty,
                    null, null, null);
                //debug(rsname+"|"+rsdescription);
                //dicRule.Add("RiskRuleSet" + "." + t.Name, t);
                //tmp.Add(rsname, t);
                //MessageBox.Show("RiskRuleSet" + "." + t.Name);
                //if (t.GetInterface("IRuleCheck") != null)
                //建立双向索引从类型-名称
                poscheckTypeMap.Add(t.FullName, t);//类型全名-类型
                poscheckTypeTitleMap.Add(t, rsname);//类型-类型中文名
                poscheckTitleTypeMap.Add(rsname, t);//类型中文名-类型

            }
        }


        //所加载的symbol均为default默认列表中的symbol
        private void LoadPositionStrategyForSymbols()
        {
            _defaultBasket = BasketTracker.getBasket("Default");
            foreach (string sym in _defaultBasket.ToSymArray())
            {
                LoadPositionStrategyForSymbol(sym);
            }
        }
        //将某个response从内存中去除
        public void delResponseFromCache(string sym, Response r)
        {
            //得到response的中文标题(参数列表) 该标识可以用于区分不同的response
            string s = getPositionStrategyTitle(r) + "(" + ((IPositionCheck)r).ToText() + ")";
            int idx = menuTitle2LocalIdx(s);
            //表明系统中symbol已经存在了该response不用重复添加
            //debug("the response Idx:" + idx.ToString());
            //表明系统中没有该response我们不去去除
            if (idx == -1)
                return;
            lock (_reslist)
            {
                _reslist[idx] = new InvalidResponse();
            }
            //从symbol list<int>中将该response删除
            lock (symPositionCheckMap[sym])
            {
                symPositionCheckMap[sym].Remove(idx);
            }
            lock (_menuTitle2Idx)
            {
                _menuTitle2Idx.Remove(s);
            }
            lock (_Idx2Symbol)
            {
                _Idx2menuTitle.Remove(idx);
            }
            lock (_Idx2Symbol)
            {
                _Idx2Symbol.Remove(idx);
            }


        }

        //将某个response加入到内存中
        public void addResponseIntoCache(string sym, Response r, bool valid)
        {
            if (valid)
            {
                int idx = addResponseIntoCache(sym, r);
                switchResponse(idx);
            }
        }
        public int  addResponseIntoCache(string sym, Response r)
        {
            //得到response的中文标题(参数列表) 该标识可以用于区分不同的response
            string s = getPositionStrategyTitle(r) + "(" + ((IPositionCheck)r).ToText() + ")";
            int idx = menuTitle2LocalIdx(s);
            //表明系统中symbol已经存在了该response不用重复添加
            if (idx >= 0)
                return idx;
            idx = addResponse(r);
            //将该idx与对应的symbol进行绑定
            if (!symPositionCheckMap.ContainsKey(sym))//如果映射列表中不存在该sym对应的list我们先增加该list
                symPositionCheckMap.Add(sym, new List<int>());
            symPositionCheckMap[sym].Add(idx);
            try
            {
                //string s = getPositionStrategyTitle(rpc) + "(" + ((IPositionCheck)r).ToText() + ")";
                _menuTitle2Idx.Add(s, idx);
                _Idx2menuTitle.Add(idx, s);
                _Idx2Symbol.Add(idx, sym);
            }
            catch (Exception ex)
            {
                debug(ex.ToString());
            }
            //debug("ID:" + idx.ToString() + " menu name:" + s.ToString());
            return idx;
        }
        //加载单个symbol下的positionstrategy
        private void LoadPositionStrategyForSymbol(string sym)
        { 
            List<string> l = PositionCheckTracker.getPositionCheckFromSymbol(sym);
            //List<Response> res = new List<Response>();
            
            for (int i = 0; i < l.Count; i++)
            {
                object[] args;
                args = new object[] { };
                string[] re = l[i].Split(':');
                string rname = re[0];
                string cfgtest = re[1];
                //Type t = 
                Response rpc = ((IPositionCheck)Activator.CreateInstance(poscheckTypeMap[rname], args)).FromText(cfgtest) as Response;
                //将从配置文件实例化得到的response加载到系统,初始情况均为不激活
                rpc.isValid = false;
                this.addResponseIntoCache(sym, rpc);

               /*
                int idx = addResponse(rpc);
                //将该idx与对应的symbol进行绑定
                if (!symPositionCheckMap.ContainsKey(sym))//如果映射列表中不存在该sym对应的list我们先增加该list
                    symPositionCheckMap.Add(sym,new List<int>());
                symPositionCheckMap[sym].Add(idx);

                //res.Add(rpc);
                try
                {
                    string s = getPositionStrategyTitle(rpc) + "(" + ((IPositionCheck)rpc).ToText() + ")";
                    _menuTitle2Idx.Add(s, idx);
                    _Idx2menuTitle.Add(idx, s);
                    _Idx2Symbol.Add(idx, sym);
                }
                catch (Exception ex)
                {
                    debug(ex.ToString());
                }
                * */

            }
        }
    
    }
}
