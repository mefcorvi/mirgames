// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="notificationTypeResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Notifications.Entities;
    using MirGames.Infrastructure;

    /// <summary>
    /// Resolves the event type.
    /// </summary>
    internal sealed class NotificationTypeResolver : INotificationTypeResolver
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The notifications cache.
        /// </summary>
        private readonly Lazy<IDictionary<string, int>> notificationsCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationTypeResolver"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public NotificationTypeResolver(IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            this.readContextFactory = readContextFactory;

            this.notificationsCache = new Lazy<IDictionary<string, int>>(this.InitializeNotificationsCache);
        }

        /// <summary>
        /// Gets the notifications cache.
        /// </summary>
        private IDictionary<string, int> NotificationsCache
        {
            get { return this.notificationsCache.Value; }
        }

        /// <inheritdoc />
        public int GetIdentifier(string eventType)
        {
            if (!this.NotificationsCache.ContainsKey(eventType))
            {
                throw new ItemNotFoundException("NotificationEventType", eventType);
            }

            return this.NotificationsCache[eventType];
        }

        /// <inheritdoc />
        public string GetNotificationType(int eventTypeId)
        {
            var pair = this.NotificationsCache.FirstOrDefault(t => t.Value == eventTypeId);

            if (pair.Key == null)
            {
                throw new ItemNotFoundException("NotificationEventType", eventTypeId);
            }

            return pair.Key;
        }

        /// <summary>
        /// Initializes the well-known notifications cache.
        /// </summary>
        /// <returns>The array of notifications.</returns>
        private Dictionary<string, int> InitializeNotificationsCache()
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var notificationTypes = readContext.Query<NotificationEventType>().ToList();
                return notificationTypes.ToDictionary(t => t.EventType, t => t.EventTypeId);
            }
        }
    }
}