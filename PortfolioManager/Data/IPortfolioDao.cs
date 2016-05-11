using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.ComponentModel;

namespace PortfolioManager
{
    public interface IPortfolioDao
    {
        PortfolioDataEntity CreatePortfolioDataEntity();
        BindingList<PortfolioDataEntity> GetAllPortfolioItems();
        PortfolioAggregate GetPortfolioAggregate();
        void Save(PortfolioDataEntity portfolio);
        void Save(String symbol, MarketDataEntity mktData);
        PortfolioDataEntity GetBySymbol(string symbol);
    }
}
