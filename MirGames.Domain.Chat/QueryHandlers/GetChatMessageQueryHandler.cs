// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetChatMessageQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Chat.Entities;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Domain.Chat.ViewModels;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the Get Chat Message query.
    /// </summary>
    internal sealed class GetChatMessageQueryHandler : SingleItemQueryHandler<GetChatMessageQuery, ChatMessageViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetChatMessageQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="textTransform">The text transform.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetChatMessageQueryHandler(IQueryProcessor queryProcessor, ITextTransform textTransform, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.textTransform = textTransform;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override ChatMessageViewModel Execute(IReadContext readContext, GetChatMessageQuery query, ClaimsPrincipal principal)
        {
            var messageEntity = readContext.Query<ChatMessage>().FirstOrDefault(m => m.MessageId == query.MessageId);

            if (messageEntity == null)
            {
                throw new ItemNotFoundException("ChatMessage", query.MessageId);
            }

            var message = new ChatMessageViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = messageEntity.AuthorId,
                            Login = messageEntity.AuthorLogin
                        },
                    CreatedDate = messageEntity.CreatedDate,
                    UpdatedDate = messageEntity.UpdatedDate,
                    MessageId = messageEntity.MessageId,
                    Text = messageEntity.Message,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", messageEntity),
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", messageEntity)
                };

            this.queryProcessor.Process(new ResolveAuthorsQuery { Authors = new[] { message.Author } });
            message.Text = this.textTransform.Transform(message.Text);

            return message;
        }
    }
}
