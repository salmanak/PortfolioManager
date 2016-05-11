using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Configuration;
using System.Data.Common;
using PortfolioManager.Common;

namespace PortfolioManager.Data
{
    class DatabaseConnection
    {
        private ILogger _logger = new LoggingService(typeof(DatabaseConnection)); 

        public IDbConnection CreateConnection()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"];
                var providerName = connectionString.ProviderName;
                var factory = DbProviderFactories.GetFactory(providerName);
                var connection = factory.CreateConnection();
                connection.ConnectionString = connectionString.ConnectionString;
                return connection;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to create Connection.", ex);
                return null;
            }
        }
    }
}
