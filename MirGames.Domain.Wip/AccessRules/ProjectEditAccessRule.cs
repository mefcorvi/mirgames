namespace MirGames.Domain.Wip.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to project view model.
    /// </summary>
    internal sealed class ProjectEditAccessRule : AccessRule<Project>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Edit"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Project resource)
        {
            return principal.Can("Edit", "WipProject", resource.ProjectId);
        }
    }
}