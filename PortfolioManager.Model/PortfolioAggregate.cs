using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManager.Model
{
    /// <summary>
    /// Data access object to hold aggregate data
    /// </summary>
    public class PortfolioAggregate : INotifyPropertyChanged, IPortfolioAggregate
    {

        #region Declarations and Definitions
        private double _costAggregate;
        /// <summary>
        /// Total _cost of the _shares owned for the _symbol
        /// </summary>
        public double CostAggregate
        {
            get { return this._costAggregate; }
            set { this._costAggregate = value; NotifyPropertyChanged("CostAggregate"); }
        }

        private double _unrealizedGainAggregate;
        /// <summary>
        /// UnRealized Profit/Loss based on the _cost basis and current market value
        /// </summary>
        public double UnrealizedGainAggregate
        {
            get { return this._unrealizedGainAggregate; }
            set { this._unrealizedGainAggregate = value; NotifyPropertyChanged("UnrealizedGainAggregate"); }
        }
        #endregion

        #region Constructors
        public PortfolioAggregate()
        {
        }
        #endregion

        #region INotifyPropertyChange Handler
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}
