using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PortfolioManager.MarketData;

namespace PortfolioManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IPortfolioDao dao = new PortfolioDao();
            IMarketData mktDataAdapter = new MarketDataAdapter();

            PortfolioView view = new PortfolioView(dao,mktDataAdapter);

            Application.Run(view);

            //Application.Run(new TestForm());
        }
    }
}
