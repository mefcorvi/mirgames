namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Get Chat Message query.
    /// </summary>
    internal sealed class GetChatMessageForEditQueryHandler : SingleItemQueryHandler<GetChatMessageForEditQuery, ChatMessageForEditViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChatMessageForEditQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetChatMessageForEditQueryHandler(IAuthorizationManager authorizationManager)
        {
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override ChatMessageForEditViewModel Execute(IReadContext readContext, GetChatMessageForEditQuery query, ClaimsPrincipal principal)
        {
            var message = readContext.Query<ChatMessage>().First(m => m.MessageId == query.MessageId);
            
            if (message == null)
            {
                throw new ItemNotFoundException("ChatMessage", query.MessageId);
            }

            this.authorizationManager.EnsureAccess(principal, "Edit", message);

            return new ChatMessageForEditViewModel
                {
                    MessageId = message.MessageId,
                    SourceText = message.Message
                };
        }
    }
}
