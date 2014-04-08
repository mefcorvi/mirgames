namespace MirGames.Hubs
{
    using System.Globalization;
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the forum topic read event.
    /// </summary>
    public class ForumTopicReadEventListener : EventListenerBase<ForumTopicReadEvent>
    {
        /// <inheritdoc />
        public override void Process(ForumTopicReadEvent @event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();

            if (@event.UserIdentifiers != null)
            {
                foreach (int userId in @event.UserIdentifiers)
                {
                    context.Clients.User(userId.ToString(CultureInfo.InvariantCulture)).ForumTopicRead();
                }
            }

            if (@event.ExcludedUsers != null)
            {
                var excludedConnections = @event.ExcludedUsers.SelectMany(EventsHub.GetUserConnections);
                context.Clients.AllExcept(excludedConnections.ToArray()).ForumTopicRead();
            }
        }
    }
}