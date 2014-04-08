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
    public class ChatMessagesListener : EventListenerBase<NewChatMessageEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessagesListener" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public ChatMessagesListener(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(NewChatMessageEvent @event)
        {
            var message = this.queryProcessor.Process(
                new GetChatMessageQuery
                {
                    MessageId = @event.MessageId
                });

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.addNewMessageToPage(JsonConvert.SerializeObject(message));
        }
    }
}