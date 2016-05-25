using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManager.Common.Interfaces.MarketData
{
    /// <summary>
    /// interface to be implemented by Service Client handling only Snap Shot market data
    /// </summary>
    public interface IServiceClientSnapShot
    {
        #region Snapshot Quotes Access Methods
        IMarketDataEntity GetQuote(string symbol);
        void GetQuotes(Dictionary<string, double> symbols);
        #endregion
    }
}
