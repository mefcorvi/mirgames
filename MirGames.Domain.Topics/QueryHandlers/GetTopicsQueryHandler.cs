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
        private readonly Lazy<IQueryProcessor> queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTopicsQueryHandler" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetTopicsQueryHandler(ISearchEngine searchEngine, IAuthorizationManager authorizationManager, Lazy<IQueryProcessor> queryProcessor)
        {
            this.searchEngine = searchEngine;
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override IEnumerable<TopicsListItem> Execute(IReadContext readContext, GetTopicsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var topicsQuery = this.GetTopicsQueryable(readContext, query);
            ICollection<TopicsListItem> topics;

            if (string.IsNullOrWhiteSpace(query.SearchString))
            {
                topics = this.ApplyPagination(topicsQuery, pagination).EnsureCollection();
            }
            else
            {
                topics = this.GetSearchResult(query, pagination, topicsQuery).EnsureCollection();
            }

            this.queryProcessor.Value.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = topics.Select(item => item.Author)
                    });

            foreach (var topicsListItem in topics)
            {
                var accessResource = new Topic
                    {
                        Id = topicsListItem.TopicId,
                        AuthorId = topicsListItem.Author.Id.GetValueOrDefault()
                    };

                topicsListItem.CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", accessResource);
                topicsListItem.CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", accessResource);
                topicsListItem.CanBeCommented = this.authorizationManager.CheckAccess(principal, "Comment", accessResource);
            }

            return topics;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetTopicsQuery query, ClaimsPrincipal principal)
        {
            var topics = this.GetTopicsQueryable(readContext, query);

            if (!string.IsNullOrWhiteSpace(query.SearchString))
            {
                var searchResults = this.searchEngine.Search("Topic", query.SearchString).Select(sr => sr.Id).ToArray();
                return topics.Count(t => searchResults.Contains(t.TopicId));
            }

            return topics.Count();
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

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                var tags = readContext.Query<TopicTag>().Where(t => t.TagText == query.Tag);
                topics = topics.Join(
                    tags,
                    topic => topic.Id,
                    tag => tag.TopicId,
                    (topic, tag) => topic);
            }

            return topics.Include(t => t.Content).OrderByDescending(t => t.CreationDate).Select(x => new TopicsListItem
            {
                Author = new AuthorViewModel
                {
                    Id = x.AuthorId,
                },
                CreationDate = x.CreationDate,
                ShortText = x.Content.TopicTextShort,
                Tags = x.TagsList,
                Title = x.TopicTitle,
                TopicId = x.Id,
                CommentsCount = x.CountComment
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
            var searchResults = this.ApplyPagination(this.searchEngine.Search("Topic", query.SearchString), pagination);

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