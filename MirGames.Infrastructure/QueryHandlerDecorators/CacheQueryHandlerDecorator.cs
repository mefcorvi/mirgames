// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CacheQueryHandlerDecorator.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.QueryHandlerDecorators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Caches the query results.
    /// </summary>
    internal sealed class CacheQueryHandlerDecorator : IQueryHandlerDecorator
    {
        /// <summary>
        /// The query cache containers.
        /// </summary>
        private readonly IEnumerable<IQueryCacheContainer> queryCacheContainers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheQueryHandlerDecorator"/> class.
        /// </summary>
        /// <param name="queryCacheContainers">The query cache containers.</param>
        public CacheQueryHandlerDecorator(IEnumerable<IQueryCacheContainer> queryCacheContainers)
        {
            Contract.Requires(queryCacheContainers != null);
            this.queryCacheContainers = queryCacheContainers;
        }

        /// <inheritdoc />
        public float Order
        {
            get { return 0; }
        }

        /// <inheritdoc />
        public IQueryHandler Decorate(IQueryHandler queryHandler)
        {
            return new CacheQueryHandler(queryHandler, this.queryCacheContainers);
        }

        private class CacheQueryHandler : IQueryHandler
        {
            /// <summary>
            /// The inner.
            /// </summary>
            private readonly IQueryHandler inner;

            /// <summary>
            /// The query cache containers.
            /// </summary>
            private readonly IEnumerable<IQueryCacheContainer> queryCacheContainers;

            /// <summary>
            /// Initializes a new instance of the <see cref="CacheQueryHandler" /> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="queryCacheContainers">The query cache containers.</param>
            public CacheQueryHandler(IQueryHandler inner, IEnumerable<IQueryCacheContainer> queryCacheContainers)
            {
                Contract.Requires(inner != null);
                Contract.Requires(queryCacheContainers != null);

                this.inner = inner;
                this.queryCacheContainers = queryCacheContainers.EnsureCollection();
            }

            /// <inheritdoc />
            public Type QueryType
            {
                get { return this.inner.QueryType; }
            }

            /// <inheritdoc />
            public IEnumerable Execute(IReadContext readContext, Query query, ClaimsPrincipal principal, PaginationSettings pagination)
            {
                var cacheContainer = this.queryCacheContainers.FirstOrDefault(c => c.CanHandle(query));

                if (cacheContainer == null)
                {
                    return this.inner.Execute(readContext, query, principal, pagination);
                }

                return cacheContainer.GetOrAdd(
                    query,
                    principal,
                    pagination,
                    () => this.inner.Execute(readContext, query, principal, pagination));
            }

            /// <inheritdoc />
            public int GetItemsCount(IReadContext readContext, Query query, ClaimsPrincipal principal)
            {
                var cacheContainer = this.queryCacheContainers.FirstOrDefault(c => c.CanHandle(query));

                if (cacheContainer == null)
                {
                    return this.inner.GetItemsCount(readContext, query, principal);
                }

                return cacheContainer.GetOrAddItemsCount(
                    query,
                    principal,
                    () => this.inner.GetItemsCount(readContext, query, principal));
            }
        }
    }
}