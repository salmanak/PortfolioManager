using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;


namespace PortfolioManager.Data
{
    public class TradeDataMapper
    {
        private DatabaseConnection _dbConnection;

        public TradeDataMapper()
        {
            _dbConnection = new DatabaseConnection();
        }

        public IEnumerable<PortfolioDataEntity> GetAllTrades()
        {
            using (IDbConnection dbConnection = _dbConnection.CreateConnection())
            {
                dbConnection.Open();
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

        public int SaveTrade(String symbol, long shares, double price)
        {
            try
            {
                using (IDbConnection dbConnection = _dbConnection.CreateConnection())
                {
                    dbConnection.Open();
                    using (IDbCommand cmd = dbConnection.CreateCommand())
                    {
                        cmd.CommandText = "sp_InsertTrade";
                        cmd.CommandType = CommandType.StoredProcedure;

                        IDbDataParameter param;

                        //TODO: Refactor
                        param = cmd.CreateParameter(); param.ParameterName = "@Symbol"; param.Value = symbol; cmd.Parameters.Add(param);
                        param = cmd.CreateParameter(); param.ParameterName = "@Shares"; param.Value = shares; cmd.Parameters.Add(param);
                        param = cmd.CreateParameter(); param.ParameterName = "@Price"; param.Value = price; cmd.Parameters.Add(param);

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
