namespace MirGames.Infrastructure.Events
{
    using System;

    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// Raised when event passed to the listener could not be converted to event supported by listener.
    /// </summary>
    public class UnsupportedEventTypeException : MirGamesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedEventTypeException"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="expectedType">The expected type.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnsupportedEventTypeException(Type targetType, Type expectedType, Exception innerException)
            : base(string.Format("Events of type {0} could not be processed by listeners of type {1}.", targetType, expectedType), innerException)
        {
        }
    }
}