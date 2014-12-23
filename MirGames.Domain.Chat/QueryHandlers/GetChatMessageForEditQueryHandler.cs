// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetChatMessageForEditQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Get Chat Message query.
    /// </summary>
    internal sealed class GetChatMessageForEditQueryHandler : SingleItemQueryHandler<GetChatMessageForEditQuery, ChatMessageForEditViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChatMessageForEditQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetChatMessageForEditQueryHandler(IAuthorizationManager authorizationManager, IReadContextFactory readContextFactory)
        {
            Contract.Requires(authorizationManager != null);
            Contract.Requires(readContextFactory != null);

            this.authorizationManager = authorizationManager;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override ChatMessageForEditViewModel Execute(GetChatMessageForEditQuery query, ClaimsPrincipal principal)
        {
            ChatMessage message;
            using (var readContext = this.readContextFactory.Create())
            {
                message = readContext.Query<ChatMessage>().First(m => m.MessageId == query.MessageId);
            }

            if (message == null)
            {
                throw new ItemNotFoundException("ChatMessage", query.MessageId);
            }

            this.authorizationManager.EnsureAccess(principal, "Edit", "ChatMessage", message.MessageId);

            return new ChatMessageForEditViewModel
                {
                    MessageId = message.MessageId,
                    SourceText = message.Message
                };
        }
    }
}
