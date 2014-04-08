namespace MirGames.Domain.Forum.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class ForumTopicCreateAccessRule : AccessRule<ForumTopic>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Create"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, ForumTopic resource)
        {
            return principal.IsInRole("TopicsAuthor");
        }
    }
}