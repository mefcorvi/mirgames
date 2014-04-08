namespace MirGames.Domain.Topics.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class TopicCommentAccessRule : AccessRule<Topic>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Comment"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Topic resource)
        {
            return principal.IsInRole("User");
        }
    }
}
