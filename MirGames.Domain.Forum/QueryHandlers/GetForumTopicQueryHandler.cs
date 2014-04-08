namespace MirGames.Domain.Forum.QueryHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
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
    public class GetForumTopicQueryHandler : SingleItemQueryHandler<GetForumTopicQuery, ForumTopicViewModel>
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
        /// Initializes a new instance of the <see cref="GetForumTopicQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public GetForumTopicQueryHandler(IAuthorizationManager authorizationManager, Lazy<IQueryProcessor> queryProcessor)
        {
            Contract.Assert(authorizationManager != null);
            this.authorizationManager = authorizationManager;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override ForumTopicViewModel Execute(IReadContext readContext, GetForumTopicQuery query, ClaimsPrincipal principal)
        {
            var topic = GetTopic(readContext, query);

            if (topic == null)
            {
                return null;
            }

            var topicViewModel = new ForumTopicViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = topic.AuthorId,
                            Login = topic.AuthorLogin
                        },
                    AuthorIp = topic.AuthorIp,
                    CreatedDate = topic.CreatedDate,
                    Title = topic.Title,
                    TopicId = topic.TopicId,
                    TagsList = topic.TagsList,
                    UpdatedDate = topic.UpdatedDate
                };

            var post = readContext.Query<ForumPost>().First(p => p.TopicId == topic.TopicId && p.IsStartPost);

            topicViewModel.StartPost = new ForumPostsListItemViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = post.AuthorId,
                            Login = post.AuthorLogin
                        },
                    AuthorIP = post.AuthorIP,
                    CreatedDate = post.CreatedDate,
                    IsHidden = post.IsHidden,
                    Text = post.Text,
                    TopicId = post.TopicId,
                    PostId = post.PostId,
                    UpdatedDate = post.UpdatedDate,
                    IsRead = true,
                    IsFirstPost = post.IsStartPost,
                    Index = 1,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", post) && !post.IsStartPost,
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", post)
                };

            topicViewModel.CanBeAnswered = this.authorizationManager.CheckAccess(principal, "Reply", topic);
            topicViewModel.CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", topic);
            topicViewModel.CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", topic);

            if (!principal.Identity.IsAuthenticated)
            {
                topicViewModel.IsRead = true;
            }
            else
            {
                int userId = principal.GetUserId().GetValueOrDefault();

                topicViewModel.IsRead =
                    readContext.Query<ForumTopicRead>().Any(
                        r =>
                        r.UserId == userId && topic.TopicId >= r.StartTopicId
                        && topic.TopicId <= r.EndTopicId);
            }

            this.queryProcessor.Value.Process(
                new ResolveAuthorsQuery
                {
                    Authors = new[] { topicViewModel.Author, topicViewModel.StartPost.Author }
                });

            return topicViewModel;
        }

        /// <summary>
        /// Gets the topic.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The topic.</returns>
        private ForumTopic GetTopic(IReadContext readContext, GetForumTopicQuery query)
        {
            return readContext.Query<ForumTopic>().FirstOrDefault(t => t.TopicId == query.TopicId);
        }
    }
}