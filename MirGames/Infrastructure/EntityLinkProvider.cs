// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EntityLinkProvider.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    using System.Collections.Generic;
    using System.Linq;

    using MirGames.Infrastructure;

    internal sealed class EntityLinkProvider : IEntityLinkProvider
    {
        /// <summary>
        /// The entity link registrar.
        /// </summary>
        private readonly IEnumerable<IEntityLinkRegistrar> entityLinkRegistrars;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityLinkProvider" /> class.
        /// </summary>
        /// <param name="entityLinkRegistrars">The entity link registrars.</param>
        public EntityLinkProvider(IEnumerable<IEntityLinkRegistrar> entityLinkRegistrars)
        {
            this.entityLinkRegistrars = entityLinkRegistrars.EnsureCollection();
        }

        /// <inheritdoc />
        public string GetLink(int? entityId, string entityType)
        {
            return this.entityLinkRegistrars
                       .Where(registrar => registrar.CanProcess(entityType))
                       .Select(registrar => registrar.GetLink(entityId, entityType))
                       .FirstOrDefault();
        }
    }
}