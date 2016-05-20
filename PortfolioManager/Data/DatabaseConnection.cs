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
    /// <summary>
    /// Factory class to create the database connection based on the configuration
    /// </summary>
    public class DatabaseConnection
    {
        #region Declarations and Definitions
        /// <summary>
        /// for logging
        /// </summary>
        private ILogger _logger = new LoggingService(typeof(DatabaseConnection));  
        #endregion

        #region Methods
        /// <summary>
        /// Creates a connection based on teh configuration
        /// </summary>
        /// <returns>returns the database connection object</returns>
        public IDbConnection CreateConnection(IExceptionHandler exceptionHandler = null)
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
                string errorString = "Unable to create Connection.";
                _logger.LogError(errorString, ex);
                exceptionHandler.HandleException(new Exception(errorString, ex));
                return null;
            }
        } 
        #endregion

    }
}
