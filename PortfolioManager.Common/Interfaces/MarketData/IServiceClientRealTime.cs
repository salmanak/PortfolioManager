using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common.Interfaces.MarketData
{
    /// <summary>
    /// interface to be implemented by Service Client handling only RealTime market data
    /// </summary>
    public interface IServiceClientRealTime
    {
        #region Connection Management Methods
        void Connect();
        void Disconnect();
        #endregion

        #region Real-Time Quotes Access Methods
        void RegisterCallBack(ServiceClientSimulatorCallBack callBack);
        void Subscribe(string symbol);
        void SubscribeAll(List<string> symbols);
        void UnSubscribe(string symbol);
        #endregion
    }
}
