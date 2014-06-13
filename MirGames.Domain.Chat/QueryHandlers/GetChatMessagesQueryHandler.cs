// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetChatMessagesQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Domain.Security;
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
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChatMessagesQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textProcessor">The text transform.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetChatMessagesQueryHandler(IQueryProcessor queryProcessor, ITextProcessor textProcessor, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.textProcessor = textProcessor;
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
                            CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "ChatMessage", m.MessageId),
                            CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "ChatMessage", m.MessageId)
                        }).ToList();

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = viewModels.Select(m => m.Author)
                    });

            viewModels.ForEach(m => m.Text = this.textProcessor.GetHtml(m.Text));

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
