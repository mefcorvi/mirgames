namespace MirGames.Hubs
{
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Users.Events;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    using Newtonsoft.Json;

    /// <summary>
    /// Listens the new chat messages.
    /// </summary>
    public class UserOnlineListener : EventListenerBase<UserOnlineEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOnlineListener" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public UserOnlineListener(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(UserOnlineEvent @event)
        {
            var user = this.queryProcessor.Process(
                new GetUsersQuery
                {
                    UserIdentifiers = new[] { @event.UserId }
                }).FirstOrDefault();

            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
            context.Clients.All.userOnline(JsonConvert.SerializeObject(user));
        }
    }
}