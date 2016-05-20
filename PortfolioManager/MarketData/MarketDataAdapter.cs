using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.Data;
using System.Threading;
using System.Collections;
using PortfolioManager.Common;
using System.Collections.Concurrent;

namespace PortfolioManager.MarketData
{
    class MarketDataAdapter:IMarketData
    {
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(MarketDataAdapter));
        /// <summary>
        /// data structure to maintain the symbol based market data
        /// </summary>
        private ConcurrentDictionary<string, MarketDataEntity> _subscriptions;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MarketDataAdapter()
        {
            _subscriptions = new ConcurrentDictionary<string, MarketDataEntity>();

            InitSimulation();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get the Last Price of symbol from external Data Provider
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>Last Price of the symbol</returns>
        private double GetLastPrice(string symbol)
        {
            MarketDataEntity mktData = MarketDataProvider.GetQuote(symbol);

            if (mktData != null)
            {
                return mktData.LastPrice;
            }
            else
            {
                _logger.Log("Generating Random Price : " + symbol);
                // For Simulation
                return GetRandomNumber();
            }
        }

        private void GetLastPrices(Dictionary<string,double> symbols)
        {
            MarketDataProvider.GetQuotes(symbols);

            foreach (var kvp in symbols)
            {
                if (kvp.Value <= 0)
                {
                    _logger.Log("Generating Random Price : " + kvp.Key);
                    // For Simulation
                    symbols[kvp.Key] = GetRandomNumber();
                }
            }
        }

        /// <summary>
        /// Add subscription in the data structure
        /// </summary>
        /// <param name="symbol">symbol to be subscribed</param>
        public override void Subscribe(string symbol)
        {
            _logger.Log("Subscribing for symbol : " + symbol);

                if (!_subscriptions.ContainsKey(symbol))
                    _subscriptions.TryAdd(symbol, new MarketDataEntity(symbol, GetLastPrice(symbol)));
        }


        public override void SubscribeAll(List<string> symbols)
        {

            if (symbols.Count <= 0)
                return;

            _logger.Log("Subscribing for _symbols : " + symbols.Count.ToString());

            Dictionary<string, double> lastPrices = symbols.ToDictionary(x => x, x => (double)0);
            GetLastPrices(lastPrices);

            foreach (var kvp in lastPrices)
            {
                string symbol = kvp.Key;

                if (!_subscriptions.ContainsKey(symbol))
                    _subscriptions.TryAdd(symbol, new MarketDataEntity(symbol, kvp.Value));

            }
        }

        /// <summary>
        /// Remove subscription from the data structure
        /// </summary>
        /// <param name="symbol">symbol to be un subscribed</param>
        public override void UnSubscribe(string symbol)
        {
            _logger.Log("UnSubscribing for _symbol : " + symbol);

            MarketDataEntity mkt;

                if (_subscriptions.ContainsKey(symbol))
                    _subscriptions.TryRemove(symbol, out mkt);


        }
        #endregion

        #region For Simulation
        /// <summary>
        /// For Simulation
        /// </summary>
        private Thread _thread;
        /// <summary>
        /// For Simulation
        /// </summary>
        private bool _keepRunning;
        /// <summary>
        /// For Simulation
        /// </summary>
        private Random _rand;
        /// <summary>
        /// For Simulation
        /// </summary>
        private void InitSimulation()
        {
            _rand = new Random();
            _keepRunning = true;
            _thread = new Thread(ThreadFunc);
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public override void Start()
        {
            _keepRunning = true;
            _thread.Start();
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public override void Stop()
        {
            _keepRunning = false;
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public void ThreadFunc()
        {
            //For Simulation
            _logger.Log("Starting simulator _thread.");
            while (_keepRunning)
            {

                
                if (_subscriptions.Count > 0)
                {
                    foreach (var kvp in _subscriptions)
                    {
                        //TODO: Refactor
                        double tmp = _rand.NextDouble();
                        bool directionUp = (tmp > 0.5);

                        MarketDataEntity mkt = (MarketDataEntity)kvp.Value;
                        if (mkt.LastPrice <= 0)
                            mkt.LastPrice = GetRandomNumber();

                        if (mkt.LastPrice <= 1) // Bottom or Top Value
                            directionUp = true;
                        else if (mkt.LastPrice >= 1000)
                            directionUp = false;

                        mkt.LastPrice += (directionUp) ? 0.3 : -0.3;

                        _logger.LogDebug("ThreadFunc " + mkt.Symbol + mkt.LastPrice.ToString());

                        Notify(new MarketDataEntity(mkt));
                    }
                }
                Thread.Sleep(250); //TODO: Get from config
            }
            _logger.Log("simulator _thread stopped.");
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public double GetRandomNumber()
        {
            return GetRandomNumber(1, 1000);
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public double GetRandomNumber(double minimum, double maximum)
        {
            return _rand.NextDouble() * (maximum - minimum) + minimum;
        }
        #endregion


    }
}
