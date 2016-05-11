using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.ComponentModel;

namespace PortfolioManager
{
    public class PortfolioDao:IPortfolioDao
    {
        /// <summary>
        /// List of all protfolio items
        /// </summary>
        private BindingList<PortfolioDataEntity> m_PortfolioItems = new BindingList<PortfolioDataEntity>();
        private PortfolioAggregate m_PortfolioAggregate = new PortfolioAggregate();

        public PortfolioDataEntity CreatePortfolioDataEntity()
        {
            return new PortfolioDataEntity();
        }

        public BindingList<PortfolioDataEntity> GetAllPortfolioItems()
        {
            return m_PortfolioItems;
        }

        public PortfolioAggregate GetPortfolioAggregate()
        {
            return m_PortfolioAggregate;
        }

        public void Save(String symbol, MarketDataEntity mktData)
        {
            var result = m_PortfolioItems.Where(p => p.Symbol == symbol).FirstOrDefault();
            if (result != null)
            {
                result.MarketPrice = Math.Round(mktData.LastPrice,2);
                result.MarketValue = Math.Round(result.Shares * mktData.LastPrice,2);
                result.UnrealizedGain = Math.Round(result.MarketValue - result.Cost, 2);
            }
            else
            {
                // Should not happen
            }

            m_PortfolioAggregate.UnrealizedGainAggregate =  m_PortfolioItems.Sum(s => s.UnrealizedGain);
        }

        public void Save(PortfolioDataEntity portfolio)
        {
            var result = m_PortfolioItems.Where(p => p.Symbol == portfolio.Symbol).FirstOrDefault();
            if (result != null)
            {
                double AvgPx;
                long CumQty;

                CumQty = result.Shares + portfolio.Shares;
                AvgPx = Math.Round((((result.Shares*result.Price) + (portfolio.Shares*portfolio.Price))/CumQty),2);


                result.Symbol = portfolio.Symbol;

                result.Price = AvgPx;
                result.Shares = CumQty;
                result.Cost = CumQty*AvgPx;

                result.MarketPrice = portfolio.MarketPrice;
                result.MarketValue = portfolio.MarketValue;
                result.UnrealizedGain = portfolio.UnrealizedGain;
            }
            else
            {
                portfolio.Cost = portfolio.Shares * portfolio.Price;

                m_PortfolioItems.Add(portfolio);
            }

            m_PortfolioAggregate.CostAggregate = m_PortfolioItems.Sum(s => s.Cost);
        }

        public PortfolioDataEntity GetBySymbol(string symbol)
        {
            return m_PortfolioItems.FirstOrDefault(p => p.Symbol == symbol);
        }
    }

    public class PortfolioAggregate
    {

        public PortfolioAggregate()
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

        private double costAggregate;
        /// <summary>
        /// Total cost of the shares owned for the symbol
        /// </summary>
        public double CostAggregate
        {
            get { return this.costAggregate; }
            set { this.costAggregate = value; NotifyPropertyChanged("CostAggregate"); }
        }

        private double unrealizedGainAggregate;
        /// <summary>
        /// UnRealized Profit/Loss based on the cost basis and current market value
        /// </summary>
        public double UnrealizedGainAggregate
        {
            get { return this.unrealizedGainAggregate; }
            set { this.unrealizedGainAggregate = value; NotifyPropertyChanged("UnrealizedGainAggregate"); }
        }
    }
}
