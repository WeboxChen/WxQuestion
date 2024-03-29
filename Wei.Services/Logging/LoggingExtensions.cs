﻿using System;
using System.Text;
using Wei.Core.Domain.Logging;
using Wei.Core.Domain.Users;

namespace Wei.Services.Logging
{
    public static class LoggingExtensions
    {
        public static void Debug(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Debug, message, exception, user);
        }
        public static void Information(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Information, message, exception, user);
        }
        public static void Warning(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Warning, message, exception, user);
        }
        public static void Error(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Error, message, exception, user);
        }
        public static void Fatal(this ILogger logger, string message, Exception exception = null, User user = null)
        {
            FilteredLog(logger, LogLevel.Fatal, message, exception, user);
        }

        private static void FilteredLog(ILogger logger, LogLevel level, string message, Exception exception = null, User user = null)
        {
            //don't log thread abort exception
            if (exception is System.Threading.ThreadAbortException)
                return;

            if (logger.IsEnabled(level))
            {
                StringBuilder sbuilder = new StringBuilder();
                while(exception != null)
                {
                    sbuilder.Append(exception.ToString());
                    exception = exception.InnerException;
                }
                logger.InsertLog(level, message, sbuilder.ToString(), user);
            }
        }
    }
}
