using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.MarketData;
using PortfolioManager.Common;
using PortfolioManager.Data;
using PortfolioManager.View;

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

                foreach (var item in _portfolioDao.GetAllPortfolioItems())
                    _mktDataAdapter.Subscribe(item.Symbol);

                _mktDataAdapter.Start();
            }
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
            foreach (var item in _tradeDataMapper.GetAllTrades())
                Save(item);

            Update();

            _mktDataQueue = new AutoQueue<MarketDataEntity>(250); // TODO: get from config
            _mktDataQueue.signalEvent += new AutoQueueEventHandler<AutoQueue<MarketDataEntity>, IEnumerable<AutoQueue<MarketDataEntity>.GenericsEventArgs<MarketDataEntity>>>(OnSignalled);
            _mktDataQueue.Start();
        } 
        #endregion

        #region General Methods
        /// <summary>
        /// called at startup to ensure view is updated
        /// </summary>
        private void Update()
        {
            _portfolioView.ShowPortfolioItems(_portfolioDao.GetAllPortfolioItems());
            _portfolioView.UpdatePortfolioItems(); //TODO: might not be needed

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

            return _tradeDataMapper.SaveTrade(symbol, shares, price);
        } 
        #endregion

        #region Events
        /// <summary>
        /// Invoked when market data update is received
        /// </summary>
        /// <param name="mktData">market data that is received</param>
        public void OnMarketDataUpdate(MarketDataEntity mktData)
        {
            _mktDataQueue.Enqueue(mktData);          
        }

        public void OnSignalled(AutoQueue<MarketDataEntity> q,  IEnumerable<AutoQueue<MarketDataEntity>.GenericsEventArgs<MarketDataEntity>> args) 
        {
            foreach (AutoQueue<MarketDataEntity>.GenericsEventArgs<MarketDataEntity>item in args )
            {
                MarketDataEntity mktData = item.Item;
                _portfolioDao.Save(mktData.Symbol, mktData);
            }
            
        }
        #endregion


    }
}
