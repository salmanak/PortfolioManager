using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using PortfolioManager.Common.Interfaces.Model;
using PortfolioManager.Common;
using PortfolioManager.Common.Interfaces.View;
using PortfolioManager.Common.Interfaces.MarketData;
using PortfolioManager.Common.Interfaces.Presenter;
using PortfolioManager.Model;
using PortfolioManager.MarketData;

namespace PortfolioManager.Presenter
{
    /// <summary>
    /// The Presenter class that controls the View and Model.
    /// This also controls the Market Data Access.
    /// </summary>
    public class PortfolioPresenter : IExceptionHandler, IPortfolioPresenter
    {
        #region Declarations and Definitions
        /// <summary>
        /// for Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(PortfolioPresenter));
        
        /// <summary>
        /// Portfolio View reference
        /// </summary>
        private IPortfolioView _portfolioView;
        /// <summary>
        /// Portfolio Model component to access underlying data
        /// </summary>
        private IPortfolioDao _portfolioDao;
        /// <summary>
        /// Market data access client reference
        /// </summary>
        private IServiceClient _serviceClient;
        /// <summary>
        /// Interim queue to conflate market data updates
        /// </summary>
        private AutoQueue<IMarketDataEntity> _mktDataQueue;
        /// <summary>
        /// Conflation rate (in millisecond) for Market Data
        /// </summary>
        private readonly int _conflationRate = 250; // TODO: get from config
        #endregion

        #region Constructor and Initializations
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="view">view attached with the presenter</param>
        /// <param name="dao">model used by presenter and view</param>
        public PortfolioPresenter(IPortfolioDao dao)
        {
            SetModel(dao);
        }

        /// <summary>
        /// Setter injection to inject View
        /// </summary>
        /// <param name="view"></param>
        public void SetView(IPortfolioView view)
        {
            _portfolioView = view;
            UpdateGUI();
        }

        /// <summary>
        /// Setter injection to inject dao
        /// </summary>
        /// <param name="dao"></param>
        public void SetModel(IPortfolioDao dao)
        {
            _portfolioDao = dao;
            _portfolioDao.SetExceptionHandler(this);
            GetAllDataFromDBAsynch();
            InitMarketData();
        }

        /// <summary>
        /// Initializes the Market Data access and registers callbacks
        /// </summary>
        private void InitMarketData()
        {
            _serviceClient = ServiceClientCreator.FactoryMethod();
            _serviceClient.RegisterCallBack(this.OnServiceClientSimulatorCallBack);
            InitMarketDataQueue();
            SubscribeMarketData();
            _serviceClient.Connect();
        }

        /// <summary>
        /// Registers events for Market Data and starts the queue processing
        /// </summary>
        private void InitMarketDataQueue()
        {
            _mktDataQueue = new AutoQueue<IMarketDataEntity>(_conflationRate);
            _mktDataQueue.signalEvent += new AutoQueueEventHandler<IEnumerable<GenericsEventArgs<IMarketDataEntity>>>(OnSignalled);
            _mktDataQueue.Start();
        } 
        #endregion

        #region Persistant Storage Access Methods
        /// <summary>
        /// Asynchronously gets trades from Db
        /// register the completion event handler as well
        /// </summary>
        private void GetAllDataFromDBAsynch()
        {
            var task = Task.Factory.StartNew(() => GetAllTradesFromDB());
            task.ContinueWith(t => GetAllTradesComplete(t.Result));
        }
        /// <summary>
        /// Gets the trades from the database
        /// </summary>
        /// <returns>trades fetched from database</returns>
        private IEnumerable<IPortfolioDataEntity> GetAllTradesFromDB()
        {
            return _portfolioDao.RecoverPersistedTrades();
        }
        #endregion

        #region General Methods
        /// <summary>
        /// called at startup to ensure view is updated
        /// </summary>
        private void UpdateGUI()
        {
            _portfolioView.ShowPortfolioItems(_portfolioDao.GetAllPortfolioItems());
            _portfolioView.ShowPortfolioAggregate(_portfolioDao.GetPortfolioAggregate());

        }

        /// <summary>
        /// Called to save a new trade
        /// </summary>
        /// <param name="portfolio">trade info that needs to be saved</param>
        private void Save(IPortfolioDataEntity portfolio, bool persist = false)
        {
            _portfolioDao.Save(portfolio, persist);
            //if ( persist )
            //    _portfolioDao.PersistTradeAsync(portfolio);

        }

        /// <summary>
        /// Called to close running threads
        /// </summary>
        public void Close()
        {
            _mktDataQueue.Stop();
            _serviceClient.Disconnect();
        }

        /// <summary>
        /// Gets all portfolio securities and subscribes market data
        /// </summary>
        private void SubscribeMarketData()
        {
            _logger.LogDebug("SubscribeMarketData ");

            _serviceClient.SubscribeAll(_portfolioDao.GetAllPortfolioItems().Select(x => x.Symbol).ToList());

        }

        /// <summary>
        /// passes trades to the darta access component to save in cache
        /// </summary>
        /// <param name="trades"></param>
        private void ProcessAllTrades(IEnumerable<IPortfolioDataEntity> trades)
        {
            foreach (var item in trades)
                Save(item, false);
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

            Save(new PortfolioDataEntity(symbol, shares, price), true);

            _portfolioView.UpdatePortfolioItems();

            _serviceClient.Subscribe(symbol);

            //_tradeDataMapper.SaveTradeAsync(symbol, shares, price);

            return 0;
        } 
        #endregion

        #region Exception Handling Methods
        /// <summary>
        /// passes exception to be processed by the GUI
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            ((IExceptionHandler)_portfolioView).HandleException(ex);
        }
        #endregion

        #region Events and Callback Handlers

        /// <summary>
        /// event handler for market data updates
        /// </summary>
        /// <param name="mktData"></param>
        public void OnServiceClientSimulatorCallBack(IMarketDataEntity mktData)
        {
            OnMarketDataUpdate(mktData);
        }

        /// <summary>
        /// Invoked when market data update is received
        /// </summary>
        /// <param name="mktData">market data that is received</param>
        public void OnMarketDataUpdate(IMarketDataEntity mktData)
        {
            _logger.LogDebug("OnMarketDataUpdate " + mktData.Symbol + mktData.LastPrice.ToString());

            _mktDataQueue.Enqueue(mktData);          
        }

        /// <summary>
        /// method which is called for market data updated after conflation duration has been passed
        /// </summary>
        /// <param name="args">market data updates</param>
        public void OnSignalled( IEnumerable<GenericsEventArgs<IMarketDataEntity>> args) 
        {
            foreach (GenericsEventArgs<IMarketDataEntity> item in args)
            {
                IMarketDataEntity mktData = item.Item;

                _logger.LogDebug("OnSignalled " + mktData.Symbol + mktData.LastPrice.ToString());

                _portfolioDao.Save(mktData.Symbol, mktData);
            }          
            
        }

        /// <summary>
        /// event handler to process the database updates
        /// </summary>
        /// <param name="trades">trades retrieved from the database</param>
        void GetAllTradesComplete(IEnumerable<IPortfolioDataEntity> trades)
        {
            _logger.LogDebug("GetAllTradesComplete ");

            ProcessAllTrades(trades);

            SubscribeMarketData();
        }

        #endregion

    }
}

