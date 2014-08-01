// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicReadEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Hubs
{
    using System.Globalization;
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Listens the forum topic read event.
    /// </summary>
    public class ForumTopicReadEventListener : EventListenerBase<ForumTopicReadEvent>
    {
        /// <inheritdoc />
        public override void Process(ForumTopicReadEvent @event)
        {
            /*var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();

            if (@event.UserIdentifiers != null)
            {
                foreach (int userId in @event.UserIdentifiers)
                {
                    context.Clients.User(userId.ToString(CultureInfo.InvariantCulture)).ForumTopicRead();
                }
            }

            if (@event.ExcludedUsers != null)
            {
                var excludedConnections = @event.ExcludedUsers.SelectMany(EventsHub.GetUserConnections);
                context.Clients.AllExcept(excludedConnections.ToArray()).ForumTopicRead();
            }*/
        }
    }
}