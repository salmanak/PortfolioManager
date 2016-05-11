using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;

namespace PortfolioManager.Common
{
    /// <summary>
    /// Class to get Market Data Settings from Configuration
    /// </summary>
    public class MarketDataSettings : ConfigurationSection
    {
        #region Methods
        /// <summary>
        /// Gets the configuration from Configuration File
        /// </summary>
        /// <returns></returns>
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
        #endregion

        #region Properties
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
        #endregion
    }

}
