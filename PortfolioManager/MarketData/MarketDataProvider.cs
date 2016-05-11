using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace PortfolioManager.MarketData
{
    class MarketDataProvider
    {
        public static RootObject_getQuote GetQuote(string symbol)
        {
            try
            {
                //TODO: Generalize
                //TODO: Configuration

                //string url_getQuote = "http://marketdata.websol.barchart.com/getQuote.json?key=75dd0aebc8c6e5a9c8d1b9be02cf5ba9&symbols=IBM,FB,AAPL,GOOG";
                string url_getQuote = "http://marketdata.websol.barchart.com/getQuote.json?key=75dd0aebc8c6e5a9c8d1b9be02cf5ba9&symbols=" + symbol;
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url_getQuote);

                WebProxy proxyObj = new WebProxy("http://nycproxy01.wallstreetsystems.com:8080");
                proxyObj.Credentials = CredentialCache.DefaultCredentials;

                webRequest.Proxy = proxyObj;

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                if ((webResponse.StatusCode == HttpStatusCode.OK))// && (webResponse.ContentLength > 0))
                {
                    StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                    string s = reader.ReadToEnd();
                                        
                    var serializer = new JavaScriptSerializer();
                    RootObject_getQuote arr = serializer.Deserialize<RootObject_getQuote>(s);

                    return arr;

                }
                else
                {
                    Console.WriteLine(string.Format("Status code == {0}, Content length == {1}",
                      webResponse.StatusCode, webResponse.ContentLength));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }

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

}
