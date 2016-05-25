using System;
using System.Collections.Generic;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManager.Common.Interfaces.MarketData
{
    public delegate void ServiceClientSimulatorCallBack(IMarketDataEntity mktData);

    /// <summary>
    /// To be implemented by Service Client that handles both Snapshot and RealTime data
    /// Interface used by external world to interact with the Service Client
    /// </summary>
    public interface IServiceClient:IServiceClientSnapShot,IServiceClientRealTime
    {

    }
}
