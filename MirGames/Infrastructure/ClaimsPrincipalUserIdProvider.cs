// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ClaimsPrincipalUserIdProvider.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames
{
    using System.Security.Claims;

    using Microsoft.AspNet.SignalR;

    using MirGames.Domain.Security;

    /// <summary>
    /// Provides an user ID for the SignalR request.
    /// </summary>
    internal sealed class ClaimsPrincipalUserIdProvider : IUserIdProvider
    {
        /// <inheritdoc />
        public string GetUserId(IRequest request)
        {
            var claimsPrincipal = request.User as ClaimsPrincipal;

            if (claimsPrincipal != null)
            {
                return claimsPrincipal.GetUserId().ToString();
            }

            return null;
        }
    }
}