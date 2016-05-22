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
using PortfolioManager.MarketData.ServiceClients;

namespace PortfolioManager
{
    /// <summary>
    /// The Presenter class that controls the View and Model.
    /// This also controls the Market Data Access.
    /// </summary>
    internal class PortfolioPresenter : IExceptionHandler
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

        private IServiceClient _serviceClient;


        private void SubscribeMarketData()
        {
            _logger.LogDebug("SubscribeMarketData ");

            _serviceClient.SubscribeAll(_portfolioDao.GetAllPortfolioItems().Select(x => x.Symbol).ToList());

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
            _tradeDataMapper = new TradeDataMapper(this);

            UpdateGUI();

            GetDataFromDB();

            InitMarketData();
        }

        private void GetDataFromDB()
        {
            var task = Task.Factory.StartNew(() => GetAllTrades());
            task.ContinueWith(t => GetAllTradesComplete(t.Result));
        }

        private void InitMarketData()
        {
            _serviceClient = ServiceClientCreator.FactoryMethod();
            _serviceClient.RegisterCallBack(this.OnServiceClientSimulatorCallBack);
            InitMarketDataQueue();
            SubscribeMarketData();
            _serviceClient.Connect();
        }

        private IEnumerable<PortfolioDataEntity> GetAllTrades()
        {
            return _tradeDataMapper.GetAllTrades();
        }

        void GetAllTradesComplete(IEnumerable<PortfolioDataEntity> trades)
        {
            _logger.LogDebug("GetAllTradesComplete ");

            ProcessAllTrades(trades);

            SubscribeMarketData();
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
            _serviceClient.Disconnect();
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

            _serviceClient.Subscribe(symbol);

            _tradeDataMapper.SaveTradeAsync(symbol, shares, price);

            return 0;
        } 
        #endregion

        #region Events

        public void OnServiceClientSimulatorCallBack(MarketDataEntity mktData)
        {
            OnMarketDataUpdate(mktData);
        }

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
        
        public void HandleException(Exception ex)
        {
            ((IExceptionHandler)_portfolioView).HandleException(ex);
        }
    }
}
