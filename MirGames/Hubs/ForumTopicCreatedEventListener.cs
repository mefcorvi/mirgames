namespace MirGames.Hubs
{
    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the new chat messages.
    /// </summary>
    public class ForumTopicCreatedEventListener : EventListenerBase<ForumTopicCreatedEvent>
    {
        /// <inheritdoc />
        public override void Process(ForumTopicCreatedEvent @event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
            context.Clients.All.NewTopic(new ForumTopicCreatedEventViewModel { AuthorId = @event.AuthorId, TopicId = @event.TopicId });
        }

        /// <summary>
        /// View Model for topic created event.
        /// </summary>
        public class ForumTopicCreatedEventViewModel
        {
            /// <summary>
            /// Gets or sets the author unique identifier.
            /// </summary>
            public int? AuthorId { get; set; }

            /// <summary>
            /// Gets or sets the topic unique identifier.
            /// </summary>
            public int TopicId { get; set; }
        }
    }
}