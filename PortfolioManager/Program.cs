using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PortfolioManager.Common;
using PortfolioManager.Common.Interfaces.Model;
using PortfolioManager.Common.Interfaces.View;
using PortfolioManager.Common.Interfaces.Presenter;
using PortfolioManager.Model;
using PortfolioManager.View;
using PortfolioManager.Presenter;

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
            IPortfolioPresenter presenter = new PortfolioPresenter(dao);
            IPortfolioView view = new PortfolioView(presenter);

            Application.Run((Form)view);
        }

    }
}
