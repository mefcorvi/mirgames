// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AuthenticationProvider.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Security
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Provides an user by session Id.
    /// </summary>
    internal sealed class AuthenticationProvider : IAuthenticationProvider
    {
        /// <summary>
        /// The user id claim type.
        /// </summary>
        public const string UserIdClaimType = "http://mirgames.ru/claims/userId";

        /// <summary>
        /// The user id claim type.
        /// </summary>
        public const string UserHostAddressClaimType = "http://mirgames.ru/claims/userHostAddress";

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The client host address provider.
        /// </summary>
        private readonly IClientHostAddressProvider clientHostAddressProvider;

        /// <summary>
        /// The principal cache.
        /// </summary>
        private readonly IPrincipalCache principalCache;

        /// <summary>
        /// The claims providers.
        /// </summary>
        private readonly IEnumerable<IAdditionalClaimsProvider> claimsProviders;

        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationProvider" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="clientHostAddressProvider">The client host address provider.</param>
        /// <param name="principalCache">The principal cache.</param>
        /// <param name="claimsProviders">The claims providers.</param>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public AuthenticationProvider(
            IWriteContextFactory writeContextFactory,
            IClientHostAddressProvider clientHostAddressProvider,
            IPrincipalCache principalCache,
            IEnumerable<IAdditionalClaimsProvider> claimsProviders,
            IOnlineUsersManager onlineUsersManager)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(clientHostAddressProvider != null);
            Contract.Requires(principalCache != null);

            this.writeContextFactory = writeContextFactory;
            this.clientHostAddressProvider = clientHostAddressProvider;
            this.principalCache = principalCache;
            this.claimsProviders = claimsProviders;
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        public ClaimsPrincipal GetPrincipal(string sessionId)
        {
            var principal = this.principalCache.GetOrAdd(sessionId, () => this.CreatePrincipal(sessionId));
            this.onlineUsersManager.Ping(principal);

            return principal;
        }

        /// <summary>
        /// Creates the principal.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <returns>The principal.</returns>
        private ClaimsPrincipal CreatePrincipal(string sessionId)
        {
            var principal = new ClaimsPrincipal();
            User currentUser = null;

            if (!string.IsNullOrEmpty(sessionId))
            {
                using (var writeContext = this.writeContextFactory.Create())
                {
                    var session = writeContext
                        .Set<UserSession>()
                        .SingleOrDefault(s => s.Id == sessionId);

                    if (session != null)
                    {
                        currentUser =
                            writeContext.Set<User>().Include(u => u.UserAdministrator).Single(u => u.Id == session.UserId);
                        currentUser.LastVisitDate = DateTime.UtcNow;

                        session.LastVisitIP = this.clientHostAddressProvider.GetHostAddress();
                        session.LastDate = DateTime.UtcNow;

                        writeContext.SaveChanges();
                    }
                }
            }

            if (currentUser != null)
            {
                var identity = new ClaimsIdentity("MirGamesAuthentication");
                principal.AddIdentity(identity);
                identity.AddClaim(new Claim(ClaimTypes.Name, currentUser.Login));
                identity.AddClaim(new Claim(ClaimsPrincipalExtensions.SessionIdClaimType, sessionId));

                if (currentUser.IsActivated)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                    identity.AddClaim(new Claim(ClaimTypes.Role, "TopicsAuthor"));
                }
                else
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "ReadOnlyUser"));
                }

                if (currentUser.UserAdministrator != null)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
                }

                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, currentUser.Login));
                identity.AddClaim(
                    new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "MirGames"));

                identity.AddClaim(new Claim(UserIdClaimType, currentUser.Id.ToString(CultureInfo.InvariantCulture)));
                identity.AddClaim(new Claim(UserHostAddressClaimType, this.clientHostAddressProvider.GetHostAddress()));

                if (currentUser.Timezone != null)
                {
                    identity.AddClaim(new Claim(ClaimsPrincipalExtensions.TimezoneClaimType, currentUser.Timezone));
                }

                var additionalClaims = this.claimsProviders.SelectMany(p => p.GetAdditionalClaims(principal));
                identity.AddClaims(additionalClaims);
            }

            // if user is guest
            if (!principal.Identities.Any())
            {
                var identity = new ClaimsIdentity();
                identity.AddClaim(new Claim(ClaimTypes.Role, "Guest"));
                identity.AddClaim(new Claim(UserHostAddressClaimType, this.clientHostAddressProvider.GetHostAddress()));
                principal.AddIdentity(identity);
            }

            return principal;
        }
    }
}