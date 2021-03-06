﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewForumTopicNotification.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Notifications
{
    public sealed class NewForumTopicNotification : ForumTopicNotificationData
    {
        /// <inheritdoc />
        public override string NotificationType
        {
            get { return "Forum.NewTopic"; }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new NewForumTopicNotification
            {
                ForumId = this.ForumId,
                IsRead = this.IsRead,
                NotificationDate = this.NotificationDate,
                NotificationTypeId = this.NotificationTypeId,
                TopicId = this.TopicId,
                UserId = this.UserId
            };
        }
    }
}
