// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryContextFactory.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The read context factory.
    /// </summary>
    internal sealed class QueryContextFactory : IReadContextFactory
    {
        /// <summary>
        /// The mappers.
        /// </summary>
        private readonly IEnumerable<IEntityMapper> mappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryContextFactory" /> class.
        /// </summary>
        /// <param name="mappers">The mappers.</param>
        public QueryContextFactory(IEnumerable<IEntityMapper> mappers)
        {
            Contract.Requires(mappers != null);
            this.mappers = mappers;
        }

        /// <inheritdoc />
        public IReadContext Create()
        {
            return new ReadContext(new Lazy<DbContext>(() => new DataContext(this.mappers)));
        }
    }
}