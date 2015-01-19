// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetForumTopicsQueryHandler.cs">
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

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Notifications.Queries;
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
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumTopicsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetForumTopicsQueryHandler(IQueryProcessor queryProcessor, ISearchEngine searchEngine, IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(searchEngine != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.searchEngine = searchEngine;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetForumTopicsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var topics = this.GetTopicsQueryable(readContext, query, principal.GetUserId());

                if (!string.IsNullOrWhiteSpace(query.SearchString))
                {
                    var searchResults =
                        this.searchEngine.Search(string.Format("ForumTopic#{0}", query.ForumAlias), query.SearchString).Select(sr => sr.Id).ToArray();
                    return topics.Count(t => searchResults.Contains(t.TopicId));
                }

                return topics.Count();
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<ForumTopicsListItemViewModel> Execute(GetForumTopicsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            ICollection<ForumTopic> topics;

            using (var readContext = this.readContextFactory.Create())
            {
                var set = this.GetTopicsQueryable(readContext, query, principal.GetUserId());
                topics = string.IsNullOrWhiteSpace(query.SearchString)
                             ? this.ApplyPagination(set, pagination).EnsureCollection()
                             : this.GetSearchResult(query, pagination, set).EnsureCollection();
            }

            var forums = this.queryProcessor.Process(new GetForumsQuery()).ToList();

            var viewModels = topics.Select(t => new ForumTopicsListItemViewModel
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
                Visits = t.Visits,
                Title = t.Title,
                TopicId = t.TopicId,
                TagsList = t.TagsList,
                UpdatedDate = t.UpdatedDate,
                IsRead = true,
                ShortDescription = t.ShortDescription,
                Forum = forums.FirstOrDefault(f => f.ForumId == t.ForumId)
            }).ToList();

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = viewModels.Select(t => t.Author).Concat(viewModels.Select(t => t.LastPostAuthor))
                    });

            if (principal.Identity.IsAuthenticated)
            {
                var topicIdentifiers = viewModels.Select(t => t.TopicId).ToArray();

                var newTopicsNotifications =
                    this.queryProcessor.Process(
                        new GetNotificationsQuery { IsRead = false }.WithFilter<NewForumTopicNotification>(n => topicIdentifiers.Contains(n.TopicId)))
                        .Select(n => n.Data)
                        .Cast<NewForumTopicNotification>()
                        .Select(n => n.TopicId)
                        .ToArray();

                var answerNotifications =
                    this.queryProcessor.Process(
                        new GetNotificationsQuery { IsRead = false }.WithFilter<NewForumAnswerNotification>(n => topicIdentifiers.Contains(n.TopicId)));

                var unreadPosts =
                    answerNotifications.GroupBy(n => ((NewForumAnswerNotification)n.Data).TopicId).Select(g => new
                    {
                        TopicId = g.Key,
                        Count = g.Count()
                    }).ToDictionary(g => g.TopicId, g => g.Count);

                foreach (var topic in viewModels)
                {
                    topic.IsRead = !unreadPosts.ContainsKey(topic.TopicId);
                    topic.UnreadPostsCount = unreadPosts.ContainsKey(topic.TopicId)
                                                 ? unreadPosts[topic.TopicId]
                                                 : (int?)null;

                    if (newTopicsNotifications.Contains(topic.TopicId))
                    {
                        topic.IsRead = false;
                        topic.UnreadPostsCount++;
                    }
                }
            }

            return viewModels;
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
        private IQueryable<ForumTopic> GetTopicsQueryable(IReadContext readContext, GetForumTopicsQuery query, int? userId)
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
                var newTopicsNotifications =
                    this.queryProcessor.Process(
                        new GetNotificationsQuery { IsRead = false, Filter = n => n is NewForumTopicNotification || n is NewForumAnswerNotification })
                        .Select(n => n.Data)
                        .ToArray();

                var newTopics = newTopicsNotifications
                    .OfType<NewForumTopicNotification>()
                    .Select(n => n.TopicId);

                var answers =
                    newTopicsNotifications.OfType<NewForumAnswerNotification>()
                                          .Select(n => n.TopicId);

                var topicIdentifiers = newTopics.Union(answers).ToArray();
                topics = topics.Where(t => topicIdentifiers.Contains(t.TopicId));
            }

            if (!string.IsNullOrEmpty(query.ForumAlias))
            {
                var forum = this.queryProcessor.Process(new GetForumsQuery()).FirstOrDefault(f => f.Alias.EqualsIgnoreCase(query.ForumAlias));

                if (forum == null)
                {
                    throw new ItemNotFoundException("Forum", query.ForumAlias);
                }

                topics = topics.Where(t => t.ForumId == forum.ForumId);
            }

            return topics.OrderByDescending(t => t.UpdatedDate);
        }

        /// <summary>
        /// Gets the search result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="topics">The topics.</param>
        /// <returns>The search result.</returns>
        private IEnumerable<ForumTopic> GetSearchResult(GetForumTopicsQuery query, PaginationSettings pagination, IQueryable<ForumTopic> topics)
        {
            var searchResults = this.ApplyPagination(this.searchEngine.Search(string.Format("ForumTopic#{0}", query.ForumAlias), query.SearchString), pagination);

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