namespace MirGames.Domain.Forum.AccessRules
{
    using System;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class ForumPostDeleteAccessRule : AccessRule<ForumPost>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Delete"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, ForumPost resource)
        {
            return principal.IsInRole("TopicsAuthor") && principal.GetUserId() == resource.AuthorId && resource.CreatedDate >= DateTime.UtcNow.AddMinutes(-5);
        }
    }
}