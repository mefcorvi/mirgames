// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetTopicsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.Services;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the GetTopicsQuery.
    /// </summary>
    internal sealed class GetTopicsQueryHandler : QueryHandler<GetTopicsQuery, TopicsListItem>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The blog resolver.
        /// </summary>
        private readonly IBlogResolver blogResolver;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicsQueryHandler" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="blogResolver">The blog resolver.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetTopicsQueryHandler(
            ISearchEngine searchEngine,
            IAuthorizationManager authorizationManager,
            IQueryProcessor queryProcessor,
            IBlogResolver blogResolver,
            IReadContextFactory readContextFactory)
        {
            Contract.Requires(searchEngine != null);
            Contract.Requires(authorizationManager != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(blogResolver != null);
            Contract.Requires(readContextFactory != null);

            this.searchEngine = searchEngine;
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
            this.blogResolver = blogResolver;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override IEnumerable<TopicsListItem> Execute(GetTopicsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            ICollection<TopicsListItem> topics;

            using (var readContext = this.readContextFactory.Create())
            {
                var topicsQuery = this.GetTopicsQueryable(readContext, query);

                topics = string.IsNullOrWhiteSpace(query.SearchString)
                             ? this.ApplyPagination(topicsQuery, pagination).EnsureCollection()
                             : this.GetSearchResult(query, pagination, topicsQuery).EnsureCollection();
            }

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = topics.Select(item => item.Author)
                    });

            var topicIdentifiers = topics.Select(t => t.TopicId).ToArray();

            var newTopics =
                this.queryProcessor.Process(
                    new GetNotificationsQuery { IsRead = false }.WithFilter<NewBlogTopicNotification>(
                        n => topicIdentifiers.Contains(n.TopicId)))
                    .Select(t => (NewBlogTopicNotification)t.Data)
                    .Select(t => t.TopicId)
                    .ToArray();

            var newComments =
                this.queryProcessor.Process(
                    new GetNotificationsQuery { IsRead = false }.WithFilter<NewTopicCommentNotification>(
                        n => topicIdentifiers.Contains(n.TopicId)))
                        .Select(t => (NewTopicCommentNotification)t.Data)
                        .GroupBy(t => t.TopicId)
                        .ToDictionary(t => t.Key, t => t.Count());

            foreach (var topicsListItem in topics)
            {
                topicsListItem.CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", "Topic", topicsListItem.TopicId);
                topicsListItem.CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", "Topic", topicsListItem.TopicId);
                topicsListItem.CanBeCommented = this.authorizationManager.CheckAccess(principal, "Comment", "Topic", topicsListItem.TopicId);

                topicsListItem.UnreadCommentsCount = newComments.ContainsKey(topicsListItem.TopicId)
                                                         ? newComments[topicsListItem.TopicId]
                                                         : 0;
                topicsListItem.IsRead = !newTopics.Contains(topicsListItem.TopicId);
                topicsListItem.Blog = this.blogResolver.Resolve(topicsListItem.Blog);
            }

            return topics;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetTopicsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var topics = this.GetTopicsQueryable(readContext, query);

                if (!string.IsNullOrWhiteSpace(query.SearchString))
                {
                    var searchResults = this.GetSearchResults(query).Select(sr => sr.Id).ToArray();
                    return topics.Count(t => searchResults.Contains(t.TopicId));
                }

                return topics.Count();
            }
        }

        /// <summary>
        /// Gets the search results.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The search results.</returns>
        private IEnumerable<SearchResult> GetSearchResults(GetTopicsQuery query)
        {
            var terms = new List<SearchIndexTerm>();

            if (!query.Tag.IsNullOrEmpty())
            {
                terms.Add(new SearchIndexTerm("tags", query.Tag));
            }

            return this.searchEngine.Search("Topic", query.SearchString, terms.ToArray());
        }

        /// <summary>
        /// Gets the topics query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The prepared query.</returns>
        private IQueryable<TopicsListItem> GetTopicsQueryable(IReadContext readContext, GetTopicsQuery query)
        {
            IQueryable<Topic> topics = readContext.Query<Topic>();

            if (query.IsPublished)
            {
                topics = topics.Where(t => t.IsPublished);
            }

            if (query.BlogId.HasValue)
            {
                topics = topics.Where(t => t.BlogId == query.BlogId);
            }

            if (query.ShowOnMain)
            {
                topics = topics.Where(t => t.ShowOnMain);
            }

            if (query.IsMicroTopic.HasValue)
            {
                bool isMicroTopic = query.IsMicroTopic.Value;
                topics = topics.Where(t => t.IsMicroTopic == isMicroTopic);
            }

            var identifiers = (query.Identifiers ?? new int[0]).EnsureCollection();
            if (identifiers.Count > 0)
            {
                topics = topics.Where(t => identifiers.Contains(t.Id));
            }

            if (query.IsTutorial.HasValue)
            {
                topics = topics.Where(t => t.IsTutorial == query.IsTutorial);
            }

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                var tags = readContext.Query<TopicTag>().Where(t => t.TagText == query.Tag);
                topics = topics.Join(
                    tags,
                    topic => topic.Id,
                    tag => tag.TopicId,
                    (topic, tag) => topic);
            }

            if (query.OnlyUnread)
            {
                var notifications =
                    this.queryProcessor.Process(
                        new GetNotificationsQuery
                        {
                            IsRead = false,
                            Filter = n => n is NewBlogTopicNotification || n is NewTopicCommentNotification
                        })
                        .Select(t => t.Data)
                        .ToArray();

                var newTopics = notifications.OfType<NewBlogTopicNotification>().Select(t => t.TopicId);
                var newComments = notifications.OfType<NewTopicCommentNotification>().Select(t => t.TopicId);

                var topicIdentifiers = newTopics.Union(newComments).ToArray();
                topics = topics.Where(t => topicIdentifiers.Contains(t.Id));
            }

            return topics.Include(t => t.Content).OrderByDescending(t => t.CreationDate).Select(x => new TopicsListItem
            {
                Author = new AuthorViewModel
                {
                    Id = x.AuthorId,
                },
                Blog = new BlogViewModel
                {
                    BlogId = x.BlogId
                },
                CreationDate = x.CreationDate,
                ShortText = x.Content.TopicTextShort,
                Tags = x.TagsList,
                Title = x.TopicTitle,
                TopicId = x.Id,
                SourceAuthor = x.SourceAuthor,
                SourceLink = x.SourceLink,
                IsTutorial = x.IsTutorial,
                IsRepost = x.IsRepost,
                CommentsCount = x.CountComment,
                ReadMoreText = x.CutText,
                IsMicroTopic = x.IsMicroTopic
            });
        }

        /// <summary>
        /// Gets the search result.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <param name="topics">The topics.</param>
        /// <returns>The search result.</returns>
        private IEnumerable<TopicsListItem> GetSearchResult(GetTopicsQuery query, PaginationSettings pagination, IQueryable<TopicsListItem> topics)
        {
            var searchResults = this.ApplyPagination(this.GetSearchResults(query), pagination);

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