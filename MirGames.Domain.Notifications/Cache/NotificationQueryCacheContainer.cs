// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NotificationQueryCacheContainer.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Notifications.Cache
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Events;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;

    internal sealed class NotificationQueryCacheContainer : QueryCacheContainer<GetNotificationsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationQueryCacheContainer"/> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public NotificationQueryCacheContainer(ICacheManagerFactory cacheManagerFactory) : base(cacheManagerFactory)
        {
        }

        /// <inheritdoc />
        protected override IEnumerable<string> InvalidationEvents
        {
            get
            {
                return new[]
                {
                    "Notifications.NotificationsAdded",
                    "Notifications.NotificationsRemoved"
                };
            }
        }

        /// <inheritdoc />
        protected override string GetCacheDomain(ClaimsPrincipal principal, GetNotificationsQuery query)
        {
            return "Notifications#" + principal.GetUserId();
        }

        /// <inheritdoc />
        protected override string GetCacheKey(
            ClaimsPrincipal principal,
            GetNotificationsQuery query,
            PaginationSettings pagination)
        {
            return string.Format(
                "{0}_{1}_{2}",
                pagination != null ? pagination.GetHashCode() : 0,
                query.NotificationType,
                query.Filter);
        }

        /// <inheritdoc />
        protected override void Invalidate(Event @event)
        {
            int[] users = null;

            var notificationsAddedEvent = @event as NotificationsAddedEvent;
            if (notificationsAddedEvent != null)
            {
                users = notificationsAddedEvent.Notifications.Select(n => n.UserId).ToArray();
            }

            var notificationsRemovedEvent = @event as NotificationsRemovedEvent;
            if (notificationsRemovedEvent != null)
            {
                users = notificationsRemovedEvent.Notifications.Select(n => n.UserId).ToArray();
            }

            if (users != null)
            {
                foreach (var user in users)
                {
                    var cacheManager = this.GetCacheManager("Notifications#" + user);
                    cacheManager.Clear();
                }
            }
        }
    }
}
