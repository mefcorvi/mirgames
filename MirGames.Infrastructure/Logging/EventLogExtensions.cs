namespace MirGames.Infrastructure.Logging
{
    using System;

    /// <summary>
    /// Extensions for event logging.
    /// </summary>
    public static class EventLogExtensions
    {
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="e">The exception.</param>
        public static void LogError(this IEventLog eventLog, string source, Exception e)
        {
            eventLog.Log(EventLogType.Error, source, e.Message, e);
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogError(this IEventLog eventLog, string source, string format, params object[] arguments)
        {
            eventLog.Log(EventLogType.Error, source, string.Format(format, arguments), null);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="e">The exception.</param>
        public static void LogWarning(this IEventLog eventLog, string source, Exception e)
        {
            eventLog.Log(EventLogType.Warning, source, e.Message, e);
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogWarning(this IEventLog eventLog, string source, string format, params object[] arguments)
        {
            eventLog.Log(EventLogType.Warning, source, string.Format(format, arguments), null);
        }

        /// <summary>
        /// Logs the information.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogInformation(this IEventLog eventLog, string source, string format, params object[] arguments)
        {
            eventLog.Log(EventLogType.Information, source, string.Format(format, arguments), null);
        }

        /// <summary>
        /// Logs the verbose.
        /// </summary>
        /// <param name="eventLog">The event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="format">The format.</param>
        /// <param name="arguments">The arguments.</param>
        public static void LogVerbose(this IEventLog eventLog, string source, string format, params object[] arguments)
        {
            eventLog.Log(EventLogType.Verbose, source, string.Format(format, arguments), null);
        }
    }
}