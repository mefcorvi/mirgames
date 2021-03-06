﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewBlogTopicNotification.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.Notifications
{
    using MirGames.Domain.Notifications.ViewModels;

    public sealed class NewBlogTopicNotification : NotificationData
    {
        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public int? BlogId { get; set; }

        /// <summary>
        /// Gets or sets the topic identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <inheritdoc />
        public override string NotificationType
        {
            get { return "Topics.NewBlogTopic"; }
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new NewBlogTopicNotification
            {
                BlogId = this.BlogId,
                IsRead = this.IsRead,
                NotificationDate = this.NotificationDate,
                NotificationTypeId = this.NotificationTypeId,
                TopicId = this.TopicId,
                UserId = this.UserId
            };
        }
    }
}
