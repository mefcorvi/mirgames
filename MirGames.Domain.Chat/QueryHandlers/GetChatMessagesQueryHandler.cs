namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Get Chat Messages query.
    /// </summary>
    internal sealed class GetChatMessagesQueryHandler : QueryHandler<GetChatMessagesQuery, ChatMessageViewModel>
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
        /// Initializes a new instance of the <see cref="GetChatMessagesQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textTransform">The text transform.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetChatMessagesQueryHandler(IQueryProcessor queryProcessor, ITextTransform textTransform, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.textTransform = textTransform;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetChatMessagesQuery query, ClaimsPrincipal principal)
        {
            return this.GetMessagesQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<ChatMessageViewModel> Execute(IReadContext readContext, GetChatMessagesQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var messages = this.ApplyPagination(this.GetMessagesQuery(readContext, query).OrderByDescending(m => m.MessageId), pagination).ToList();

            var viewModels = messages
                .Select(
                    m => new ChatMessageViewModel
                        {
                            Author = new AuthorViewModel
                                {
                                    Id = m.AuthorId,
                                    Login = m.AuthorLogin
                                },
                            CreatedDate = m.CreatedDate,
                            UpdatedDate = m.UpdatedDate,
                            MessageId = m.MessageId,
                            Text = m.Message,
                            CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", m),
                            CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", m)
                        }).ToList();

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = viewModels.Select(m => m.Author)
                    });

            viewModels.ForEach(m => m.Text = this.textTransform.Transform(m.Text));

            return viewModels;
        }

        /// <summary>
        /// Gets the messages query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The messages query.</returns>
        private IQueryable<ChatMessage> GetMessagesQuery(IReadContext readContext, GetChatMessagesQuery query)
        {
            IQueryable<ChatMessage> set = readContext.Query<ChatMessage>();

            if (query.LastIndex.HasValue)
            {
                set = set.Where(m => m.MessageId < query.LastIndex);
            }

            if (query.FirstIndex.HasValue)
            {
                set = set.Where(m => m.MessageId > query.FirstIndex);
            }

            return set;
        }
    }
}
