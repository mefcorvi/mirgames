namespace MirGames.Hubs
{
    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the removed online user tags.
    /// </summary>
    internal sealed class UserOnlineTagRemovedListener : EventListenerBase<OnlineUserTagRemovedEvent>
    {
        /// <inheritdoc />
        public override void Process(OnlineUserTagRemovedEvent @event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
            context.Clients.All.userOnlineTagRemoved(@event.UserId, @event.Tag);
        }
    }
}