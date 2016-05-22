using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Common;
using PortfolioManager.Data;
using System.Net;
using System.IO;
using PortfolioManager.MarketData.ServiceClients.Yahoo.ObjectModel;

namespace PortfolioManager.MarketData.ServiceClients.Yahoo
{
    class YahooClient : ServiceClientSimulator, IServiceClient
    {

        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private static ILogger _logger = new LoggingService(typeof(YahooClient));
        #endregion

        #region Constructor
        public YahooClient()
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
        private Quote GetQuoteInternal(string symbol)
        {
            _logger.LogDebug("GetQuoteInternal " + symbol);

            try
            {
                //"http://finance.yahoo.com/d/quotes.csv?s=AAPL+GOOG+MSFT&f=sl1"
                string url_getQuote = MarketDataSettings.GetConfiguration().URLPrefix;
                if (string.IsNullOrEmpty(url_getQuote))
                    return null;
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
                    string str = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(str))
                        return null;

                    string[] lines = str.Replace("\r", "").Split('\n');

                    return Quote.FromString(lines[0]);

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
        public MarketDataEntity GetQuote(string symbol)
        {
            _logger.LogDebug("GetQuote " + symbol);
            Quote q = GetQuoteInternal(symbol);


            if (q != null && !string.IsNullOrEmpty(q.symbol))
            {
                return new MarketDataEntity(q.symbol, q.lastPrice);
            }

            _logger.LogError("Unable to process the Quote Response.");
            return null;
        }
        #endregion

        #region Interface Methods
        private  void GetQuotesInternal(Dictionary<string, Quote> symbols)
        {
            _logger.LogDebug("GetQuoteInternal " + symbols.Count.ToString());

            try
            {
                //"http://finance.yahoo.com/d/quotes.csv?s=AAPL+GOOG+MSFT&f=sl1"
                string url_getQuote = MarketDataSettings.GetConfiguration().URLPrefix;
                if (string.IsNullOrEmpty(url_getQuote))
                    return;
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
                    string str = reader.ReadToEnd();

                    if (string.IsNullOrEmpty(str))
                        return;

                    string[] lines = str.Replace("\r", "").Split('\n');
                    foreach (string item in lines)
                    {
                        Quote q = Quote.FromString(item);

                        if ( q != null && !string.IsNullOrEmpty(q.symbol))
                            symbols[q.symbol] = q;
                    } 
                    
                        

                  

                        
                    //s.Replace("\r", "")
                    //.Split('\n')
                    //.Select(x => x.Split(','))
                    //.Select(x => symbols[x[0]] = x[1]);

                    //symbols = s.Replace("\r", "")
                    //            .Split('\n')
                    //            .Select(x => x.Split(','))
                    //            .ToDictionary(key => key[0].Trim(), value => value[1].Trim());


                    //var a = str.Replace("\r", "");
                    //var b = a.Split('\n');
                    //var c = b.Select(x => x.Split(",".ToCharArray(),StringSplitOptions.RemoveEmptyEntries));
                    //c = c.Where(s => s.Length>0).ToList();
                    ////symbols = c.ToDictionary(key => key[0].Replace('"', ' ').Trim(), value => value[1].Replace('"', ' ').Trim());                    
                    //c.Select(x => symbols[x[0].Replace('"', ' ').Trim()] = x[1].Replace('"', ' ').Trim());


                    //var data = s.Replace("\r", "")
                    //            .Split('\n')
                    //            .Select(x => x.Split(','))
                    //            .Select(x => new
                    //                {
                    //                    symbol = x[0],
                    //                    lastPrice = double.Parse(x[1])
                    //                });




                    //var serializer = new JavaScriptSerializer();
                    //RootObject_getQuote arr = serializer.Deserialize<RootObject_getQuote>(s);

                    //if (arr != null && arr.results != null)
                    //{
                    //    foreach (var item in arr.results)
                    //    {
                    //        symbols[item.symbol] = item;
                    //    }
                    //}

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

        public void GetQuotes(Dictionary<string, double> symbols)
        {
            if (symbols.Count <= 0)
                return;

            Dictionary<string, Quote> outSymbols = symbols.Keys.ToDictionary(x => x, x => new Quote(x,0));

            _logger.LogDebug("GetQuotes " + symbols.Count.ToString());

            GetQuotesInternal(outSymbols);

            foreach (var kvp in outSymbols)
            {
                Quote q = kvp.Value;
                if (q != null)
                {
                    symbols[kvp.Key] = q.lastPrice;
                }
            }
        }
        #endregion

    }
}
