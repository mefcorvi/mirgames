namespace MirGames.Infrastructure.Notifications
{
    /// <summary>
    /// The notification base class.
    /// </summary>
    public abstract class Notification
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        public abstract string Title { get; }

        /// <summary>
        /// Gets the body.
        /// </summary>
        public abstract string Body { get; }
    }
}
