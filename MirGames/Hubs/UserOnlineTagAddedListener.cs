namespace MirGames.Hubs
{
    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the new online user tags.
    /// </summary>
    internal sealed class UserOnlineTagAddedListener : EventListenerBase<OnlineUserTagAddedEvent>
    {
        /// <inheritdoc />
        public override void Process(OnlineUserTagAddedEvent @event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
            context.Clients.All.userOnlineTagAdded(@event.UserId, @event.Tag);
        }
    }
}