using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common
{
    class Utility
    {
        public static double GetDouble(string valueString)
        {
            double valueDouble = 0;
            double.TryParse(valueString, out valueDouble);
            return valueDouble;
        }
    }
}
