using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PortfolioManager.Common.Interfaces.Model;


namespace PortfolioManager.Model
{
    /// <summary>
    /// data access object to hold the data for Portfolio/Trade entry
    /// </summary>
    public class PortfolioDataEntity : INotifyPropertyChanged, IPortfolioDataEntity
    {

        #region Declarations and Definitions
        private String _symbol;
        /// <summary>
        /// Symbol of the portfolio item
        /// </summary>
        public String Symbol
        {
            get { return this._symbol; }
            set { this._symbol = value; NotifyPropertyChanged("Symbol"); }
        }

        private long _shares;
        /// <summary>
        /// Number of _shares owned for the _symbol
        /// </summary>
        public long Shares
        {
            get { return this._shares; }
            set { this._shares = value; NotifyPropertyChanged("Shares"); }
        }

        private double _price;
        /// <summary>
        /// Weighted Average Price of the Symbol in portfolio
        /// </summary>
        public double Price
        {
            get { return this._price; }
            set { this._price = value; NotifyPropertyChanged("Price"); }
        }

        private double _cost;
        /// <summary>
        /// Total _cost of the _shares owned for the _symbol
        /// </summary>
        public double Cost
        {
            get { return this._cost; }
            set { this._cost = value; NotifyPropertyChanged("Cost"); }
        }

        private double _marketPrice;
        /// <summary>
        /// Current MarketValue Price for the Symbol
        /// </summary>
        public double MarketPrice
        {
            get { return this._marketPrice; }
            set { this._marketPrice = value; NotifyPropertyChanged("MarketPrice"); }
        }

        private double _marketValue;
        /// <summary>
        /// Market Value for the _shares based on the current Market Price
        /// </summary>
        public double MarketValue
        {
            get { return this._marketValue; }
            set { this._marketValue = value; NotifyPropertyChanged("MarketValue"); }
        }

        private double _unrealizedGain;
        /// <summary>
        /// UnRealized Profit/Loss based on the _cost basis and current market value
        /// </summary>
        public double UnrealizedGain
        {
            get { return this._unrealizedGain; }
            set { this._unrealizedGain = value; NotifyPropertyChanged("UnrealizedGain"); }
        } 
        #endregion

        #region Constructors
        /// <summary>
        /// default constructor
        /// </summary>
        public PortfolioDataEntity()
        {

        }
        /// <summary>
        /// constructor for Trade
        /// </summary>
        public PortfolioDataEntity(String symbol, long shares, double price)
        {
            this.Symbol = symbol;
            this.Shares = shares;
            this.Price = price;
        } 
        #endregion

        #region INotifyPropertyChanged Handler
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
