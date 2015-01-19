// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryCacheContainer.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.Queries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Events;

    public abstract class QueryCacheContainer<TQuery> : IQueryCacheContainer, IEventListener where TQuery : Query
    {
        /// <summary>
        /// The cache manager factory.
        /// </summary>
        private readonly ICacheManagerFactory cacheManagerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryCacheContainer{TQuery}"/> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        protected QueryCacheContainer(ICacheManagerFactory cacheManagerFactory)
        {
            Contract.Requires(cacheManagerFactory != null);
            this.cacheManagerFactory = cacheManagerFactory;
        }

        /// <inheritdoc />
        IEnumerable<string> IEventListener.SupportedEventTypes
        {
            get { return this.InvalidationEvents; }
        }

        /// <summary>
        /// Gets the invalidation events.
        /// </summary>
        protected abstract IEnumerable<string> InvalidationEvents { get; }

        /// <inheritdoc />
        public bool CanHandle(Query query)
        {
            return query is TQuery;
        }

        /// <inheritdoc />
        IEnumerable IQueryCacheContainer.GetOrAdd(
            Query query,
            ClaimsPrincipal principal,
            PaginationSettings pagination,
            Func<IEnumerable> resultFactory)
        {
            var cacheManager = this.GetCacheManager(this.GetCacheDomain(principal, (TQuery)query));
            var cacheKey = this.GetCacheKey(principal, (TQuery)query, pagination);

            return cacheManager.GetOrAdd(
                cacheKey,
                resultFactory,
                GetExpiration());
        }

        /// <inheritdoc />
        int IQueryCacheContainer.GetOrAddItemsCount(Query query, ClaimsPrincipal principal, Func<int> resultFactory)
        {
            var cacheManager = this.GetCacheManager(this.GetCacheDomain(principal, (TQuery)query));
            var cacheKey = this.GetCacheKey(principal, (TQuery)query, null);

            return cacheManager.GetOrAdd(
                cacheKey + "#Count",
                resultFactory,
                DateTimeOffset.Now.AddHours(1));
        }

        /// <inheritdoc />
        bool IEventListener.CanProcess(Event @event)
        {
            return this.InvalidationEvents.Any(e => e.EqualsIgnoreCase(@event.EventType));
        }

        /// <inheritdoc />
        void IEventListener.Process(Event @event)
        {
            var listener = (IEventListener)this;

            if (!listener.CanProcess(@event))
            {
                throw new NotSupportedException(string.Format("Event of type {0} is not supported", @event.EventType));
            }

            this.Invalidate(@event);
        }

        /// <summary>
        /// Gets the cache domain.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="query">The query.</param>
        /// <returns>The cache domain.</returns>
        protected abstract string GetCacheDomain(ClaimsPrincipal principal, TQuery query);

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="query">The query.</param>
        /// <param name="pagination">The pagination.</param>
        /// <returns>The cache key.</returns>
        protected abstract string GetCacheKey(ClaimsPrincipal principal, TQuery query, PaginationSettings pagination);

        /// <summary>
        /// Invalidates the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        protected abstract void Invalidate(Event @event);

        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        /// <param name="cacheDomain">The cache domain.</param>
        /// <returns>The cache manager.</returns>
        protected ICacheManager GetCacheManager(string cacheDomain)
        {
            return this.cacheManagerFactory.Create(cacheDomain);
        }

        /// <summary>
        /// Gets the expiration.
        /// </summary>
        /// <returns>The expiration.</returns>
        private static DateTimeOffset GetExpiration()
        {
            return DateTimeOffset.Now.AddHours(1);
        }
    }
}