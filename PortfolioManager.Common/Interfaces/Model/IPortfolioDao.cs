using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace PortfolioManager.Common.Interfaces.Model
{
    /// <summary>
    /// Interface to represent the Model
    /// </summary>
    public interface IPortfolioDao
    {
        #region Methods

        #region Initialization Methods
        /// <summary>
        /// concrete class should load the data from persisted storage
        /// </summary>
        void Load();
        /// <summary>
        /// Creates the portfolio object
        /// </summary>
        IPortfolioDataEntity CreatePortfolioDataEntity();
        #endregion

        #region Binding Methods
        /// <summary>
        /// Returns all the portfolio items
        /// </summary>
        BindingList<IPortfolioDataEntity> GetAllPortfolioItems();
        /// <summary>
        /// Provides aggregate access to the view
        /// </summary>
        IPortfolioAggregate GetPortfolioAggregate();
        /// <summary>
        /// Returns the portfolio item by name
        /// </summary>
        /// <param name="symbol">Symbol to search for</param>
        IPortfolioDataEntity GetBySymbol(string symbol);
        #endregion

        #region Save Methods
        /// <summary>
        /// Saves the portfolio trade in cache
        /// </summary>
        /// <param name="portfolio">the portfolio item to save</param>
        /// <param name="persist">persist in DB or not</param>
        void Save(IPortfolioDataEntity portfolio);
        /// <summary>
        /// Saves  market data for portfolio in cache
        /// </summary>
        /// <param name="symbol">Symbol of portfolio</param>
        /// <param name="mktData">Market data to be saved</param>
        void Save(String symbol, IMarketDataEntity mktData);
        #endregion

        #region Exception Handling Methods
        /// <summary>
        /// Method to save the exception handler reference 
        /// which can be used to notify GUI from non-GUI thread
        /// </summary>
        /// <param name="exceptionHandler">exception handler to notify GUI for exception</param>
        void SetExceptionHandler(IExceptionHandler exceptionHandler);
        #endregion

        #endregion
    }
}
