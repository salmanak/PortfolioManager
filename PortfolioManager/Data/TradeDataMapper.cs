using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;
using PortfolioManager.Common;


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
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TradeDataMapper()
        {
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

            using (IDbConnection dbConnection = _dbConnection.CreateConnection())
            {
                try
                {
                    dbConnection.Open();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error in opening DB Connection.", ex);
                    yield break;
                }

                using (IDbCommand cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "sp_GetTrades";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var trades = new PortfolioDataEntity();

                            trades.Symbol = reader["Symbol"].ToString();
                            trades.Shares = long.Parse(reader["Shares"].ToString());
                            trades.Price = Math.Round(double.Parse(reader["Price"].ToString()), 2);

                            yield return trades;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Saves the Trade in the database
        /// </summary>
        /// <param name="symbol">symbol for the trade</param>
        /// <param name="shares">shares for the trade</param>
        /// <param name="price">price for the trade</param>
        /// <returns> (0) Success (-1) DB Error (-2) Db Connection Open error</returns>
        public int SaveTrade(String symbol, long shares, double price)
        {
            try
            {
                _logger.Log("Saving Trade in DB.");

                using (IDbConnection dbConnection = _dbConnection.CreateConnection())
                {
                    try
                    {
                        dbConnection.Open();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error in opening DB Connection.", ex);
                        return -2;
                    }
                    using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "sp_InsertTrade";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDbDataParameter param;

                        //TODO: Refactor
                        param = cmd.CreateParameter(); param.ParameterName = "@Symbol"; param.Value = symbol; cmd.Parameters.Add(param);
                        param = cmd.CreateParameter(); param.ParameterName = "@Shares"; param.Value = shares; cmd.Parameters.Add(param);
                        param = cmd.CreateParameter(); param.ParameterName = "@Price"; param.Value = price; cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();

                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to save trade in DB.", ex);
                return -2;
            }
        }
        
        #endregion
    }
}
