using System;
using PortfolioManager.Data;
using System.Collections.Generic;
namespace PortfolioManager.MarketData.ServiceClients
{
    public interface IServiceClient
    {
        #region Connection Management Methods
        void Connect();
        void Disconnect();
        #endregion

        #region Snapshot Quotes Access Methods
        MarketDataEntity GetQuote(string symbol);
        void GetQuotes(Dictionary<string, double> symbols);
        #endregion

        #region Real-Time Quotes Access Methods
        void RegisterCallBack(ServiceClientSimulator.ServiceClientSimulatorCallBack callBack);
        void Subscribe(string symbol);
        void SubscribeAll(List<string> symbols);
        void UnSubscribe(string symbol);
        #endregion
    }
}
