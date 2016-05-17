# PortfolioManager

APPLICATION OVERVIEW:
The application is a sample application where user can enter Equity Trades for his portfolio positions. The application will manage and display the average price for individual positions and the unrealized profit and loss based on simulated market data prices.

COMPILATION:
The code can be compiled in Visual Studio 2010 (.NET Framework 4.0) using Microsoft C#.

DEPENDENCIES:
In order to run the application with full features, please use the SQL Script (MS SQL 2008) to create a database named "PortfolioManager". [folder: /PortfolioManager/Script/PortfolioManager.sql]
If you are behind a proxy server, then you can specify the proxy address:proxy port in the config file as [section:  marketDataSettings\proxyAddress].

RUNNING THE APPLICATION:
Once launched, you can Add Trades from the top left area. Please enter Symbol Name, Number of Shares and the Trade Price. Press Add Trade button to add the trade. You will notice that the Trade is visible in the grid. You will also notice that the Market Values are also being updated in the grid for each position (the base market price is fetched from external market data provider, while the ticks are being simulated from the application itself). You will also notice the aggregate position of your portfolio in the bottom left section.

