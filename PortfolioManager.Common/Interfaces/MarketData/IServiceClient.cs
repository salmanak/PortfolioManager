using System;
using System.Collections.Generic;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManager.Common.Interfaces.MarketData
{
    public delegate void ServiceClientSimulatorCallBack(IMarketDataEntity mktData);

    public interface IServiceClient
    {
        #region Connection Management Methods
        void Connect();
        void Disconnect();
        #endregion

        #region Snapshot Quotes Access Methods
        IMarketDataEntity GetQuote(string symbol);
        void GetQuotes(Dictionary<string, double> symbols);
        #endregion

        #region Real-Time Quotes Access Methods
        void RegisterCallBack(ServiceClientSimulatorCallBack callBack);
        void Subscribe(string symbol);
        void SubscribeAll(List<string> symbols);
        void UnSubscribe(string symbol);
        #endregion
    }
}
