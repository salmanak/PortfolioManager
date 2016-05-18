using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Data
{
    /// <summary>
    /// Data Access object to hold the Market Data
    /// </summary>
    public class MarketDataEntity
    {
        #region Constructors
        /// <summary>
        /// Constructor with Symbol and Last Price
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="lastPrice"></param>
        public MarketDataEntity(string symbol, double lastPrice)
        {
            this._lastPrice = lastPrice;
            this._symbol = symbol;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="mkt">object from which the values need to be copied</param>
        public MarketDataEntity(MarketDataEntity mkt)
        {
            this._lastPrice = mkt.LastPrice;
            this._symbol = mkt.Symbol;
        } 
        #endregion

        #region Declarations and Definitions
        /// <summary>
        /// Symbol for the quote
        /// </summary>
        private string _symbol;

        public string Symbol
        {
            get { return _symbol; }
            set { _symbol = value; }
        }
        /// <summary>
        /// Current _price for the _symbol
        /// </summary>
        private double _lastPrice;
        private MarketDataEntity mkt;

        public double LastPrice
        {
            get { return _lastPrice; }
            set { _lastPrice = value; }
        } 
        #endregion


    }
}
