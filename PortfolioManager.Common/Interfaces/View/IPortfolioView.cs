using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManager.Common.Interfaces.View
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
        void ShowPortfolioItems(BindingList<IPortfolioDataEntity> PortfolioItemsList);
        /// <summary>
        /// to bind with the Aggregate object
        /// </summary>
        /// <param name="IPortfolioAggregate"></param>
        void ShowPortfolioAggregate(IPortfolioAggregate IPortfolioAggregate);
        /// <summary>
        /// to reset the binding
        /// </summary>
        void UpdatePortfolioItems(); 
        #endregion
    }
}
