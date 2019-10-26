using Microsoft.Extensions.Logging;

namespace GasWeb.Domain.Initialization
{
    internal static class LoggingExtensions
    {
        public static void LogInitializationProcess(this ILogger logger, string message)
            => logger.LogInformation($"System initialization: {message}");
    }
}
