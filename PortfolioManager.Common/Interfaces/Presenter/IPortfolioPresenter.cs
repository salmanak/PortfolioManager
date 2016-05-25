using System;
namespace PortfolioManager.Common.Interfaces.Presenter
{
    public interface IPortfolioPresenter
    {
        /// <summary>
        /// Setter injection to inject dao
        /// </summary>
        /// <param name="dao"></param>
        void SetModel(PortfolioManager.Common.Interfaces.Model.IPortfolioDao dao);

        /// <summary>
        /// Setter injection to inject View
        /// </summary>
        /// <param name="view"></param>
        void SetView(PortfolioManager.Common.Interfaces.View.IPortfolioView view);

        /// <summary>
        /// Command handler from the View
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="shares"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        int AddPortfolioClicked(string symbol, long shares, double price);
        
        /// <summary>
        /// Method to be called from View when closing the application is required
        /// </summary>
        void Close();
    }
}
