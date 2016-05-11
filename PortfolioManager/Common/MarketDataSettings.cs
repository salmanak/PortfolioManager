using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace PortfolioManager.Common
{
    public class MarketDataSettings : ConfigurationSection
    {
        public static MarketDataSettings GetConfiguration()
        {
            MarketDataSettings configuration =
                ConfigurationManager
                .GetSection("marketDataSettings")
                as MarketDataSettings;

            if (configuration != null)
                return configuration;

            return new MarketDataSettings();
        }

        [ConfigurationProperty("url", IsRequired = false)]
        public string URL
        {
            get
            {
                return this["url"] as string;
            }
        }

        [ConfigurationProperty("proxyAddress", IsRequired = false)]
        public string ProxyAddress
        {
            get
            {
                return this["proxyAddress"] as string;
            }
        }
    }

}
