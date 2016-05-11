using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Configuration;
using System.Data.Common;

namespace PortfolioManager.Data
{
    class DatabaseConnection
    {
        public IDbConnection CreateConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnection"];
            var providerName = connectionString.ProviderName;
            var factory = DbProviderFactories.GetFactory(providerName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString.ConnectionString;
            return connection;
        }
    }
}
