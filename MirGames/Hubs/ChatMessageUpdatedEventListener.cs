namespace MirGames.Hubs
{
    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Chat.Events;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    using Newtonsoft.Json;

    /// <summary>
    /// Listens the new chat messages.
    /// </summary>
    public class ChatMessageUpdatedEventListener : EventListenerBase<ChatMessageUpdatedEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageUpdatedEventListener" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public ChatMessageUpdatedEventListener(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(ChatMessageUpdatedEvent @event)
        {
            var message = this.queryProcessor.Process(
                new GetChatMessageQuery
                    {
                        MessageId = @event.MessageId
                    });

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.updateMessage(message);
        }
    }
}