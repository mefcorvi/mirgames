// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WriteContext.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Repositories
{
    using System.Data.Entity;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Context of writing.
    /// </summary>
    internal sealed class WriteContext : IWriteContext
    {
        /// <summary>
        /// The data context.
        /// </summary>
        private DbContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteContext"/> class.
        /// </summary>
        /// <param name="context">The DB context.</param>
        public WriteContext(DbContext context)
        {
            Contract.Requires(context != null);
            this.context = context;
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

        /// <inheritdoc />
        public DbSet<T> Set<T>() where T : class
        {
            return this.context.Set<T>();
        }

        /// <inheritdoc />
        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}