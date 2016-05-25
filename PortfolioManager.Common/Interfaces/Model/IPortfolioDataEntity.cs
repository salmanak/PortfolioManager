using System;
using System.ComponentModel;
namespace PortfolioManager.Common.Interfaces.Model
{
    public interface IPortfolioDataEntity:INotifyPropertyChanged
    {
        string Symbol { get; set; }
        long Shares { get; set; }
        double Price { get; set; }
        double Cost { get; set; }

        double MarketPrice { get; set; }

        double MarketValue { get; set; }
        double UnrealizedGain { get; set; }
    }
}
