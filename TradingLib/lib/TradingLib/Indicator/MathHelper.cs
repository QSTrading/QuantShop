using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;

namespace TradingLib.Indicator
{
    public class MathHelper
    {
        public static double Standard(ISeries dataLst, int start, int len, double avg)
        {
            if (((len <= 1) || (start < 0)) || (dataLst.Count < (start + len)))
            {
                return 0.0;
            }
            double d = 0.0;
            int num2 = start + len;
            for (int i = start; i < num2; i++)
            {
                double num4 = dataLst[i];
                d += (num4 - avg) * (num4 - avg);
            }
            d /= (double)len;
            return Math.Sqrt(d);
        }

 

    }
}
