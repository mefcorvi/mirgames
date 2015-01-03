// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatMentionNotification.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Chat.Notifications
{
    using MirGames.Domain.Notifications.ViewModels;

    public sealed class ChatMentionNotification : NotificationData
    {
        /// <summary>
        /// Gets or sets the message identifier.
        /// </summary>
        public int MessageId { get; set; }

        /// <inheritdoc />
        public override string NotificationType
        {
            get { return "Chat.UserMentioned"; }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new ChatMentionNotification
            {
                IsRead = this.IsRead,
                NotificationDate = this.NotificationDate,
                NotificationTypeId = this.NotificationTypeId,
                UserId = this.UserId,
                MessageId = this.MessageId
            };
        }
    }
}
