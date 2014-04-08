namespace MirGames.Services.Git.AccessRules
{
    using System.Security.Claims;

    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Entities;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class RepositoryCreateAccessRule : AccessRule<Repository>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Create"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Repository resource)
        {
            return principal.IsInRole("User");
        }
    }
}