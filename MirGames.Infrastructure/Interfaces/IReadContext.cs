// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IReadContext.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System;
    using System.Linq;

    /// <summary>
    /// Represents a data read context.
    /// </summary>
    public interface IReadContext : IDisposable
    {
        /// <summary>
        /// Creates a new query.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <returns>The query.</returns>
        IQueryable<T> Query<T>() where T : class;
    }
}
