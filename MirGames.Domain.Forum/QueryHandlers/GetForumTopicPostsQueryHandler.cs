namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
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
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the get forum topic query.
    /// </summary>
    public class GetForumTopicPostsQueryHandler : QueryHandler<GetForumTopicPostsQuery, ForumPostsListItemViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetForumTopicPostsQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetForumTopicPostsQueryHandler(IAuthorizationManager authorizationManager, IQueryProcessor queryProcessor)
        {
            Contract.Assert(authorizationManager != null);
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetForumTopicPostsQuery query, ClaimsPrincipal principal)
        {
            return this.GetPostsQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<ForumPostsListItemViewModel> Execute(IReadContext readContext, GetForumTopicPostsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var postsQuery = this.ApplyPagination(GetPostsQuery(readContext, query), pagination).ToList();
            var startIndex = (pagination != null ? pagination.PageNum * pagination.PageSize : 0) + 1;

            var posts = postsQuery.Select(
                (p, idx) => new ForumPostsListItemViewModel
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
                    TopicId = p.TopicId,
                    PostId = p.PostId,
                    UpdatedDate = p.UpdatedDate,
                    IsRead = true,
                    IsFirstPost = p.IsStartPost,
                    Index = idx + startIndex,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", p) && !p.IsStartPost,
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", p)
                })
                .ToList();

            if (principal.Identity.IsAuthenticated)
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                
                var unreadTopic = readContext.Query<ForumTopicUnread>().FirstOrDefault(
                    t => t.TopicId == query.TopicId && t.UserId == userId);

                if (unreadTopic != null)
                {
                    posts.ForEach(p => p.IsRead = p.CreatedDate < unreadTopic.UnreadDate);
                    var firstUnread = posts.FirstOrDefault(p => !p.IsRead);

                    if (firstUnread != null)
                    {
                        firstUnread.FirstUnread = true;
                    }
                }
            }

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                {
                    Authors = posts.Select(t => t.Author)
                });

            return posts;
        }

        /// <summary>
        /// Gets the posts query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The forum posts.</returns>
        private IQueryable<ForumPost> GetPostsQuery(IReadContext readContext, GetForumTopicPostsQuery query)
        {
            var queryable = readContext.Query<ForumPost>()
                .Where(p => p.TopicId == query.TopicId);

            if (!query.LoadStartPost)
            {
                queryable = queryable.Where(p => !p.IsStartPost);
            }

            return queryable.OrderBy(p => p.PostId);
        }
    }
}