﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortfolioManager.Common
{

    public interface ILogger
    {
        void Log(string message);
        void LogError(string message, Exception ex = null);
    }

    public class LoggingService:ILogger
    {
        private readonly log4net.ILog _logger;

        internal static void Init()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

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

        public void Log(string message)
        {
            _logger.Info(message);
        }

        public void LogError(string message, Exception ex = null)
        {
            _logger.Error(message, ex);
        }


    }
}
