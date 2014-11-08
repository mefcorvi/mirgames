// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AttachmentInfoCacheContainer.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Attachments.Cache
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Events;
    using MirGames.Domain.Attachments.Queries;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;

    internal sealed class AttachmentInfoCacheContainer : QueryCacheContainer<GetAttachmentInfoQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentInfoCacheContainer"/> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public AttachmentInfoCacheContainer(ICacheManagerFactory cacheManagerFactory)
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
                    "Attachments.AttachmentRemoved"
                };
            }
        }

        /// <inheritdoc />
        protected override string GetCacheDomain(ClaimsPrincipal principal, GetAttachmentInfoQuery query)
        {
            return "GetAttachmentInfoQuery";
        }

        /// <inheritdoc />
        protected override string GetCacheKey(
            ClaimsPrincipal principal,
            GetAttachmentInfoQuery query,
            PaginationSettings pagination)
        {
            return query.AttachmentId.ToString(CultureInfo.InvariantCulture);
        }

        /// <inheritdoc />
        protected override void Invalidate(Event @event)
        {
            var attachmentRemovedEvent = @event as AttachmentRemovedEvent;

            if (attachmentRemovedEvent != null)
            {
                var cacheManager = this.GetCacheManager("GetAttachmentInfoQuery");
                cacheManager.Remove(attachmentRemovedEvent.AttachmentId.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
