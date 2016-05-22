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
            string activeMarketDataSettingsName = GetActiveMarketDateSettingsName();
            if (string.IsNullOrEmpty(activeMarketDataSettingsName))
                activeMarketDataSettingsName = "yahoo";

            MarketDataSettings configuration =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                .SectionGroups["marketDataSettings"]
                .Sections[activeMarketDataSettingsName]
                as MarketDataSettings;

            //MarketDataSettings configuration =
            //    ConfigurationManager
            //    .GetSection("marketDataSettings")
            //    as MarketDataSettings;

            if (configuration != null)
                return configuration;

            return new MarketDataSettings();
        }

        public static string GetActiveMarketDateSettingsName()
        {
            try
            {
                return
                    (ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                    .SectionGroups["marketDataSettings"]
                    .Sections["_activeSettings"]
                    as MarketDataSettings)
                    .Name;
            }
            catch (Exception ex)
            {
                //TODO: Log error
                return "";
            }
        }

        #endregion

        #region Properties

        [ConfigurationProperty("name", IsRequired = false)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("urlPrefix", IsRequired = false)]
        public string URLPrefix
        {
            get
            {
                return this["urlPrefix"] as string;
            }
        }

        [ConfigurationProperty("symbolsSeparator", IsRequired = false)]
        public string SymbolsSeparator
        {
            get
            {
                return this["symbolsSeparator"] as string;
            }
        }

        [ConfigurationProperty("urlPostfix", IsRequired = false)]
        public string URLPostfix
        {
            get
            {
                return this["urlPostfix"] as string;
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
