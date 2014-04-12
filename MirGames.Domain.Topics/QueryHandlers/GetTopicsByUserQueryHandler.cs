// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetTopicsByUserQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the GetTopicQuery.
    /// </summary>
    internal sealed class GetTopicsByUserQueryHandler : QueryHandler<GetTopicsByUserQuery, TopicsListItem>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly Lazy<IQueryProcessor> queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicsByUserQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetTopicsByUserQueryHandler(IAuthorizationManager authorizationManager, Lazy<IQueryProcessor> queryProcessor)
        {
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override IEnumerable<TopicsListItem> Execute(IReadContext readContext, GetTopicsByUserQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var author = this.queryProcessor.Value.Process(
                new GetAuthorQuery
                    {
                        UserId = query.UserId
                    });

            return
                this.ApplyPagination(
                    readContext
                        .Query<Topic>()
                        .Include(t => t.Content)
                        .Where(t => t.AuthorId == query.UserId && t.IsPublished)
                        .OrderByDescending(t => t.CreationDate),
                    pagination)
                    .ToList()
                    .Select(t => this.GetTopic(t, author, principal))
                    .ToList();
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetTopicsByUserQuery query, ClaimsPrincipal principal)
        {
            return readContext.Query<Topic>().Count(t => t.AuthorId == query.UserId && t.IsPublished);
        }

        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <param name="author">The author.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The topics list item.</returns>
        private TopicsListItem GetTopic(Topic topic, AuthorViewModel author, ClaimsPrincipal principal)
        {
            return new TopicsListItem
                {
                    Author = author,
                    CanBeCommented = this.authorizationManager.CheckAccess(principal, "Comment", topic),
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", topic),
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", topic),
                    CreationDate = topic.CreationDate,
                    CommentsCount = topic.CountComment,
                    ShortText = topic.Content.TopicTextShort,
                    Tags = topic.TagsList,
                    TopicId = topic.Id,
                    Title = topic.TopicTitle
                };
        }
    }
}