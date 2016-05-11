using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PortfolioManager.MarketData;
using PortfolioManager.Common;
using PortfolioManager.Data;

namespace PortfolioManager
{
    internal class PortfolioPresenter
    {
        private TradeDataMapper _tradeDataMapper;

        private readonly IPortfolioView _portfolioView;
        private readonly IPortfolioDao _portfolioDao;

        private IMarketData _mktDataAdapter;

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

        public PortfolioPresenter(IPortfolioView view, IPortfolioDao dao)
        {
            _portfolioView = view;
            _portfolioDao = dao;

            _tradeDataMapper = new TradeDataMapper();
            foreach (var item in _tradeDataMapper.GetAllTrades())
                Save(item);

            Update();
        }


        private void Update()
        {
            _portfolioView.ShowPortfolioItems(_portfolioDao.GetAllPortfolioItems());
            _portfolioView.UpdatePortfolioItems(); //TODO: might not be needed

            PortfolioAggregate p = _portfolioDao.GetPortfolioAggregate();
            _portfolioView.ShowPortfolioAggregate(p);

        }

        private void Save(PortfolioDataEntity portfolio)
        {
            _portfolioDao.Save(portfolio);
        }

        public int AddPortfolioClicked(String symbol, long shares, double price)
        {

            if (_tradeDataMapper.SaveTrade(symbol, shares, price) < 0)
                return -1;

            Save(new PortfolioDataEntity(symbol, shares, price));

            _portfolioView.UpdatePortfolioItems();

            _mktDataAdapter.Subscribe(symbol);

            return 0;
            
        }

        public void OnMarketDataUpdate(MarketDataEntity mktData)
        {
            _portfolioDao.Save(mktData.Symbol, mktData);
        }

        public void Close()
        {
            _mktDataAdapter.Stop();
        }
    }
}
