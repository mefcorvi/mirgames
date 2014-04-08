namespace MirGames.Domain.Users.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines whether new wall record could be posted.
    /// </summary>
    internal sealed class UserPostNewWallRecordAccessRule : AccessRule<User>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "PostWallRecord"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, User resource)
        {
            return principal.IsInRole("User");
        }
    }
}