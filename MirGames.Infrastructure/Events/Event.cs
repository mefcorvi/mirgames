namespace MirGames.Infrastructure.Events
{
    /// <summary>
    /// Describes the abstract game event.
    /// </summary>
    public abstract class Event
    {
        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        protected internal abstract string EventType { get; }
    }
}