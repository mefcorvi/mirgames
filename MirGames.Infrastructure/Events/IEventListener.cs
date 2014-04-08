namespace MirGames.Infrastructure.Events
{
    using System.Collections.Generic;

    /// <summary>
    /// Listens the game events.
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Gets the supported event types.
        /// </summary>
        IEnumerable<string> SupportedEventTypes { get; }

        /// <summary>
        /// Determines whether this instance can process the specified game event.
        /// </summary>
        /// <param name="event">The game event.</param>
        /// <returns>True whether this instance can process the specified game event.</returns>
        bool CanProcess(Event @event);

        /// <summary>
        /// Processes the specified game event.
        /// </summary>
        /// <param name="event">The game event.</param>
        void Process(Event @event);
    }
}