namespace MirGames.Domain.Chat.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to topic view model.
    /// </summary>
    internal sealed class ChatMessagePostAccessRule : AccessRule<ChatMessage>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "Post"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, ChatMessage resource)
        {
            return principal.IsInRole("ChatMember");
        }
    }
}