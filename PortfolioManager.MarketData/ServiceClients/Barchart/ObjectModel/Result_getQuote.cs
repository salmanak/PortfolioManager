using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.MarketData.ServiceClients.Barchart.ObjectModel
{
    //
    // Classes generated using 
    // http://json2csharp.com/
    //
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
}
