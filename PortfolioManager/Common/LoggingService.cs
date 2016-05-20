using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common
{
    /// <summary>
    /// Interface for other classes to log
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
        void LogDebug(string message);
        void LogError(string message, Exception ex = null);
    }

    /// <summary>
    /// Wrapper Loggin Service class
    /// </summary>
    public class LoggingService:ILogger
    {
        #region Declarations and Definitions
        private readonly log4net.ILog _logger; 
        #endregion

        #region Constructors
        public LoggingService()
        {
            _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public LoggingService(string name)
        {
            _logger = log4net.LogManager.GetLogger(name);
        }

        public LoggingService(Type type)
        {
            _logger = log4net.LogManager.GetLogger(type);
        }
        internal static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
        #endregion

        #region Logging Methods
        public void LogDebug(string message)
        {
            _logger.Debug(message);
        }

        public void Log(string message)
        {
            _logger.Info(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            _logger.Error(message, ex);
        }
        
        #endregion

    }
}
