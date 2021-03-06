// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IAuthorizationManager.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Security
{
    /// <summary>
    /// Claims-based authorization.
    /// </summary>
    public interface IAuthorizationManager
    {
        /// <summary>
        /// Checks the authorization policy.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="action">The action.</param>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity type identifier.</param>
        /// <returns>True when authorized, otherwise false.</returns>
        bool CheckAccess(int userId, string action, string entityType, int? entityId = null);
    }
}