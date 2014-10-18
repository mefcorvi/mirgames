// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ActionTypesResolverExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Services
{
    using MirGames.Domain.Exceptions;

    internal static class ActionTypesResolverExtensions
    {
        /// <summary>
        /// Gets the action identifier.
        /// </summary>
        /// <param name="actionTypesResolver">The action types resolver.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="entityTypeId">The entity type identifier.</param>
        /// <returns>The action identifier.</returns>
        public static int GetActionId(this IActionTypesResolver actionTypesResolver, string actionName, int entityTypeId)
        {
            var actionType = actionTypesResolver.FindActionId(actionName, entityTypeId);

            if (actionType == null)
            {
                throw new ItemNotFoundException("Action", string.Format("{0}#{1}", entityTypeId, actionName));
            }

            return actionType.Value;
        }
    }
}