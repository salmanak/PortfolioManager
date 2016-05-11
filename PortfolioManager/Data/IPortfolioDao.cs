using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.ComponentModel;

namespace PortfolioManager.Data
{
    /// <summary>
    /// Interface to represent the Model
    /// </summary>
    public interface IPortfolioDao
    {
        #region Methods
        /// <summary>
        /// Creates the portfolio object
        /// </summary>
        PortfolioDataEntity CreatePortfolioDataEntity();

        /// <summary>
        /// Returns all the portfolio items
        /// </summary>
        BindingList<PortfolioDataEntity> GetAllPortfolioItems();
        /// <summary>
        /// Provides aggregate access to the view
        /// </summary>
        PortfolioAggregate GetPortfolioAggregate();
        /// <summary>
        /// Returns the portfolio item by name
        /// </summary>
        /// <param name="symbol">Symbol to search for</param>
        PortfolioDataEntity GetBySymbol(string symbol);

        /// <summary>
        /// Saves the portfolio trade in cache
        /// </summary>
        /// <param name="portfolio">the portfolio item to save</param>
        void Save(PortfolioDataEntity portfolio);
        /// <summary>
        /// Saves  market data for portfolio in cache
        /// </summary>
        /// <param name="symbol">Symbol of portfolio</param>
        /// <param name="mktData">Market data to be saved</param>
        void Save(String symbol, MarketDataEntity mktData); 
        #endregion
    }
}
