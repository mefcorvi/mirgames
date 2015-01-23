// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EventsHub.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Hubs
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Security;

    /// <summary>
    /// The events hub.
    /// </summary>
    public class EventsHub : Hub
    {
        /// <summary>
        /// The users.
        /// </summary>
        private static readonly ConcurrentDictionary<int, HashSet<string>> Users = new ConcurrentDictionary<int, HashSet<string>>();

        /// <summary>
        /// The connections.
        /// </summary>
        private static readonly ConcurrentDictionary<string, int> Connections = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// The principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> principalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventsHub"/> class.
        /// </summary>
        /// <param name="principalProvider">The principal provider.</param>
        public EventsHub(Func<ClaimsPrincipal> principalProvider)
        {
            Contract.Requires(principalProvider != null);
            this.principalProvider = principalProvider;
        }

        /// <summary>
        /// Gets the user connections.
        /// </summary>
        /// <param name="userId">The user unique identifier.</param>
        /// <returns>The user connections.</returns>
        public static IEnumerable<string> GetUserConnections(int userId)
        {
            HashSet<string> connections;
            return Users.TryGetValue(userId, out connections) ? connections.ToArray() : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Gets the user by connection.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <returns>The user identifier.</returns>
        public static int? GetUserByConnection(string connectionId)
        {
            int userId;
            return Connections.TryGetValue(connectionId, out userId) ? userId : (int?)null;
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <returns>The connections.</returns>
        public static IEnumerable<string> GetConnections()
        {
            return Connections.Keys.ToArray();
        }

        /// <inheritdoc />
        public override Task OnDisconnected(bool stopCalled)
        {
            var principal = this.principalProvider.Invoke();

            if (principal.Identity.IsAuthenticated)
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                string connectionId = this.Context.ConnectionId;
                int value;
                Connections.TryRemove(connectionId, out value);
                
                var connections = Users.GetOrAdd(userId, id => new HashSet<string>());

                lock (connections)
                {
                    connections.Remove(connectionId);
                }
            }

            return base.OnDisconnected(stopCalled);
        }

        /// <inheritdoc />
        public override Task OnReconnected()
        {
            this.HandleUserConnection();
            return base.OnReconnected();
        }

        /// <inheritdoc />
        public override Task OnConnected()
        {
            this.HandleUserConnection();
            return base.OnConnected();
        }

        /// <summary>
        /// Handles the user connection.
        /// </summary>
        private void HandleUserConnection()
        {
            var principal = this.principalProvider.Invoke();

            if (principal.Identity.IsAuthenticated)
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                string connectionId = this.Context.ConnectionId;

                Connections.AddOrUpdate(connectionId, userId, (id, newValue) => userId);
                var connections = Users.GetOrAdd(userId, id => new HashSet<string>());

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }
    }
}