using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using PortfolioManager.Common;
using PortfolioManager.Data;

namespace PortfolioManager.MarketData
{
    /// <summary>
    /// Concrete class to represent the external market data provider
    /// in this case connects to BarChart OnDemand REST API
    /// </summary>
    class MarketDataProvider
    {
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private static ILogger _logger = new LoggingService(typeof(MarketDataProvider));  
        #endregion

        #region Methods
        /// <summary>
        /// Gets Quote from external market data provider
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>returns the market data AS-IS. Need to make it loosely coupled</returns>
        private static RootObject_getQuote GetQuoteInternal(string symbol)
        {
            _logger.LogDebug("GetQuoteInternal " + symbol);

            try
            {
                //"http://marketdata.websol.barchart.com/getQuote.json?key=75dd0aebc8c6e5a9c8d1b9be02cf5ba9&symbols="
                string url_getQuote = MarketDataSettings.GetConfiguration().URL;
                if (string.IsNullOrEmpty(url_getQuote))
                    return null;
                url_getQuote += "&symbols=";
                url_getQuote += symbol;

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
        public static MarketDataEntity GetQuote(string symbol)
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

        private static void GetQuotesInternal(Dictionary<string,Result_getQuote> symbols)
        {
            _logger.LogDebug("GetQuoteInternal " + symbols.Count.ToString());

            try
            {
                //"http://marketdata.websol.barchart.com/getQuote.json?key=75dd0aebc8c6e5a9c8d1b9be02cf5ba9&symbols="
                string url_getQuote = MarketDataSettings.GetConfiguration().URL;
                if (string.IsNullOrEmpty(url_getQuote))
                    return;
                url_getQuote += "&symbols=";


                string symbolsList = String.Join(",", symbols.Select(kv=>kv.Key.ToString()));
                url_getQuote += symbolsList;



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

        public static void GetQuotes(Dictionary<string, double> symbols)
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

    #region REST/JSON Objects
    //
    // Classes generated using 
    // http://json2csharp.com/
    //
    public class Status
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class Result_getQuote
    {
        public string symbol { get; set; }
        public string exchange { get; set; }
        public string name { get; set; }
        public string dayCode { get; set; }
        public string serverTimestamp { get; set; }
        public string mode { get; set; }
        public double lastPrice { get; set; }
        public string tradeTimestamp { get; set; }
        public double netChange { get; set; }
        public double percentChange { get; set; }
        public string unitCode { get; set; }
        public double open { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public object close { get; set; }
        public string flag { get; set; }
        public int volume { get; set; }
    }

    public class RootObject_getQuote
    {
        public Status status { get; set; }
        public List<Result_getQuote> results { get; set; }
    } 
    #endregion

}
