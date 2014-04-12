// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserOfflineListener.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Hubs
{
    using System.Linq;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Users.Events;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    using Newtonsoft.Json;

    /// <summary>
    /// Listens the new chat messages.
    /// </summary>
    public class UserOfflineListener : EventListenerBase<UserOfflineEvent>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserOfflineListener" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public UserOfflineListener(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(UserOfflineEvent @event)
        {
            var user = this.queryProcessor.Process(
                new GetUsersQuery
                    {
                        UserIdentifiers = new[] { @event.UserId }
                    }).FirstOrDefault();

            var context = GlobalHost.ConnectionManager.GetHubContext<EventsHub>();
            context.Clients.All.userOffline(JsonConvert.SerializeObject(user));
        }
    }
}