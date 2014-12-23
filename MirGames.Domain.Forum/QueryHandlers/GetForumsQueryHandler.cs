// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetForumsQueryHandler : QueryHandler<GetForumsQuery, ForumViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetForumsQueryHandler(IQueryProcessor queryProcessor, IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetForumsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetQuery(readContext).Count();
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<ForumViewModel> Execute(
            GetForumsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            IEnumerable<ForumInfo> forums;

            using (var readContext = this.readContextFactory.Create())
            {
                var forumTopics = readContext.Query<ForumTopic>();

                forums = readContext
                    .Query<Forum>()
                    .GroupJoin(
                        forumTopics,
                        forum => forum.ForumId,
                        topic => topic.ForumId,
                        (forum, topics) =>
                        new ForumInfo
                        {
                            Forum = forum,
                            TopicsCount = topics.Count(),
                            PostsCount = topics.Sum(t => t.PostsCount),
                            LastTopic = topics.OrderByDescending(t => t.UpdatedDate).FirstOrDefault()
                        })
                    .ToList();
            }

            var forumViewModels = forums
                .Select(f =>
                {
                    var forum = new ForumViewModel
                    {
                        Description = f.Forum.Description,
                        ForumId = f.Forum.ForumId,
                        Title = f.Forum.Title,
                        PostsCount = f.PostsCount,
                        TopicsCount = f.TopicsCount,
                        IsRetired = f.Forum.IsRetired,
                        Alias = f.Forum.Alias
                    };

                    if (f.LastTopic != null)
                    {
                        forum.LastAuthor = new AuthorViewModel
                        {
                            Login = f.LastTopic.LastPostAuthorLogin,
                            Id = f.LastTopic.LastPostAuthorId
                        };

                        forum.LastPostDate = f.LastTopic.UpdatedDate;
                        forum.LastTopicId = f.LastTopic.TopicId;
                        forum.LastTopicTitle = f.LastTopic.Title;
                    }

                    return forum;
                })
                .ToList();

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = forumViewModels.Select(f => f.LastAuthor)
            });

            return forumViewModels;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <returns>The query.</returns>
        private IQueryable<Forum> GetQuery(IReadContext readContext)
        {
            return readContext.Query<Forum>();
        }

        /// <summary>
        /// The forum info.
        /// </summary>
        private sealed class ForumInfo
        {
            /// <summary>
            /// Gets or sets the forum.
            /// </summary>
            public Forum Forum { get; set; }

            /// <summary>
            /// Gets or sets the topics count.
            /// </summary>
            public int TopicsCount { get; set; }

            /// <summary>
            /// Gets or sets the posts count.
            /// </summary>
            public int PostsCount { get; set; }

            /// <summary>
            /// Gets or sets the last topic.
            /// </summary>
            public ForumTopic LastTopic { get; set; }
        }
    }
}
