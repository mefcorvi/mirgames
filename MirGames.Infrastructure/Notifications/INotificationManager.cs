namespace MirGames.Infrastructure.Notifications
{
    /// <summary>
    /// The notification manager.
    /// </summary>
    public interface INotificationManager
    {
        /// <summary>
        /// Sends the notification message.
        /// </summary>
        /// <typeparam name="T">The type of notification.</typeparam>
        /// <param name="recipient">The recipient.</param>
        /// <param name="notification">The notification.</param>
        void SendNotification<T>(string recipient, T notification) where T : Notification;
    }
}
