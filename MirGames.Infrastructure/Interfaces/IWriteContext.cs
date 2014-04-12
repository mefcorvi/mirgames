// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IWriteContext.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// Represents the write context.
    /// </summary>
    public interface IWriteContext : IDisposable
    {
        /// <summary>
        /// Returns the set of items.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <returns>The set of items.</returns>
        DbSet<T> Set<T>() where T : class;

        /// <summary>
        /// Saves the changes.
        /// </summary>
        void SaveChanges();
    }
}