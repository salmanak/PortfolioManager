using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.Threading;
using System.Collections;
using PortfolioManager.Common;

namespace PortfolioManager.MarketData
{
    class MarketDataAdapter:IMarketData
    {
        private ILogger _logger = new LoggingService(typeof(MarketDataAdapter)); 

        Dictionary<string, MarketDataEntity> subscriptions;

        // For Simulation
        Thread thread;
        bool keepRunning;
        Random rand;

        public MarketDataAdapter()
        {
            subscriptions = new Dictionary<string, MarketDataEntity>();

            // For Simulation
            rand = new Random();
            keepRunning = true;
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
            _logger.Log("Subscribing for symbol : " + symbol);

            lock (((IDictionary)subscriptions).SyncRoot)
            {
                if (!subscriptions.ContainsKey(symbol))
                    subscriptions.Add(symbol, new MarketDataEntity(symbol, GetLastPrice(symbol)));
            }
        }

        public override void UnSubscribe(string symbol)
        {
            _logger.Log("UnSubscribing for symbol : " + symbol);

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
            RootObject_getQuote arr = MarketDataProvider.GetQuote(symbol);

            if (arr != null)
            {
                return arr.results[0].lastPrice;
            }
            else
            {
                _logger.Log("Generating Random Price : " + symbol);
                return GetRandomNumber();
            }
        }

        public void ThreadFunc()
        {
            //For Simulation
            _logger.Log("Starting simulator thread.");
            while (keepRunning)
            {
                lock (((IDictionary)subscriptions).SyncRoot)
                {
                    foreach (var kvp in subscriptions)
                    {
                        //TODO: Refactor
                        double tmp = rand.NextDouble();
                        bool directionUp = (tmp > 0.5);

                        MarketDataEntity mkt = (MarketDataEntity)kvp.Value;
                        if (mkt.LastPrice <= 0)
                            mkt.LastPrice = GetRandomNumber();

                        if (mkt.LastPrice <= 1) // Bottom or Top Value
                            directionUp = true;
                        else if (mkt.LastPrice >= 1000)
                            directionUp = false;

                        mkt.LastPrice += (directionUp) ? 0.2 : -0.2;

                        Notify(mkt);
                    }
                }
                Thread.Sleep(750); //TODO: Get from config
            }
            _logger.Log("simulator thread stopped.");
        }
    }
}
