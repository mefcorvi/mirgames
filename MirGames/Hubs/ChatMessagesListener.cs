// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatMessagesListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Hubs
{
    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Chat.Events;
    using MirGames.Domain.Chat.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    using Newtonsoft.Json;

    /// <summary>
    /// Listens the new chat messages.
    /// </summary>
    public class ChatMessagesListener : EventListenerBase<NewChatMessageEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessagesListener" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public ChatMessagesListener(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager)
        {
            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override void Process(NewChatMessageEvent @event)
        {
            var message = this.queryProcessor.Process(
                new GetChatMessageQuery
                {
                    MessageId = @event.MessageId
                });

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

            foreach (string connectionId in ChatHub.GetConnections())
            {
                int? userId = ChatHub.GetUserByConnection(connectionId);

                message.CanBeDeleted = this.authorizationManager.CheckAccess(
                    userId.GetValueOrDefault(),
                    "Delete",
                    "ChatMessage",
                    message.MessageId);

                message.CanBeEdited = this.authorizationManager.CheckAccess(
                    userId.GetValueOrDefault(),
                    "Edit",
                    "ChatMessage",
                    message.MessageId);

                context.Clients.Client(connectionId).addNewMessageToPage(JsonConvert.SerializeObject(message));
            }
        }
    }
}