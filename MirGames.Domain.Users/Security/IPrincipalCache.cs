// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IPrincipalCache.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Security
{
    using System;
    using System.Security.Claims;

    /// <summary>
    /// Stores the principal in cache.
    /// </summary>
    public interface IPrincipalCache
    {
        /// <summary>
        /// Gets or adds the principal.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="principalFactory">The principal factory.</param>
        /// <returns>The principal.</returns>
        ClaimsPrincipal GetOrAdd(string sessionId, Func<ClaimsPrincipal> principalFactory);

        /// <summary>
        /// Removes the specified session unique identifier.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        void Remove(string sessionId);
    }
}
