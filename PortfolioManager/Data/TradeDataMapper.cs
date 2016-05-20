using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using PortfolioManager.Common;
using System.Threading;


namespace PortfolioManager.Data
{
    /// <summary>
    /// Data Access Layer for bidirection data access to persistent data store
    /// </summary>
    public class TradeDataMapper
    {
        #region Declarations and Definitions
        /// <summary>
        /// For Logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(TradeDataMapper));
        /// <summary>
        /// Database Connection object
        /// </summary>
        private DatabaseConnection _dbConnection;

        private IExceptionHandler _exceptionHandler;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TradeDataMapper(IExceptionHandler exceptionHandler)
        {
            _exceptionHandler = exceptionHandler;
            _dbConnection = new DatabaseConnection();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets all trades from the database
        /// </summary>
        /// <returns>All trades</returns>
        public IEnumerable<PortfolioDataEntity> GetAllTrades()
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

                    if ( !string.IsNullOrEmpty(errorString) )
                        _exceptionHandler.HandleException(new Exception(errorString)); // To let user know about the exception
                }
            }
        }


        /// <summary>
        /// Saves the Trade in the database Asynchronously
        /// </summary>
        /// <param name="symbol">symbol for the trade</param>
        /// <param name="shares">shares for the trade</param>
        /// <param name="price">price for the trade</param>
        /// <returns> Passes the exception/error upstream to be shown to the user</returns>
        public void SaveTradeAsync(String symbol, long shares, double price)
        {
            _logger.Log("Saving Trade in DB.");

            ThreadPool.QueueUserWorkItem(
                delegate
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
                });
        }

        private string HandleException(string errorString, Exception ex)
        {
            _logger.LogError(errorString, ex);
            _exceptionHandler.HandleException(new Exception(errorString, ex)); // To let user know about the exception
            return "";
        }

        private string HandleException(string errorString, SqlException SqlEx)
        {
            _logger.LogError("Errors Count:" + SqlEx.Errors.Count.ToString());
            foreach (SqlError error in SqlEx.Errors)
                _logger.LogError(error.Number + " - " + error.Message);
            _exceptionHandler.HandleException(new Exception(errorString, SqlEx)); // To let user know about the exception
            return "";
        }

        private static void AddParameter<T>(IDbCommand cmd, string paramName, T paramValue)
        {
            IDbDataParameter param;
            param = cmd.CreateParameter();
            param.ParameterName = paramName;
            param.Value = paramValue;
            cmd.Parameters.Add(param);
        }

        #endregion
    }
}

