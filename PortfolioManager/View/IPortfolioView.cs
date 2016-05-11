using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PortfolioManager.Data;

namespace PortfolioManager.View
{
    /// <summary>
    /// Interface for the View
    /// </summary>
    public interface IPortfolioView
    {
        #region Methods
        /// <summary>
        /// To bind with the Portfolio Items
        /// </summary>
        /// <param name="PortfolioItemsList"></param>
        void ShowPortfolioItems(BindingList<PortfolioDataEntity> PortfolioItemsList);
        /// <summary>
        /// to bind with the Aggregate object
        /// </summary>
        /// <param name="portfolioAggregate"></param>
        void ShowPortfolioAggregate(PortfolioAggregate portfolioAggregate);
        /// <summary>
        /// to reset the binding
        /// </summary>
        void UpdatePortfolioItems(); 
        #endregion
    }
}
