// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetAttachmentsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Entities;
    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Attachments.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Utilities;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    internal sealed class GetAttachmentsQueryHandler : QueryHandler<GetAttachmentsQuery, AttachmentViewModel>
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The content type provider.
        /// </summary>
        private readonly IContentTypeProvider contentTypeProvider;

        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAttachmentsQueryHandler" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="contentTypeProvider">The content type provider.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetAttachmentsQueryHandler(ISettings settings, IContentTypeProvider contentTypeProvider, IReadContextFactory readContextFactory)
        {
            Contract.Requires(settings != null);
            Contract.Requires(contentTypeProvider != null);
            Contract.Requires(readContextFactory != null);

            this.settings = settings;
            this.contentTypeProvider = contentTypeProvider;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetAttachmentsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return GetAttachmentQuery(readContext, query).Count();
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<AttachmentViewModel> Execute(GetAttachmentsQuery query, ClaimsPrincipal principal, PaginationSettings paginationSettings)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var attachmentQuery = GetAttachmentQuery(readContext, query);

                var attachmentsUrl = this.settings.GetValue<string>("Attachments.Url");
                var directory = this.settings.GetValue<string>("Attachments.Directory");

                return this.ApplyPagination(attachmentQuery, paginationSettings)
                           .ToList()
                           .Select(a => new AttachmentViewModel
                           {
                               AttachmentId = a.AttachmentId,
                               IsImage = a.AttachmentType == "image",
                               CreatedDate = a.CreatedDate,
                               EntityId = a.EntityId,
                               EntityType = a.EntityType,
                               FileName = a.FileName,
                               FileSize = a.FileSize,
                               UserId = a.UserId,
                               AttachmentUrl = string.Format(attachmentsUrl, a.AttachmentId),
                               FilePath = Path.Combine(directory, a.FilePath),
                               ContentType = this.contentTypeProvider.GetContentType(Path.GetExtension(a.FileName))
                           });
            }
        }

        /// <summary>
        /// Gets the attachment query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The attachment query.</returns>
        private static IQueryable<Attachment> GetAttachmentQuery(IReadContext readContext, GetAttachmentsQuery query)
        {
            IQueryable<Attachment> attachmentQuery = readContext
                .Query<Attachment>();

            if (query.IsImage == true)
            {
                attachmentQuery = attachmentQuery.Where(a => a.AttachmentType == "image");
            }

            if (query.IsImage == false)
            {
                attachmentQuery = attachmentQuery.Where(a => a.AttachmentType != "image");
            }

            if (query.EntityId.HasValue)
            {
                attachmentQuery = attachmentQuery.Where(a => a.EntityId == query.EntityId);
            }

            if (!string.IsNullOrWhiteSpace(query.EntityType))
            {
                attachmentQuery = attachmentQuery.Where(a => a.EntityType == query.EntityType);
            }

            switch (query.OrderingBy)
            {
                case AttachmentsOrderingType.CreationDateAscending:
                    attachmentQuery = attachmentQuery.OrderBy(a => a.CreatedDate);
                    break;
                case AttachmentsOrderingType.CreationDateDescending:
                    attachmentQuery = attachmentQuery.OrderByDescending(a => a.CreatedDate);
                    break;
                case AttachmentsOrderingType.Random:
                    attachmentQuery = attachmentQuery.OrderBy(a => Guid.NewGuid());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return attachmentQuery;
        }
    }
}