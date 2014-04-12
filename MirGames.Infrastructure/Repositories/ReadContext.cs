﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ReadContext.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Repositories
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Context of reading.
    /// </summary>
    internal sealed class ReadContext : IReadContext
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadContext" /> class.
        /// </summary>
        /// <param name="context">The DB context.</param>
        public ReadContext(DbContext context)
        {
            Contract.Requires(context != null);
            this.context = context;
        }

        /// <inheritdoc />
        public DbQuery<T> Query<T>() where T : class
        {
            return this.context.Set<T>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.context != null)
            {
                this.context.Dispose();
                this.context = null;
            }
        }
    }
}
