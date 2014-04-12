// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ClaimsPrincipalExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Security
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Infrastructure;

    /// <summary>
    /// Extensions for IIdentity.
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// The user id claim type.
        /// </summary>
        public const string UserIdClaimType = "http://mirgames.ru/claims/userId";

        /// <summary>
        /// The session id claim type.
        /// </summary>
        public const string SessionIdClaimType = "http://mirgames.ru/claims/sessionId";

        /// <summary>
        /// The IP address claim type.
        /// </summary>
        public const string UserHostAddressClaimType = "http://mirgames.ru/claims/userHostAddress";

        /// <summary>
        /// The time-zone claim type.
        /// </summary>
        public const string TimezoneClaimType = "http://mirgames.ru/claims/timezone";

        /// <summary>
        /// The action claim type.
        /// </summary>
        private const string MirGamesActionClaimType = "http://mirgames.ru/claims/action";

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The user ID.</returns>
        public static int? GetUserId(this ClaimsPrincipal principal)
        {
            Contract.Requires(principal != null);

            var userIdClaim = principal.FindFirst(UserIdClaimType);

            if (userIdClaim != null && !string.IsNullOrEmpty(userIdClaim.Value))
            {
                return int.Parse(userIdClaim.Value);
            }

            return null;
        }

        /// <summary>
        /// Gets the user login.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The user login.</returns>
        public static string GetUserLogin(this ClaimsPrincipal principal)
        {
            Contract.Requires(principal != null);

            var nameClaim = principal.FindFirst(ClaimTypes.Name);

            if (nameClaim != null && !string.IsNullOrEmpty(nameClaim.Value))
            {
                return nameClaim.Value;
            }

            return null;
        }

        /// <summary>
        /// Gets the session unique identifier.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The session identifier.</returns>
        public static string GetSessionId(this ClaimsPrincipal principal)
        {
            Contract.Requires(principal != null);

            var sessionIdClaim = principal.FindFirst(SessionIdClaimType);
            return sessionIdClaim != null ? sessionIdClaim.Value : null;
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The user ID.</returns>
        public static string GetHostAddress(this ClaimsPrincipal principal)
        {
            Contract.Requires(principal != null);

            var userIdClaim = principal.FindFirst(UserHostAddressClaimType);
            return userIdClaim != null ? userIdClaim.Value : null;
        }

        /// <summary>
        /// Gets the user ID.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>The user ID.</returns>
        public static TimeZoneInfo GetTimeZone(this ClaimsPrincipal principal)
        {
            Contract.Requires(principal != null);

            var timezoneClaim = principal.FindFirst(TimezoneClaimType);
            return timezoneClaim != null ? TimeZoneInfo.FindSystemTimeZoneById(timezoneClaim.Value) : null;
        }

        /// <summary>
        /// Determines whether this instance the specified principal can do the specified action.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="action">The action.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity unique identifier.</param>
        /// <returns>
        /// True whether this instance the specified principal can do the specified action.
        /// </returns>
        public static bool Can(this ClaimsPrincipal principal, string action, string entityType, int? entityId = null)
        {
            Contract.Requires(principal != null);
            Contract.Requires(!string.IsNullOrEmpty(action));
            Contract.Requires(!string.IsNullOrEmpty(entityType));

            string claimType = string.Format("{0}/{1}/{2}/{3}", MirGamesActionClaimType, action, entityType, entityId);
            var actionClaim = principal.FindFirst(claimType);

            return actionClaim != null && actionClaim.Value.EqualsIgnoreCase("allowed");
        }

        /// <summary>
        /// Creates the action claim.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity unique identifier.</param>
        /// <returns>The action claim.</returns>
        public static Claim CreateActionClaim(string action, string entityType, int? entityId = null)
        {
            Contract.Requires(!string.IsNullOrEmpty(action));
            Contract.Requires(!string.IsNullOrEmpty(entityType));

            string claimType = string.Format("{0}/{1}/{2}/{3}", MirGamesActionClaimType, action, entityType, entityId);

            return new Claim(claimType, "allowed");
        }
    }
}