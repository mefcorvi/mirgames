// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ActionTypesResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Services
{
    using System.Linq;

    using MirGames.Domain.Acl.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Repositories;

    /// <summary>
    /// Resolves action by the action name.
    /// </summary>
    internal sealed class ActionTypesResolver : DictionaryEntityResolver<string, ActionType>, IActionTypesResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTypesResolver"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public ActionTypesResolver(IReadContextFactory readContextFactory) : base(readContextFactory)
        {
        }

        /// <inheritdoc />
        public int? FindActionId(string actionName, int entityTypeId)
        {
            var action = this.Resolve(actionName).FirstOrDefault(a => a.EntityTypeId == entityTypeId);

            if (action == null)
            {
                return null;
            }

            return action.ActionId;
        }

        /// <inheritdoc />
        protected override bool IsSatisfied(ActionType entity, string key)
        {
            return entity.ActionName.EqualsIgnoreCase(key);
        }
    }
}