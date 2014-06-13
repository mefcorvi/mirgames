// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetCommentsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    internal sealed class GetCommentsQueryHandler : QueryHandler<GetCommentsQuery, CommentViewModel>
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
        /// Initializes a new instance of the <see cref="GetCommentsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textProcessor">The text transform.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetCommentsQueryHandler(IQueryProcessor queryProcessor, ITextProcessor textProcessor, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.textProcessor = textProcessor;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetCommentsQuery query, ClaimsPrincipal principal)
        {
            return GetCommentsSet(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<CommentViewModel> Execute(IReadContext readContext, GetCommentsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var comments =
                this.ApplyPagination(GetCommentsSet(readContext, query).OrderByDescending(c => c.Date), pagination).ToList();

            var topicIds = comments.Select(c => c.TopicId).ToArray();
            var topics = readContext.Query<Topic>().Where(t => topicIds.Contains(t.Id)).ToDictionary(t => t.Id, t => t.TopicTitle);

            var commentViewModels = comments
                .Select(
                    c => new CommentViewModel
                        {
                            Author = new AuthorViewModel
                                {
                                    Id = c.UserId,
                                    Login = c.UserLogin
                                },
                            CreationDate = c.Date,
                            UpdatedDate = c.UpdatedDate,
                            Text = c.Text ?? this.textProcessor.GetHtml(c.SourceText),
                            Id = c.CommentId,
                            TopicId = c.TopicId,
                            TopicTitle = topics.ContainsKey(c.TopicId) ? topics[c.TopicId] : null,
                            CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "Comment", c.CommentId),
                            CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "Comment", c.CommentId)
                        }).ToList();

            this.queryProcessor.Process(new ResolveAuthorsQuery { Authors = commentViewModels.Select(c => c.Author) });

            return commentViewModels;
        }

        /// <summary>
        /// Gets the comments set.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The comments set.</returns>
        private static IQueryable<Comment> GetCommentsSet(IReadContext readContext, GetCommentsQuery query)
        {
            IQueryable<Comment> comments = readContext.Query<Comment>();
            
            if (query.AuthorId.HasValue)
            {
                comments = comments.Where(c => c.UserId == query.AuthorId.Value);
            }

            return comments;
        }
    }
}