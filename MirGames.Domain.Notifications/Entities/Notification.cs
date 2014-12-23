// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Notification.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Entities
{
    using System;

    using MirGames.Domain.Notifications.ViewModels;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    /// <summary>
    /// The notification entity.
    /// </summary>
    internal sealed class Notification
    {
        /// <summary>
        /// Gets or sets the notification identifier.
        /// </summary>
        [BsonId]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the event type identifier.
        /// </summary>
        public int NotificationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public NotificationData Data { get; set; }
    }
}
