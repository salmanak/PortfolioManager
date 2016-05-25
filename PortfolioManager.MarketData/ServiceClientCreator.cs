using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.MarketData.ServiceClients;
using PortfolioManager.Common;
using PortfolioManager.MarketData.ServiceClients.Yahoo;
using PortfolioManager.MarketData.ServiceClients.Barchart;
using PortfolioManager.Common.Interfaces.MarketData;

namespace PortfolioManager.MarketData
{
    public class ServiceClientCreator
    {
        public static IServiceClient FactoryMethod()
        {
            return FactoryMethod(MarketDataSettings.GetActiveMarketDateSettingsName());
        }

        public static IServiceClient FactoryMethod(string serviceClientName)
        {
            switch (serviceClientName)
            {

                // TODO: can generalize more by using reflection

                case "yahoo":
                    return new YahooClient();
                case "barchart":
                    return new BarchartClient();
                default:
                    return null;
            }
        }

    }
}
