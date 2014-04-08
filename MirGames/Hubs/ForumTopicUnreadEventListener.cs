namespace MirGames.Hubs
{
    using System.Globalization;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the new chat messages.
    /// </summary>
    public class ForumTopicUnreadEventListener : EventListenerBase<ForumTopicUnreadEvent>
    {
        /// <inheritdoc />
        public override void Process(ForumTopicUnreadEvent @event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();

            foreach (int userIdentifier in @event.UserIdentifiers)
            {
                context.Clients.User(userIdentifier.ToString(CultureInfo.InvariantCulture)).NewTopicUnread();
            }
        }
    }
}