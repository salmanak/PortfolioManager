using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Common;

namespace PortfolioManager.MarketData.ServiceClients.Yahoo.ObjectModel
{
    public class Quote
    {
        public Quote(string symbol,double lastPrice)
        {
            this.symbol = symbol;
            this.lastPrice = lastPrice;
        }
        public Quote(string symbol, string lastPrice)
        {
            this.symbol = symbol;
            this.lastPrice = Utility.GetDouble(lastPrice);
        }

        public string symbol { get; set; }
        public double lastPrice { get; set; }
        public static Quote FromString(string item)
        {
            if (string.IsNullOrEmpty(item))
                return null;

            string[] arr = item.Split(',');

            string symbol = arr[0].Replace('"', ' ').Trim();
            string lastPrice = arr[1].Replace('"', ' ').Trim();

            return new Quote(symbol, lastPrice);
        }
    }
}
