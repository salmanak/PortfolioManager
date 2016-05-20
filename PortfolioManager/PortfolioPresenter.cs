using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.MarketData;
using PortfolioManager.Common;
using PortfolioManager.Data;
using PortfolioManager.View;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace PortfolioManager
{
    /// <summary>
    /// The Presenter class that controls the View and Model.
    /// This also controls the Market Data Access.
    /// </summary>
    internal class PortfolioPresenter
    {
        #region Declarations and Definitions
        /// <summary>
        /// for Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(PortfolioPresenter));
        /// <summary>
        /// DAL data mapper
        /// </summary>
        private TradeDataMapper _tradeDataMapper;
        /// <summary>
        /// Portfolio View reference
        /// </summary>
        private readonly IPortfolioView _portfolioView;
        /// <summary>
        /// Portfolio Model component to access underlying data
        /// </summary>
        private readonly IPortfolioDao _portfolioDao;
        /// <summary>
        /// Market Data Adapter to access market data
        /// </summary>
        private IMarketData _mktDataAdapter;
        /// <summary>
        /// setter injection for Market Data Adapter
        /// </summary>
        public IMarketData MarketDataAdapter
        {
            //get { return _mktDataAdapter; }
            set
            {
                _mktDataAdapter = value;

                _mktDataAdapter.AddObserver(new ObservableObject<MarketDataEntity>.NotifyObserver(this.OnMarketDataUpdate));

                SubscribeMarketData();

                _mktDataAdapter.Start();
            }
        }

        private void SubscribeMarketData()
        {
            _logger.LogDebug("SubscribeMarketData ");

            _mktDataAdapter.SubscribeAll(_portfolioDao.GetAllPortfolioItems().Select(x => x.Symbol).ToList());

        }

        private AutoQueue<MarketDataEntity> _mktDataQueue;

        #endregion

        #region Constructor and Initializations
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="view">view attached with the presenter</param>
        /// <param name="dao">model used by presenter and view</param>
        public PortfolioPresenter(IPortfolioView view, IPortfolioDao dao)
        {
            _portfolioView = view;
            _portfolioDao = dao;
            _tradeDataMapper = new TradeDataMapper();

            UpdateGUI();

            //ProcessAllTrades(GetAllTrades());

            GetAllTradesMethodCaller caller = new GetAllTradesMethodCaller(this.GetAllTrades);
            IAsyncResult result = caller.BeginInvoke(new AsyncCallback(GetAllTradesComplete),"The call executed on thread {0}, with return value \"{1}\".");


            //var task = Task.Factory.StartNew(() => ProcessAllTrades());
            //task.Wait();
            ////GetAllTrades();
            
            InitMarketDataQueue();
        }


        // The callback method must have the same signature as the
        // AsyncCallback delegate.
        void GetAllTradesComplete(IAsyncResult ar)
        {
            _logger.LogDebug("GetAllTradesComplete ");

            // Retrieve the delegate.
            AsyncResult result = (AsyncResult)ar;
            GetAllTradesMethodCaller caller = (GetAllTradesMethodCaller)result.AsyncDelegate;

            // Retrieve the format string that was passed as state 
            // information.
            //string formatString = (string)ar.AsyncState;

            // Call EndInvoke to retrieve the results.
            IEnumerable<PortfolioDataEntity> trades = caller.EndInvoke(ar);

            ProcessAllTrades(trades);

            SubscribeMarketData();
        }

        public delegate IEnumerable<PortfolioDataEntity> GetAllTradesMethodCaller();
        private IEnumerable<PortfolioDataEntity> GetAllTrades()
        {
            return _tradeDataMapper.GetAllTrades();
        }

        private void ProcessAllTrades(IEnumerable<PortfolioDataEntity> trades)
        {
            //IEnumerable<PortfolioDataEntity> trades = GetAllTrades();
            foreach (var item in trades)
                Save(item);
        }



        private void InitMarketDataQueue()
        {
            _mktDataQueue = new AutoQueue<MarketDataEntity>(250); // TODO: get from config
            _mktDataQueue.signalEvent += new AutoQueueEventHandler<AutoQueue<MarketDataEntity>, IEnumerable<AutoQueue<MarketDataEntity>.GenericsEventArgs<MarketDataEntity>>>(OnSignalled);
            _mktDataQueue.Start();
        } 
        #endregion

        #region General Methods
        /// <summary>
        /// called at startup to ensure view is updated
        /// </summary>
        private void UpdateGUI()
        {
            _portfolioView.ShowPortfolioItems(_portfolioDao.GetAllPortfolioItems());
            
            PortfolioAggregate p = _portfolioDao.GetPortfolioAggregate();
            _portfolioView.ShowPortfolioAggregate(p);

        }

        /// <summary>
        /// Called to save a new trade
        /// </summary>
        /// <param name="portfolio">trade info that needs to be saved</param>
        private void Save(PortfolioDataEntity portfolio)
        {
            _portfolioDao.Save(portfolio);
        }

        /// <summary>
        /// Called to close running threads
        /// </summary>
        public void Close()
        {
            _mktDataQueue.Stop();
            _mktDataAdapter.Stop();
        } 
        #endregion

        #region View Methods
        /// <summary>
        /// Called from View to add Trade
        /// </summary>
        /// <param name="symbol">symbol to add</param>
        /// <param name="shares">shares for the trade</param>
        /// <param name="price">price for the trade</param>
        public int AddPortfolioClicked(String symbol, long shares, double price)
        {

            Save(new PortfolioDataEntity(symbol, shares, price));

            _portfolioView.UpdatePortfolioItems();

            _mktDataAdapter.Subscribe(symbol);

            _tradeDataMapper.SaveTradeAsync(symbol, shares, price);

            return 0;
        } 
        #endregion

        #region Events
        /// <summary>
        /// Invoked when market data update is received
        /// </summary>
        /// <param name="mktData">market data that is received</param>
        public void OnMarketDataUpdate(MarketDataEntity mktData)
        {
            _logger.LogDebug("OnMarketDataUpdate " + mktData.Symbol + mktData.LastPrice.ToString());

            _mktDataQueue.Enqueue(mktData);          
        }

        public void OnSignalled(AutoQueue<MarketDataEntity> q,  IEnumerable<AutoQueue<MarketDataEntity>.GenericsEventArgs<MarketDataEntity>> args) 
        {
            foreach (AutoQueue<MarketDataEntity>.GenericsEventArgs<MarketDataEntity> item in args)
            {
                MarketDataEntity mktData = item.Item;

                _logger.LogDebug("OnSignalled " + mktData.Symbol + mktData.LastPrice.ToString());

                _portfolioDao.Save(mktData.Symbol, mktData);
            }          
            
        }
        #endregion


    }
}
