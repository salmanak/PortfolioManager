using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using PortfolioManager.Common;
using System.Threading;
using System.Threading.Tasks;
using PortfolioManager.Common.Interfaces.Model;
using PortfolioManager.Common.Interfaces.MarketData;
using PortfolioManager.Model;

namespace PortfolioManager.MarketData.ServiceClients
{
    public class ServiceClientSimulator
    {
        
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(ServiceClientSimulator));
        /// <summary>
        /// data structure to maintain the symbol based market data
        /// </summary>
        private ConcurrentDictionary<string, IMarketDataEntity> _subscriptions = new ConcurrentDictionary<string, IMarketDataEntity>();

        private ServiceClientSimulatorCallBack _callBackDelegate;

        private IServiceClient _serviceClient;
        public IServiceClient ServiceClient
        {
            set { _serviceClient = value; }
        }

        #endregion


        public ServiceClientSimulator()
        {
            InitSimulation();
        }


        public void RegisterCallBack(ServiceClientSimulatorCallBack callBack)
        {
            _callBackDelegate = callBack;
        }


        #region Methods
        /// <summary>
        /// Get the Last Price of symbol from external Data Provider
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <returns>Last Price of the symbol</returns>
        private double GetLastPrice(string symbol)
        {
            IMarketDataEntity mktData = _serviceClient.GetQuote(symbol);

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

        private void GetLastPrices(Dictionary<string, double> symbols)
        {
            _serviceClient.GetQuotes(symbols);

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
        public void Subscribe(string symbol)
        {

            _logger.Log("Subscribing for symbol : " + symbol);

            double lastPrice = GetLastPrice(symbol);

            if (!_subscriptions.ContainsKey(symbol))
                _subscriptions.TryAdd(symbol, new MarketDataEntity(symbol, lastPrice));
        }


        public void SubscribeAll(List<string> symbols)
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
        public void UnSubscribe(string symbol)
        {
            _logger.Log("UnSubscribing for _symbol : " + symbol);

            IMarketDataEntity mkt;

            if (_subscriptions.ContainsKey(symbol))
                _subscriptions.TryRemove(symbol, out mkt);


        }
        #endregion

        #region For Simulation
        private Task _task;
        private CancellationTokenSource _tokenSource;
        private CancellationToken _token;


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

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;            
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public void Connect()
        {
            _task = Task.Factory.StartNew(() => ThreadFunc(_token), _token);
        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public void Disconnect()
        {
            _tokenSource.Cancel();

            try
            {
                _task.Wait();
            }
            catch (AggregateException e)
            {
                _logger.Log("AggregateException thrown with the following inner exceptions:");
                // Display information about each exception. 
                foreach (var v in e.InnerExceptions)
                {
                    if (v is TaskCanceledException)
                        _logger.Log(string.Format( "TaskCanceledException: Task {0}", ((TaskCanceledException)v).Task.Id));
                    else
                        _logger.Log(string.Format("   Exception: {0}", v.GetType().Name));
                }
            }
            finally
            {
                _tokenSource.Dispose();
            }

        }
        /// <summary>
        /// For Simulation
        /// </summary>
        public void ThreadFunc(CancellationToken ct)
        {
            //For Simulation
            _logger.Log("Starting simulator _thread.");

            // Was cancellation already requested? 
            if (ct.IsCancellationRequested == true)
            {
                _logger.Log("Task was cancelled before it got started.");
                ct.ThrowIfCancellationRequested();
            } 

            while (true)
            {
                if (_subscriptions.Count > 0)
                {
                    foreach (var kvp in _subscriptions)
                    {
                        //TODO: Refactor
                        double tmp = _rand.NextDouble();
                        bool directionUp = (tmp > 0.5);

                        IMarketDataEntity mktData = (IMarketDataEntity)kvp.Value;
                        if (mktData.LastPrice <= 0)
                            mktData.LastPrice = GetRandomNumber();

                        if (mktData.LastPrice <= 1) // Bottom or Top Value
                            directionUp = true;
                        else if (mktData.LastPrice >= 1000)
                            directionUp = false;

                        mktData.LastPrice += (directionUp) ? 0.3 : -0.3;

                        _logger.LogDebug("ThreadFunc " + mktData.Symbol + mktData.LastPrice.ToString());

                        _callBackDelegate(mktData);
                        //Notify(new IMarketDataEntity(mkt));
                    }
                }
                Thread.Sleep(250); //TODO: Get from config

                // Was cancellation already requested? 
                if (ct.IsCancellationRequested == true)
                {
                    _logger.Log("Task was cancelled.");
                    ct.ThrowIfCancellationRequested();
                } 
            }
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
