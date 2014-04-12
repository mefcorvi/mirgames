// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTopicsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.QueryHandlers
{
    using System;
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
    using MirGames.Infrastructure.SearchEngine;

    /// <summary>
    /// Handles the GetForumTopicsQuery.
    /// </summary>
    internal sealed class GetForumTopicsQueryHandler : QueryHandler<GetForumTopicsQuery, ForumTopicsListItemViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly Lazy<IQueryProcessor> queryProcessor;

        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumTopicsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="searchEngine">The search engine.</param>
        public GetForumTopicsQueryHandler(Lazy<IQueryProcessor> queryProcessor, ISearchEngine searchEngine)
        {
            this.queryProcessor = queryProcessor;
            this.searchEngine = searchEngine;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetForumTopicsQuery query, ClaimsPrincipal principal)
        {
            var topics = this.GetTopicsQueryable(readContext, query, principal.GetUserId());

            if (!string.IsNullOrWhiteSpace(query.SearchString))
            {
                var searchResults = this.searchEngine.Search("ForumTopic", query.SearchString).Select(sr => sr.Id).ToArray();
                return topics.Count(t => searchResults.Contains(t.TopicId));
            }

            return topics.Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<ForumTopicsListItemViewModel> Execute(IReadContext readContext, GetForumTopicsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var set = this.GetTopicsQueryable(readContext, query, principal.GetUserId());
            ICollection<ForumTopicsListItemViewModel> topics;

            if (string.IsNullOrWhiteSpace(query.SearchString))
            {
                topics = this.ApplyPagination(set, pagination).EnsureCollection();
            }
            else
            {
                topics = this.GetSearchResult(query, pagination, set).EnsureCollection();
            }

            this.queryProcessor.Value.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = topics.Select(t => t.Author).Concat(topics.Select(t => t.LastPostAuthor))
                    });

            if (!principal.Identity.IsAuthenticated)
            {
                topics.ForEach(topic => topic.IsRead = true);
            }
            else
            {
                int? userId = principal.GetUserId().GetValueOrDefault();
                var ranges = readContext.Query<ForumTopicRead>().Where(r => r.UserId == userId).ToList();

                var topicIdentifiers = topics.Select(t => t.TopicId).ToArray();
                var unreadTopics = readContext.Query<ForumTopicUnread>();

                var unreadQuery = readContext
                    .Query<ForumPost>()
                    .Join(unreadTopics, p => p.TopicId, ur => ur.TopicId, (p, ur) => new { post = p, unreadTopics = ur })
                    .Where(x => x.post.CreatedDate >= x.unreadTopics.UnreadDate && x.unreadTopics.UserId == userId)
                    .GroupBy(x => x.post.TopicId)
                    .Where(x => topicIdentifiers.Contains(x.Key))
                    .Select(
                        g => new
                            {
                                group = g.Key,
                                count = g.Count()
                            });

                var unreadPosts = unreadQuery.ToDictionary(g => g.group, g => g.count);
                
                foreach (var topic in topics)
                {
                    topic.IsRead = ranges.Any(r => topic.TopicId >= r.StartTopicId && topic.TopicId <= r.EndTopicId);
                    topic.UnreadPostsCount = unreadPosts.ContainsKey(topic.TopicId)
                                                 ? unreadPosts[topic.TopicId]
                                                 : (int?)null;
                }
            }

            return topics;
        }

        /// <summary>
        /// Gets the topics query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>
        /// The prepared query.
        /// </returns>
        private IQueryable<ForumTopicsListItemViewModel> GetTopicsQueryable(IReadContext readContext, GetForumTopicsQuery query, int? userId)
        {
            IQueryable<ForumTopic> topics = readContext.Query<ForumTopic>();

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                var tags = readContext.Query<ForumTag>().Where(t => t.TagText == query.Tag);
                topics = topics.Join(
                    tags,
                    topic => topic.TopicId,
                    tag => tag.TopicId,
                    (topic, tag) => topic);
            }

            if (query.OnlyUnread && userId != null)
            {
                var topicReadItems = readContext.Query<ForumTopicRead>().Where(t => t.UserId == userId);
                topics = topics
                    .SelectMany(
                        t => topicReadItems
                                 .Where(x => t.TopicId >= x.StartTopicId && t.TopicId <= x.EndTopicId)
                                 .DefaultIfEmpty(),
                        (t, r) => new { t, r })
                    .Where(@t1 => @t1.r == null)
                    .Select(@t1 => @t1.t);
            }

            return topics.OrderByDescending(t => t.UpdatedDate).Select(t => new ForumTopicsListItemViewModel
            {
                Author = new AuthorViewModel
                {
                    Id = t.AuthorId,
                    Login = t.AuthorLogin
                },
                LastPostAuthor = new AuthorViewModel
                {
                    Id = t.LastPostAuthorId,
                    Login = t.AuthorLogin
                },
                AuthorIp = t.AuthorIp,
                CreatedDate = t.CreatedDate,
                PostsCount = t.PostsCount,
                Title = t.Title,
                TopicId = t.TopicId,
                TagsList = t.TagsList,
                UpdatedDate = t.UpdatedDate
            });
        }

        /// <summary>
        /// Gets the search result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="topics">The topics.</param>
        /// <returns>The search result.</returns>
        private IEnumerable<ForumTopicsListItemViewModel> GetSearchResult(GetForumTopicsQuery query, PaginationSettings pagination, IQueryable<ForumTopicsListItemViewModel> topics)
        {
            var searchResults = this.ApplyPagination(this.searchEngine.Search("ForumTopic", query.SearchString), pagination);

            var searchResultsCollection = searchResults.ToList();
            var searchIdentifiers = searchResultsCollection.Select(sr => sr.Id).ToArray();
            var topicsList = topics.Where(t => searchIdentifiers.Contains(t.TopicId)).ToList();

            return topicsList
                .Join(
                    searchResultsCollection,
                    t => t.TopicId,
                    sr => sr.Id,
                    (topic, result) => new
                    {
                        topic,
                        result.Score
                    })
                .OrderByDescending(t => t.Score)
                .Select(t => t.topic);
        }
    }
}