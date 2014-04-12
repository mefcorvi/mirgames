// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryItemPostProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Queries
{
    using System;

    /// <summary>
    /// Post processing of results of query.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public interface IQueryItemPostProcessor<in T> : IQueryItemPostProcessor
    {
        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Process(T item);
    }

    /// <summary>
    /// Post processing of results of query.
    /// </summary>
    public interface IQueryItemPostProcessor
    {
        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Process(object item);
    }

    /// <summary>
    /// Post processing of results of query.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    public abstract class QueryItemPostProcessor<T> : IQueryItemPostProcessor<T>
    {
        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        public Type ItemType
        {
            get { return typeof(T); }
        }

        /// <inheritdoc />
        void IQueryItemPostProcessor<T>.Process(T item)
        {
            this.Process(item);
        }

        /// <inheritdoc />
        void IQueryItemPostProcessor.Process(object item)
        {
            this.Process((T)item);
        }

        /// <summary>
        /// Processes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The result of processing.</returns>
        protected abstract void Process(T item);
    }
}
