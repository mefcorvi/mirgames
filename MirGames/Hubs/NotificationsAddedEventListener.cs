// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NotificationsAddedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Hubs
{
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Notifications.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the new notifications.
    /// </summary>
    public sealed class NotificationsAddedEventListener : EventListenerBase<NotificationsAddedEvent>
    {
        /// <inheritdoc />
        public override void Process(NotificationsAddedEvent @event)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();

            foreach (var notification in @event.Notifications)
            {
                var connections = EventsHub.GetUserConnections(notification.Data.UserId).ToArray();

                if (connections.Any())
                {
                    context.Clients.Clients(connections).NewNotification(notification);
                }
            }
        }
    }
}