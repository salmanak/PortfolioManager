using System;
namespace PortfolioManager.Common.Interfaces.Presenter
{
    public interface IPortfolioPresenter
    {
        int AddPortfolioClicked(string symbol, long shares, double price);
        void Close();
        void SetModel(PortfolioManager.Common.Interfaces.Model.IPortfolioDao dao);
        void SetView(PortfolioManager.Common.Interfaces.View.IPortfolioView view);
    }
}
