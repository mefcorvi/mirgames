// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumPostsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the get forum topic query.
    /// </summary>
    public class GetForumPostsQueryHandler : QueryHandler<GetForumPostsQuery, ForumPostViewModel>
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
        /// Initializes a new instance of the <see cref="GetForumPostsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetForumPostsQueryHandler(IQueryProcessor queryProcessor, IReadContextFactory readContextFactory)
        {
            this.queryProcessor = queryProcessor;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetForumPostsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetForumPostsQuery(readContext, query).Count();
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<ForumPostViewModel> Execute(GetForumPostsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            List<ForumPostViewModel> posts;

            using (var readContext = this.readContextFactory.Create())
            {
                var postsWithPagination = this.GetForumPostsQuery(readContext, query);
                var topics = readContext.Query<ForumTopic>();
                var postsQuery = postsWithPagination.Join(
                    topics,
                    p => p.TopicId,
                    t => t.TopicId,
                    (p, t) => new ForumPostViewModel
                    {
                        Author = new AuthorViewModel
                        {
                            Id = p.AuthorId,
                            Login = p.AuthorLogin
                        },
                        AuthorIP = p.AuthorIP,
                        CreatedDate = p.CreatedDate,
                        IsHidden = p.IsHidden,
                        Text = p.Text,
                        PostId = p.PostId,
                        TopicId = p.TopicId,
                        UpdatedDate = p.UpdatedDate,
                        TopicTitle = t.Title,
                        ForumId = t.ForumId,
                        VotesRating = p.VotesRating,
                        CanBeVoted = false
                    }).OrderByDescending(p => p.PostId);

                posts = this.ApplyPagination(postsQuery, pagination).ToList();
            }

            if (principal.IsInRole("User"))
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                int[] postIdentifiers = posts.Select(p => p.PostId).ToArray();
                Dictionary<int, ForumPostVote> userVotes;

                using (var readContext = this.readContextFactory.Create())
                {
                    userVotes = readContext.Query<ForumPostVote>()
                                           .Where(v => v.UserId == userId && postIdentifiers.Contains(v.PostId))
                                           .ToDictionary(v => v.PostId);
                }

                posts.ForEach(post =>
                {
                    post.UserVote = userVotes.ContainsKey(post.PostId) ? userVotes[post.PostId].Vote : (int?)null;
                    post.CanBeVoted = post.Author.Id != userId;
                });
            }

            var forums = this.queryProcessor.Process(new GetForumsQuery());

            posts.ForEach(post => post.ForumAlias = forums.Where(f => f.ForumId == post.ForumId).Select(f => f.Alias).FirstOrDefault());

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = posts.Select(p => p.Author)
                    });

            return posts;
        }

        /// <summary>
        /// Gets the forum posts query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The posts query.
        /// </returns>
        private IQueryable<ForumPost> GetForumPostsQuery(IReadContext readContext, GetForumPostsQuery query)
        {
            IQueryable<ForumPost> set = readContext.Query<ForumPost>();

            if (query.AuthorId.HasValue)
            {
                set = set.Where(p => p.AuthorId == query.AuthorId.Value);
            }

            return set;
        }
    }
}