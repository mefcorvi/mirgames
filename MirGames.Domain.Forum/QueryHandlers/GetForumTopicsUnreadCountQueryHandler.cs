namespace MirGames.Domain.Forum.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns count of unread forum topics.
    /// </summary>
    internal sealed class GetForumTopicsUnreadCountQueryHandler : SingleItemQueryHandler<GetForumTopicsUnreadCountQuery, int>
    {
        /// <inheritdoc />
        public override int Execute(IReadContext readContext, GetForumTopicsUnreadCountQuery query, ClaimsPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return 0;
            }

            int userId = principal.GetUserId().GetValueOrDefault();

            var topicReadItems = readContext.Query<ForumTopicRead>().Where(t => t.UserId == userId);

            int totalCount = readContext.Query<ForumTopic>().Count();
            int readCount = readContext.Query<ForumTopic>().SelectMany(
                topic => topicReadItems,
                (topic, topicRead) => new
                    {
                        topic,
                        topicRead
                    }).Where(
                        t => t.topicRead.UserId == userId && t.topic.TopicId >= t.topicRead.StartTopicId
                             && t.topic.TopicId <= t.topicRead.EndTopicId).Select(
                                 t => t.topic).Count();

            return totalCount - readCount;
        }
    }
}
