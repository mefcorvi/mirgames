namespace MirGames.Domain.Topics.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class TopicEditAccessRule : AccessRule<Topic>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Edit"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Topic resource)
        {
            return principal.IsInRole("TopicsAuthor") && principal.GetUserId() == resource.AuthorId;
        }
    }
}