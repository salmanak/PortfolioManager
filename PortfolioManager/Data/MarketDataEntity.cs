using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Data
{
    public class MarketDataEntity
    {

        public MarketDataEntity(string symbol, double lastPrice)
        {
            this.lastPrice = lastPrice;
            this.symbol = symbol;
        }

        /// <summary>
        /// Symbol for the quote
        /// </summary>
        private string symbol;

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        /// <summary>
        /// Current price for the symbol
        /// </summary>
        private double lastPrice;

        public double LastPrice
        {
            get { return lastPrice; }
            set { lastPrice = value; }
        }


    }
}
