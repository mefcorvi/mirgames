namespace MirGames.Domain.Forum.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to post replying.
    /// </summary>
    internal sealed class ForumTopicReplyAccessRule : AccessRule<ForumTopic>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Reply"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, ForumTopic resource)
        {
            return principal.IsInRole("TopicsAuthor");
        }
    }
}