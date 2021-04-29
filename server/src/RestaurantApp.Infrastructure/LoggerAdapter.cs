using Microsoft.Extensions.Logging;
using RestaurantApp.Core.Interface;
using System;
using System.Diagnostics;

namespace RestaurantApp.Infrastructure
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> logger;

        private string GetMessageFormat(string message)
        {
            var st = new StackTrace();

            return $"[{typeof(T).FullName}.{st.GetFrame(2).GetMethod().Name}] {message}";
        }

        public LoggerAdapter(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, GetMessageFormat(message), args);
        }

        public void LogInformation(string message, params object[] args)
        {
            logger.LogInformation(GetMessageFormat(message), args);
        }

        public void LogWarning(string message, params object[] args)
        {
            logger.LogWarning(GetMessageFormat(message), args);
        }
    }
}
