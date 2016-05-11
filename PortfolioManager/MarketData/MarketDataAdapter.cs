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
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(MarketDataAdapter));
        /// <summary>
        /// data structure to maintain the symbol based market data
        /// </summary>
        private Dictionary<string, MarketDataEntity> _subscriptions;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public MarketDataAdapter()
        {
            _subscriptions = new Dictionary<string, MarketDataEntity>();

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
        /// <summary>
        /// Add subscription in the data structure
        /// </summary>
        /// <param name="symbol">symbol to be subscribed</param>
        public override void Subscribe(string symbol)
        {
            _logger.Log("Subscribing for _symbol : " + symbol);

            lock (((IDictionary)_subscriptions).SyncRoot)
            {
                if (!_subscriptions.ContainsKey(symbol))
                    _subscriptions.Add(symbol, new MarketDataEntity(symbol, GetLastPrice(symbol)));
            }
        }
        /// <summary>
        /// Remove subscription from the data structure
        /// </summary>
        /// <param name="symbol">symbol to be un subscribed</param>
        public override void UnSubscribe(string symbol)
        {
            _logger.Log("UnSubscribing for _symbol : " + symbol);

            lock (((IDictionary)_subscriptions).SyncRoot)
            {
                if (_subscriptions.ContainsKey(symbol))
                    _subscriptions.Remove(symbol);
            }

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
                lock (((IDictionary)_subscriptions).SyncRoot)
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

                        mkt.LastPrice += (directionUp) ? 0.2 : -0.2;

                        Notify(mkt);
                    }
                }
                Thread.Sleep(750); //TODO: Get from config
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
