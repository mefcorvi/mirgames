// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewChatMessageEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Chat.EventListeners
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Chat.Events;
    using MirGames.Domain.Chat.Notifications;
    using MirGames.Domain.Notifications.Commands;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about new chat message created.
    /// </summary>
    internal sealed class NewChatMessageEventListener : EventListenerBase<NewChatMessageEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewChatMessageEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public NewChatMessageEventListener(ICommandProcessor commandProcessor)
        {
            Contract.Requires(commandProcessor != null);

            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(NewChatMessageEvent @event)
        {
            Contract.Requires(@event != null);

            if (@event.Mentions.Any())
            {
                this.commandProcessor.Execute(new NotifyUsersCommand
                {
                    NotificationTemplate = new ChatMentionNotification { MessageId = @event.MessageId },
                    UserIdentifiers = @event.Mentions.Where(m => m.Id.HasValue).Select(m => m.Id.Value).ToArray()
                });
            }
        }
    }
}