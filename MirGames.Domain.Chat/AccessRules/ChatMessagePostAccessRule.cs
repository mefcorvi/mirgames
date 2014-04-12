// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatMessagePostAccessRule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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