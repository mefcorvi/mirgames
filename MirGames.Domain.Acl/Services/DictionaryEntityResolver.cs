// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="DictionaryEntityResolver.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Acl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MirGames.Infrastructure;

    /// <summary>
    /// Resolves the entity by the key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    internal abstract class DictionaryEntityResolver<TKey, TEntity> : IDictionaryEntityResolver<TKey, TEntity>
        where TEntity : class
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The entities dictionary.
        /// </summary>
        private readonly Lazy<IEnumerable<TEntity>> entitiesDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryEntityResolver{TKey, TEntity}"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        protected DictionaryEntityResolver(IReadContextFactory readContextFactory)
        {
            this.readContextFactory = readContextFactory;
            this.entitiesDictionary = new Lazy<IEnumerable<TEntity>>(this.LoadDictionary);
        }

        /// <summary>
        /// Gets the entities dictionary.
        /// </summary>
        public IEnumerable<TEntity> Entities
        {
            get { return this.entitiesDictionary.Value; }
        }

        /// <inheritdoc />
        public virtual IEnumerable<TEntity> Resolve(TKey key)
        {
            return this.Entities.Where(e => this.IsSatisfied(e, key));
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        /// The key of the entity.
        /// </returns>
        protected abstract bool IsSatisfied(TEntity entity, TKey key);

        /// <summary>
        /// Loads the dictionary.
        /// </summary>
        /// <returns>The dictionary.</returns>
        private IEnumerable<TEntity> LoadDictionary()
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return readContext.Query<TEntity>().ToList();
            }
        }
    }
}