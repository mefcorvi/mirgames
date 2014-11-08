namespace MirGames.Domain.Attachments.Cache
{
    using System.Collections.Generic;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Events;
    using MirGames.Domain.Attachments.Queries;
    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Queries;

    internal sealed class AttachmentsCacheContainer : QueryCacheContainer<GetAttachmentsQuery>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachmentsCacheContainer" /> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        public AttachmentsCacheContainer(ICacheManagerFactory cacheManagerFactory)
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
                    "Attachments.AttachmentCreated",
                    "Attachments.AttachmentPublished",
                    "Attachments.AttachmentRemoved"
                };
            }
        }

        /// <inheritdoc />
        protected override string GetCacheDomain(ClaimsPrincipal principal, GetAttachmentsQuery query)
        {
            return string.Format("GetAttachmentInfoQuery_{0}_{1}", query.EntityId, query.EntityType);
        }

        /// <inheritdoc />
        protected override string GetCacheKey(
            ClaimsPrincipal principal,
            GetAttachmentsQuery query,
            PaginationSettings pagination)
        {
            return string.Format(
                "GetAttachmentInfoQuery_{0}_{1}_{2}_{3}_{4}",
                query.EntityId,
                query.EntityType,
                query.IsImage,
                query.OrderingBy,
                pagination != null ? pagination.GetHashCode() : 0);
        }

        /// <inheritdoc />
        protected override void Invalidate(Event @event)
        {
            var attachmentEvent = @event as AttachmentBaseEvent;

            if (attachmentEvent != null)
            {
                var domain = string.Format("GetAttachmentInfoQuery_{0}_{1}", attachmentEvent.EntityId, attachmentEvent.EntityType);
                var cacheManager = this.GetCacheManager(domain);
                cacheManager.Clear();
            }
        }
    }
}