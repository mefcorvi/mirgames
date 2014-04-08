namespace MirGames.Domain.Wip.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class TopicCreateAccessRule : AccessRule<Project>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Create"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Project resource)
        {
            return principal.IsInRole("User");
        }
    }
}