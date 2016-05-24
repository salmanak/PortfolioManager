using System;
namespace PortfolioManager.Common.Interfaces.Model
{
    public interface IMarketDataEntity
    {
        double LastPrice { get; set; }
        string Symbol { get; set; }
    }
}
