using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PortfolioManager.Common;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using PortfolioManager.Common.Interfaces.Model;

namespace PortfolioManager.Model
{
     /// <summary>
     /// Concrete Model class for portfolio
     /// </summary>
    public class PortfolioDao:IPortfolioDao
    {
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(PortfolioDao));
        /// <summary>
        /// List of all protfolio items
        /// </summary>
        private BindingList<IPortfolioDataEntity> _portfolioItems = new BindingList<IPortfolioDataEntity>();
        /// <summary>
        /// Portfolio Aggregate object binded with teh View
        /// </summary>
        private IPortfolioAggregate _portfolioAggregate = new PortfolioAggregate();

        /// <summary>
        /// Database Connection object
        /// </summary>
        private DatabaseConnection _dbConnection;

        /// <summary>
        /// exception handler to notify GUI for exception
        /// </summary>
        private IExceptionHandler _exceptionHandler;

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public PortfolioDao()
        {
            // Initialize database connection
            _dbConnection = new DatabaseConnection();
        }
        #endregion

        #region Create Methods
        /// <summary>
        /// Creates the portfolio object
        /// </summary>
        public IPortfolioDataEntity CreatePortfolioDataEntity()
        {
            return new PortfolioDataEntity();
        } 
        #endregion

        #region Access Methods
        /// <summary>
        /// Returns all the portfolio items
        /// </summary>
        public BindingList<IPortfolioDataEntity> GetAllPortfolioItems()
        {
            return _portfolioItems;
        }
        /// <summary>
        /// Provides aggregate access to the view
        /// </summary>
        public IPortfolioAggregate GetPortfolioAggregate()
        {
            return _portfolioAggregate;
        }
        /// <summary>
        /// Returns the portfolio item by name
        /// </summary>
        /// <param name="symbol">Symbol to search for</param>
        public IPortfolioDataEntity GetBySymbol(string symbol)
        {
            return _portfolioItems.FirstOrDefault(p => p.Symbol == symbol);
        } 
        #endregion

        #region Save Methods
        /// <summary>
        /// Saves the portfolio trade in cache
        /// </summary>
        /// <param name="symbol">Symbol of portfolio</param>
        /// <param name="mktData">Market data to be saved</param>
        public void Save(String symbol, IMarketDataEntity mktData)
        {
            var result = _portfolioItems.Where(p => p.Symbol == symbol).FirstOrDefault();
            if (result != null)
            {
                result.MarketPrice = Math.Round(mktData.LastPrice, 2);
                result.MarketValue = Math.Round(result.Shares * mktData.LastPrice, 2);
                result.UnrealizedGain = Math.Round(result.MarketValue - result.Cost, 2);
            }
            else
            {
                // Should not happen
            }
            
            _portfolioAggregate.UnrealizedGainAggregate = Math.Round(_portfolioItems.Sum(s => s.UnrealizedGain), 2);
        }

        /// <summary>
        /// Save all the portfolios in cache
        /// </summary>
        /// <param name="portfolios"></param>
        private void Save(IEnumerable<IPortfolioDataEntity> portfolios)
        {
            portfolios
                .ToList()
                .ForEach(x => SaveInternal(x));
        }

        /// <summary>
        /// Saves  market data for portfolio in cache
        /// </summary>
        /// <param name="portfolio">the portfolio item to save</param>
        public void Save(IPortfolioDataEntity portfolio)
        {
            PersistTradeAsync(portfolio);

            SaveInternal(portfolio);
        }

        /// <summary>
        /// Only save in Cache. Does not save in persistant storage
        /// </summary>
        /// <param name="portfolio"></param>
        private void SaveInternal(IPortfolioDataEntity portfolio)
        {
            _logger.Log("Saving for _symbol : " + portfolio.Symbol);

            var result = _portfolioItems.Where(p => p.Symbol == portfolio.Symbol).FirstOrDefault();
            if (result != null)
            {
                double AvgPx;
                long CumQty;

                CumQty = result.Shares + portfolio.Shares;
                AvgPx = Math.Round((((result.Shares * result.Price) + (portfolio.Shares * portfolio.Price)) / CumQty), 2);


                result.Symbol = portfolio.Symbol;

                result.Price = AvgPx;
                result.Shares = CumQty;
                result.Cost = Math.Round(CumQty * AvgPx, 2);

                result.MarketPrice = Math.Round(portfolio.MarketPrice, 2);
                result.MarketValue = Math.Round(portfolio.MarketValue, 2);
                result.UnrealizedGain = Math.Round(portfolio.UnrealizedGain, 2);
            }
            else
            {
                portfolio.Cost = Math.Round(portfolio.Shares * portfolio.Price, 2);

                _portfolioItems.Add(portfolio);
            }

            _portfolioAggregate.CostAggregate = Math.Round(_portfolioItems.Sum(s => s.Cost), 2);
        } 
        #endregion

        #region Recovery and Persistance Methods

        /// <summary>
        /// Loads and then Saves Trades from the persisted storage
        /// </summary>
        public void Load()
        {
            Save(RecoverPersistedTrades());
        }

        /// <summary>
        /// Recovers the Trades from Persistant Storage
        /// </summary>
        /// <returns>Trades recovered from persistant storage</returns>
        private IEnumerable<IPortfolioDataEntity> RecoverPersistedTrades()
        {
            _logger.Log("Getting Trades from DB.");

            string errorString = ""; // workaround as yield is not allowed within Try/Catch

            using (IDbConnection dbConnection = _dbConnection.CreateConnection(_exceptionHandler))
            {
                try
                {
                    errorString = "Error in opening DB Connection.";
                    dbConnection.Open();
                    errorString = "";
                }
                catch (Exception ex)
                {
                    errorString = HandleException(errorString, ex);
                    yield break;
                }


                IDbCommand cmd = null;
                IDataReader reader = null;

                try
                {
                    try
                    {
                        errorString = "Error is creating database command.";
                        cmd = dbConnection.CreateCommand();
                        cmd.CommandText = "sp_GetTrades";
                        cmd.CommandType = CommandType.StoredProcedure;
                        errorString = "";
                    }
                    catch (SqlException SqlEx)
                    {
                        errorString = HandleException(errorString, SqlEx);
                        yield break;
                    }
                    catch (Exception ex)
                    {
                        errorString = HandleException(errorString, ex);
                        yield break;
                    }

                    try
                    {
                        errorString = "Unable to get Trades from the DB.";
                        reader = cmd.ExecuteReader();
                        errorString = "";
                    }
                    catch (SqlException SqlEx)
                    {
                        errorString = HandleException(errorString, SqlEx);
                        yield break;
                    }
                    catch (Exception ex)
                    {
                        errorString = HandleException(errorString, ex);
                        yield break;
                    }

                    errorString = "Unable to read trades from DB.";
                    while (reader.Read())
                    {
                        var trades = new PortfolioDataEntity();

                        trades.Symbol = reader["Symbol"].ToString();
                        trades.Shares = long.Parse(reader["Shares"].ToString());
                        trades.Price = Math.Round(double.Parse(reader["Price"].ToString()), 2);

                        yield return trades;
                    }
                    errorString = "";
                }
                finally // workaround as yield is not allowed within Try/Catch
                {
                    if (cmd != null)
                        cmd.Dispose();
                    if (reader != null && !reader.IsClosed)
                        reader.Close();

                    if (!string.IsNullOrEmpty(errorString))
                        _exceptionHandler.HandleException(new Exception(errorString)); // To let user know about the exception
                }
            }
        }

        /// <summary>
        /// Persists the trades Asynchronously
        /// </summary>
        /// <param name="portfolio">item to be persisted</param>
        private void PersistTradeAsync(IPortfolioDataEntity portfolio)
        {
            PersistTradeAsync(portfolio.Symbol, portfolio.Shares, portfolio.Price);
        }

        /// <summary>
        /// Asynchronouslyt saves the Trade in the database Asynchronously
        /// </summary>
        /// <param name="symbol">symbol for the trade</param>
        /// <param name="shares">shares for the trade</param>
        /// <param name="price">price for the trade</param>
        /// <returns></returns>
        private void PersistTradeAsync(String symbol, long shares, double price)
        {
            _logger.Log("Saving Trade in DB.");

            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    PersistTrade(symbol, shares, price);
                });
        }

        /// <summary>
        /// Saves the Trade in the database Asynchronously
        /// </summary>
        /// <param name="symbol">symbol for the trade</param>
        /// <param name="shares">shares for the trade</param>
        /// <param name="price">price for the trade</param>
        /// <returns></returns>
        private void PersistTrade(String symbol, long shares, double price)
        {
            using (IDbConnection dbConnection = _dbConnection.CreateConnection(_exceptionHandler))
            {
                try
                {
                    dbConnection.Open();
                }
                catch (Exception ex)
                {
                    string errorString = "Error in opening DB Connection.";
                    errorString = HandleException(errorString, ex);

                    return;
                }

                try
                {
                    using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "sp_InsertTrade";
                        cmd.CommandType = CommandType.StoredProcedure;

                        AddParameter(cmd, "@Symbol", symbol);
                        AddParameter(cmd, "@Shares", shares);
                        AddParameter(cmd, "@Price", price);


                        cmd.ExecuteNonQuery();

                        return; //Successful

                    }
                }
                catch (SqlException SqlEx)
                {
                    String errorString = "Unable to save trade in DB for symbol " + symbol;
                    errorString = HandleException(errorString, SqlEx);
                    return;
                }
                catch (Exception ex)
                {
                    String errorString = "Unable to save trade in DB for symbol " + symbol;
                    errorString = HandleException(errorString, ex);
                    return;
                }
            }
        }

        /// <summary>
        /// Refactored method to add parameter in the command
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cmd"></param>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        private static void AddParameter<T>(IDbCommand cmd, string paramName, T paramValue)
        {
            IDbDataParameter param;
            param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
        }
        #endregion

        #region Exception Handling Methods
        /// <summary>
        /// Method to save the exception handler reference 
        /// which can be used to notify GUI from non-GUI thread
        /// </summary>
        /// <param name="exceptionHandler">exception handler to notify GUI for exception</param>
        public void SetExceptionHandler(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
        }

        /// <summary>
        /// Refactored method for logging and passing exception upstream
        /// </summary>
        /// <param name="errorString"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string HandleException(string errorString, Exception ex)
        {
            _logger.LogError(errorString, ex);
            _exceptionHandler.HandleException(new Exception(errorString, ex)); // To let user know about the exception
            return "";
        }

        /// <summary>
        /// Refactored method for logging and passing SQL exception upstream
        /// </summary>
        /// <param name="errorString"></param>
        /// <param name="SqlEx"></param>
        /// <returns></returns>
        private string HandleException(string errorString, SqlException SqlEx)
        {
            _logger.LogError("Errors Count:" + SqlEx.Errors.Count.ToString());
            foreach (SqlError error in SqlEx.Errors)
                _logger.LogError(error.Number + " - " + error.Message);
            _exceptionHandler.HandleException(new Exception(errorString, SqlEx)); // To let user know about the exception
            return "";
        }
        #endregion

    }

}
