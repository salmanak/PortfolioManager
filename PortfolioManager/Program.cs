using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PortfolioManager.MarketData;
using PortfolioManager.Common;
using PortfolioManager.View;
using PortfolioManager.Data;

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

            LoggingService.Init();

            IPortfolioDao dao = new PortfolioDao();
            //IMarketDataAdapter<MarketDataEntity> mktDataAdapter = new MarketDataAdapter();

            PortfolioView view = new PortfolioView(dao);//,mktDataAdapter);

            Application.Run(view);
        }
    }
}
