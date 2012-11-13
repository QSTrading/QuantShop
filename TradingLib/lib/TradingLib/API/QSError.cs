using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.API
{
    public class QSNoServerException:Exception
    {
    }

    public class QSAsyncServerError : Exception
    { 
    
    }

    //AsyncClient端错误
    public class QSAsyncClientError : Exception
    { }

}
