using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace PortfolioManager
{
    public class PortfolioDataEntity : INotifyPropertyChanged
    {
        public PortfolioDataEntity(String symbol, long shares, double price)
        {
            this.Symbol = symbol;
            this.Shares = shares;
            this.Price = price;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public PortfolioDataEntity()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private String symbol;
        /// <summary>
        /// Symbol of the portfolio item
        /// </summary>
        public String Symbol
        {
            get { return this.symbol; }
            set { this.symbol = value; NotifyPropertyChanged("Symbol"); }
        }

        private long shares;
        /// <summary>
        /// Number of shares owned for the symbol
        /// </summary>
        public long Shares
        {
            get { return this.shares; }
            set { this.shares = value; NotifyPropertyChanged("Shares"); }
        }

        private double price;
        /// <summary>
        /// Weighted Average Price of the Symbol in portfolio
        /// </summary>
        public double Price
        {
            get { return this.price; }
            set { this.price = value; NotifyPropertyChanged("Price"); }
        }

        private double cost;
        /// <summary>
        /// Total cost of the shares owned for the symbol
        /// </summary>
        public double Cost
        {
            get { return this.cost; }
            set { this.cost = value; NotifyPropertyChanged("Cost"); }
        }

        private double marketPrice;
        /// <summary>
        /// Current MarketValue Price for the Symbol
        /// </summary>
        public double MarketPrice
        {
            get { return this.marketPrice; }
            set { this.marketPrice = value; NotifyPropertyChanged("MarketPrice"); }
        }

        private double marketValue;
        /// <summary>
        /// Market Value for the shares based on the current Market Price
        /// </summary>
        public double MarketValue
        {
            get { return this.marketValue; }
            set { this.marketValue = value; NotifyPropertyChanged("MarketValue"); }
        }

        private double unrealizedGain;
        /// <summary>
        /// UnRealized Profit/Loss based on the cost basis and current market value
        /// </summary>
        public double UnrealizedGain
        {
            get { return this.unrealizedGain; }
            set { this.unrealizedGain = value; NotifyPropertyChanged("UnrealizedGain"); }
        }
    }

}
