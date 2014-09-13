// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumsQueryCacheContainer.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.Cache
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Queries;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;

    internal sealed class ForumsQueryCacheContainer : QueryCacheContainer<GetForumsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumsQueryCacheContainer"/> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public ForumsQueryCacheContainer(ICacheManagerFactory cacheManagerFactory)
            : base(cacheManagerFactory)
        {
        }

        /// <inheritdoc />
        protected override IEnumerable<string> InvalidationEvents
        {
            get
            {
                return new[]
                {
                    "Forum.ForumPostDeleted",
                    "Forum.ForumPostUpdated",
                    "Forum.TopicCreated",
                    "Forum.ForumTopicDeleted",
                    "Forum.TopicReplied"
                };
            }
        }

        /// <inheritdoc />
        protected override string GetCacheDomain(ClaimsPrincipal principal, GetForumsQuery query)
        {
            return "GetForumsQuery";
        }

        /// <inheritdoc />
        protected override string GetCacheKey(
            ClaimsPrincipal principal,
            GetForumsQuery query,
            PaginationSettings pagination)
        {
            return (pagination != null ? pagination.GetHashCode() : 0).ToString(CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        protected override void Invalidate(Event @event)
        {
            var cacheManager = this.GetCacheManager("GetForumsQuery");
            cacheManager.Clear();
        }
    }
}
