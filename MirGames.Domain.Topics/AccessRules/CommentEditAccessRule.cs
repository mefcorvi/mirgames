namespace MirGames.Domain.Topics.AccessRules
{
    using System;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to comment view model.
    /// </summary>
    internal sealed class CommentEditAccessRule : AccessRule<Comment>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Edit"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, Comment resource)
        {
            return principal.IsInRole("TopicsAuthor") && principal.GetUserId() == resource.UserId && resource.Date >= DateTime.UtcNow.AddMinutes(-30);
        }
    }
}