namespace MirGames.Infrastructure.Events
{
    /// <summary>
    /// The event bus.
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// Adds the listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        void AddListener(IEventListener listener);

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        void Raise(Event @event);
    }
}