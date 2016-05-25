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
}
