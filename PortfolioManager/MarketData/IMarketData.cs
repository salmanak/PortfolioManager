using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using PortfolioManager.Common;

namespace PortfolioManager.MarketData
{
    public abstract class IMarketData:ObservableObject<MarketDataEntity>
    {
        abstract public void Start();
        abstract public void Stop();
        abstract public void Subscribe(string symbol);
        abstract public void UnSubscribe(string symbol);
    }
}
