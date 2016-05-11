using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.Threading;
using System.Collections;

namespace PortfolioManager.MarketData
{
    class MarketDataAdapter:IMarketData
    {
        Dictionary<string, MarketDataEntity> subscriptions;
        Thread thread;
        bool keepRunning;
        Random rand;

        public MarketDataAdapter()
        {
            rand = new Random();
            keepRunning = true;
            subscriptions = new Dictionary<string, MarketDataEntity>();
            thread = new Thread(ThreadFunc);
        }

        public override void Start()
        {
            keepRunning = true;
            thread.Start();
        }

        public override void Stop()
        {
            keepRunning = false;
        }

        public override void Subscribe(string symbol)
        {
            lock (((IDictionary)subscriptions).SyncRoot)
            {
                if (!subscriptions.ContainsKey(symbol))
                    subscriptions.Add(symbol, new MarketDataEntity(symbol, GetLastPrice(symbol)));
            }
        }

        public override void UnSubscribe(string symbol)
        {
            lock (((IDictionary)subscriptions).SyncRoot)
            {
                if ( subscriptions.ContainsKey(symbol))
                    subscriptions.Remove(symbol);
            }
             
        }


        public double GetRandomNumber()
        {
            return GetRandomNumber(1, 1000);
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            return rand.NextDouble() * (maximum - minimum) + minimum;
        }


        private double GetLastPrice(string symbol)
        {
            //TODO: Get quote from external market data provider
            RootObject_getQuote arr = MarketDataProvider.GetQuote(symbol);

            if (arr != null)
            {
                return arr.results[0].lastPrice;
            }
            else
            {
                return GetRandomNumber();
            }

            /*
            switch (symbol)
            {
                case "IBM":
                    return 150;
                case "MSFT":
                    return 50;
                case "AAPL":
                    return 90;
                case "FB":
                    return 120;
                case "GOOG":
                    return 720;
                default:
                    return GetRandomNumber();
            }
            */

        }

        public void ThreadFunc()
        {
            while (keepRunning)
            {
                lock (((IDictionary)subscriptions).SyncRoot)
                {
                    foreach (var kvp in subscriptions)
                    {
                        double tmp = rand.NextDouble();
                        bool directionUp = (tmp > 0.5);

                        MarketDataEntity mkt = (MarketDataEntity)kvp.Value;
                        if (mkt.LastPrice <= 0)
                        {
                            mkt.LastPrice = GetRandomNumber();
                        }

                        if (mkt.LastPrice <= 1) // Bottom or Top Value
                        {
                            directionUp = true;
                        }
                        else if (mkt.LastPrice >= 1000)
                        {
                            directionUp = false;
                        }

                        mkt.LastPrice += (directionUp) ? 1 : -1;

                        Notify(mkt);
                    }
                }

                Thread.Sleep(750);
            }
        }
    }
}
