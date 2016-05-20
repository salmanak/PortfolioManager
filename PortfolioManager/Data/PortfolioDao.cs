using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.ComponentModel;
using PortfolioManager.Common;

namespace PortfolioManager.Data
{
     /// <summary>
     /// Concrete Model class for portfolio
     /// </summary>
    public class PortfolioDao:IPortfolioDao
    {
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(PortfolioDao));
        /// <summary>
        /// List of all protfolio items
        /// </summary>
        private BindingList<PortfolioDataEntity> _portfolioItems = new BindingList<PortfolioDataEntity>();
        /// <summary>
        /// Portfolio Aggregate object binded with teh View
        /// </summary>
        private PortfolioAggregate _portfolioAggregate = new PortfolioAggregate(); 
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates the portfolio object
        /// </summary>
        public PortfolioDataEntity CreatePortfolioDataEntity()
        {
            return new PortfolioDataEntity();
        } 
        #endregion

        #region Access Methods
        /// <summary>
        /// Returns all the portfolio items
        /// </summary>
        public BindingList<PortfolioDataEntity> GetAllPortfolioItems()
        {
            return _portfolioItems;
        }
        /// <summary>
        /// Provides aggregate access to the view
        /// </summary>
        public PortfolioAggregate GetPortfolioAggregate()
        {
            return _portfolioAggregate;
        }
        /// <summary>
        /// Returns the portfolio item by name
        /// </summary>
        /// <param name="symbol">Symbol to search for</param>
        public PortfolioDataEntity GetBySymbol(string symbol)
        {
            return _portfolioItems.FirstOrDefault(p => p.Symbol == symbol);
        } 
        #endregion

        #region Save Methods
        /// <summary>
        /// Saves the portfolio trade in cache
        /// </summary>
        /// <param name="symbol">Symbol of portfolio</param>
        /// <param name="mktData">Market data to be saved</param>
        public void Save(String symbol, MarketDataEntity mktData)
        {
            var result = _portfolioItems.Where(p => p.Symbol == symbol).FirstOrDefault();
            if (result != null)
            {
                result.MarketPrice = Math.Round(mktData.LastPrice, 2);
                result.MarketValue = Math.Round(result.Shares * mktData.LastPrice, 2);
                result.UnrealizedGain = Math.Round(result.MarketValue - result.Cost, 2);
            }
            else
            {
                // Should not happen
            }
            
            _portfolioAggregate.UnrealizedGainAggregate = Math.Round(_portfolioItems.Sum(s => s.UnrealizedGain), 2);
        }

        /// <summary>
        /// Saves  market data for portfolio in cache
        /// </summary>
        /// <param name="portfolio">the portfolio item to save</param>
        public void Save(PortfolioDataEntity portfolio)
        {
            _logger.Log("Saving for _symbol : " + portfolio.Symbol);

            var result = _portfolioItems.Where(p => p.Symbol == portfolio.Symbol).FirstOrDefault();
            if (result != null)
            {
                double AvgPx;
                long CumQty;

                CumQty = result.Shares + portfolio.Shares;
                AvgPx = Math.Round((((result.Shares * result.Price) + (portfolio.Shares * portfolio.Price)) / CumQty), 2);


                result.Symbol = portfolio.Symbol;

                result.Price = AvgPx;
                result.Shares = CumQty;
                result.Cost = Math.Round(CumQty * AvgPx,2);

                result.MarketPrice = Math.Round(portfolio.MarketPrice,2);
                result.MarketValue = Math.Round(portfolio.MarketValue,2);
                result.UnrealizedGain = Math.Round(portfolio.UnrealizedGain,2);
            }
            else
            {
                portfolio.Cost = Math.Round(portfolio.Shares * portfolio.Price,2);

                _portfolioItems.Add(portfolio);
            }
            
            _portfolioAggregate.CostAggregate = Math.Round(_portfolioItems.Sum(s => s.Cost),2);
        } 
        #endregion

    }

    /// <summary>
    /// Data access object to hold aggregate data
    /// </summary>
    public class PortfolioAggregate : INotifyPropertyChanged
    {

        #region Declarations and Definitions
        private double _costAggregate;
        /// <summary>
        /// Total _cost of the _shares owned for the _symbol
        /// </summary>
        public double CostAggregate
        {
            get { return this._costAggregate; }
            set { this._costAggregate = value; NotifyPropertyChanged("CostAggregate"); }
        }

        private double _unrealizedGainAggregate;
        /// <summary>
        /// UnRealized Profit/Loss based on the _cost basis and current market value
        /// </summary>
        public double UnrealizedGainAggregate
        {
            get { return this._unrealizedGainAggregate; }
            set { this._unrealizedGainAggregate = value; NotifyPropertyChanged("UnrealizedGainAggregate"); }
        } 
        #endregion

        #region Constructors
        public PortfolioAggregate()
        {
        } 
        #endregion

        #region INotifyPropertyChange Handler
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        } 
        #endregion

    }

}
