using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using PortfolioManager.Common;
using PortfolioManager.MarketData.ServiceClients.Barchart.ObjectModel;
using PortfolioManager.Common.Interfaces.Model;
using PortfolioManager.Common.Interfaces.MarketData;
using PortfolioManager.Model;

namespace PortfolioManager.MarketData.ServiceClients.Barchart
{
    public class BarchartClient:ServiceClientRealTimeSimulator, IServiceClientSnapShot, IServiceClient
    {

        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private static ILogger _logger = new LoggingService(typeof(BarchartClient));
        #endregion

        #region Constructor
        public BarchartClient()
        {
            base.ServiceClient = this;
        }
        #endregion

        #region Internal Methods
        /// <summary>
        /// Gets Quote from external market data provider
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>returns the market data AS-IS. Need to make it loosely coupled</returns>
        private  RootObject_getQuote GetQuoteInternal(string symbol)
        {
            _logger.LogDebug("GetQuoteInternal " + symbol);

            try
            {
                //"http://marketdata.websol.barchart.com/getQuote.json?key=75dd0aebc8c6e5a9c8d1b9be02cf5ba9&symbols="
                string url_getQuote = MarketDataSettings.GetConfiguration().URLPrefix;
                if (string.IsNullOrEmpty(url_getQuote))
                    return null;
                //url_getQuote += "&symbols=";
                url_getQuote += symbol;
                url_getQuote += MarketDataSettings.GetConfiguration().URLPostfix;


                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_getQuote);

                string proxyAddress = MarketDataSettings.GetConfiguration().ProxyAddress;
                if (!string.IsNullOrEmpty(proxyAddress))
                {
                    WebProxy proxyObj = new WebProxy(proxyAddress);
                    proxyObj.Credentials = CredentialCache.DefaultCredentials;
                    webRequest.Proxy = proxyObj;
                }

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                if ((webResponse.StatusCode == HttpStatusCode.OK))// && (webResponse.ContentLength > 0))
                {
                    StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                    string s = reader.ReadToEnd();

                    var serializer = new JavaScriptSerializer();
                    RootObject_getQuote arr = serializer.Deserialize<RootObject_getQuote>(s);

                    //TODO: Need to make it loosely coupled
                    return arr;

                }
                else
                {
                    _logger.Log(string.Format("Status code == {0}, Content length == {1}", webResponse.StatusCode, webResponse.ContentLength));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to create Connection.", ex);
            }
            return null;
        }

        /// <summary>
        /// Public method to get the market data. Calls internal method to get data in provider format.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public  IMarketDataEntity GetQuote(string symbol)
        {
            _logger.LogDebug("GetQuote " + symbol);
            RootObject_getQuote q = GetQuoteInternal(symbol);

            if (q != null && q.results != null)
            {
                if (q.results.Count > 0)
                {
                    if (q.results[0] != null)
                    {
                        return new MarketDataEntity(symbol, q.results[0].lastPrice);
                    }
                }
            }

            _logger.LogError("Unable to process the Quote Response.");
            return null;
        }
        #endregion

        #region Interface Methods
        private  void GetQuotesInternal(Dictionary<string, Result_getQuote> symbols)
        {
            _logger.LogDebug("GetQuoteInternal " + symbols.Count.ToString());

            try
            {
                //"http://marketdata.websol.barchart.com/getQuote.json?key=75dd0aebc8c6e5a9c8d1b9be02cf5ba9&symbols="
                string url_getQuote = MarketDataSettings.GetConfiguration().URLPrefix;
                if (string.IsNullOrEmpty(url_getQuote))
                    return;
                //url_getQuote += "&symbols=";


                string symbolsList = String.Join(MarketDataSettings.GetConfiguration().SymbolsSeparator, symbols.Select(kv => kv.Key.ToString()));
                url_getQuote += symbolsList;

                url_getQuote += MarketDataSettings.GetConfiguration().URLPostfix;



                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_getQuote);

                string proxyAddress = MarketDataSettings.GetConfiguration().ProxyAddress;
                if (!string.IsNullOrEmpty(proxyAddress))
                {
                    WebProxy proxyObj = new WebProxy(proxyAddress);
                    proxyObj.Credentials = CredentialCache.DefaultCredentials;
                    webRequest.Proxy = proxyObj;
                }

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                if ((webResponse.StatusCode == HttpStatusCode.OK))// && (webResponse.ContentLength > 0))
                {
                    StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                    string s = reader.ReadToEnd();

                    var serializer = new JavaScriptSerializer();
                    RootObject_getQuote arr = serializer.Deserialize<RootObject_getQuote>(s);

                    if (arr != null && arr.results != null)
                    {
                        foreach (var item in arr.results)
                        {
                            symbols[item.symbol] = item;
                        }
                    }

                    //TODO: Need to make it loosely coupled
                    return;

                }
                else
                {
                    _logger.Log(string.Format("Status code == {0}, Content length == {1}", webResponse.StatusCode, webResponse.ContentLength));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to create Connection.", ex);
            }
            return;
        }

        public  void GetQuotes(Dictionary<string, double> symbols)
        {
            if (symbols.Count <= 0)
                return;

            Dictionary<string, Result_getQuote> outSymbols = symbols.Keys.ToDictionary(x => x, x => new Result_getQuote());

            _logger.LogDebug("GetQuotes " + symbols.Count.ToString());

            GetQuotesInternal(outSymbols);

            foreach (var kvp in outSymbols)
            {
                Result_getQuote q = kvp.Value;
                if (q != null)
                {
                    symbols[kvp.Key] = q.lastPrice;
                }
            }
        }
        #endregion
    }
}
