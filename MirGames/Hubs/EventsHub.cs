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

        /// <inheritdoc />
        public override Task OnConnected()
        {
            var principal = this.principalProvider.Invoke();

            if (!principal.Identity.IsAuthenticated)
            {
                return base.OnConnected();
            }

            int userId = principal.GetUserId().GetValueOrDefault();
            string connectionId = Context.ConnectionId;

            var connections = Users.GetOrAdd(userId, id => new HashSet<string>());

            lock (connections)
            {
                connections.Add(connectionId);
            }

            return base.OnConnected();
        }
    }
}