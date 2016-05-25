using System;
using System.ComponentModel;
namespace PortfolioManager.Common.Interfaces.Model
{
    public interface IPortfolioAggregate:INotifyPropertyChanged
    {
        double CostAggregate { get; set; }
        double UnrealizedGainAggregate { get; set; }
    }
}
