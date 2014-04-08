namespace MirGames.Domain.Chat.AccessRules
{
    using System;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to chat message.
    /// </summary>
    internal sealed class ChatMessageDeleteAccessRule : AccessRule<ChatMessage>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Delete"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, ChatMessage resource)
        {
            return principal.IsInRole("ChatMember") && resource.AuthorId == principal.GetUserId() && resource.CreatedDate >= DateTime.UtcNow.AddMinutes(-5);
        }
    }
}