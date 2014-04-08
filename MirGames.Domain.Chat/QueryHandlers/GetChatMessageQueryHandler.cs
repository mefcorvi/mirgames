namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Get Chat Message query.
    /// </summary>
    internal sealed class GetChatMessageQueryHandler : SingleItemQueryHandler<GetChatMessageQuery, ChatMessageViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChatMessageQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textTransform">The text transform.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetChatMessageQueryHandler(IQueryProcessor queryProcessor, ITextTransform textTransform, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.textTransform = textTransform;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override ChatMessageViewModel Execute(IReadContext readContext, GetChatMessageQuery query, ClaimsPrincipal principal)
        {
            var messageEntity = readContext.Query<ChatMessage>().FirstOrDefault(m => m.MessageId == query.MessageId);

            if (messageEntity == null)
            {
                throw new ItemNotFoundException("ChatMessage", query.MessageId);
            }

            var message = new ChatMessageViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = messageEntity.AuthorId,
                            Login = messageEntity.AuthorLogin
                        },
                    CreatedDate = messageEntity.CreatedDate,
                    UpdatedDate = messageEntity.UpdatedDate,
                    MessageId = messageEntity.MessageId,
                    Text = messageEntity.Message,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", messageEntity),
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", messageEntity)
                };

            this.queryProcessor.Process(new ResolveAuthorsQuery { Authors = new[] { message.Author } });
            message.Text = this.textTransform.Transform(message.Text);

            return message;
        }
    }
}
