using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PortfolioManager
{
    public interface IPortfolioView
    {
        void ShowPortfolioItems(BindingList<PortfolioDataEntity> PortfolioItemsList);
        void ShowPortfolioAggregate(PortfolioAggregate portfolioAggregate);
        void UpdatePortfolioItems();
    }
}
