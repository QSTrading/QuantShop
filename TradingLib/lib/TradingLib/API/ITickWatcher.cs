using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradeLink.API;

namespace TradingLib.API
{
    //定义了不同客体的观察者,系统中我们需要生成不同的窗体,组件用于动态的现实当前的报价,图表,仓位,委托,成交等。
    //对于这些数据的变化我们利用观察接口去完成.中心引擎只需要当有特定数据到达时候调用注册的观察者的接口即可达到通知到观察者的目的。
    public interface ITickWatcher
    {
        //用于响应tick数据 观察者注册实现该接口，使用时注册到被观察者通知列表，被观察者有需要通知的数据时便统一通知列表中的所有观察者
        void GotTick(Tick t);
    }
}
