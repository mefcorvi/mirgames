// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetAttachmentInfoQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.QueryHandlers
{
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
    internal sealed class GetAttachmentInfoQueryHandler : SingleItemQueryHandler<GetAttachmentInfoQuery, AttachmentViewModel>
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The content type provider.
        /// </summary>
        private readonly IContentTypeProvider contentTypeProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAttachmentInfoQueryHandler" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="contentTypeProvider">The content type provider.</param>
        public GetAttachmentInfoQueryHandler(ISettings settings, IContentTypeProvider contentTypeProvider)
        {
            Contract.Requires(settings != null);
            Contract.Requires(contentTypeProvider != null);

            this.settings = settings;
            this.contentTypeProvider = contentTypeProvider;
        }

        /// <inheritdoc />
        public override AttachmentViewModel Execute(IReadContext readContext, GetAttachmentInfoQuery query, ClaimsPrincipal principal)
        {
            var attachment = readContext
                .Query<Attachment>()
                .Where(a => a.AttachmentId == query.AttachmentId)
                .Select(a => new AttachmentViewModel
                    {
                        AttachmentId = a.AttachmentId,
                        IsImage = a.AttachmentType == "image",
                        CreatedDate = a.CreatedDate,
                        EntityId = a.EntityId,
                        EntityType = a.EntityType,
                        FileName = a.FileName,
                        FilePath = a.FilePath,
                        FileSize = a.FileSize,
                        UserId = a.UserId
                    })
                .FirstOrDefault();

            if (attachment == null)
            {
                return null;
            }

            attachment.AttachmentUrl = string.Format(this.settings.GetValue<string>("Attachments.Url"), attachment.AttachmentId);

            var directory = this.settings.GetValue<string>("Attachments.Directory");
            attachment.FilePath = Path.Combine(directory, attachment.FilePath);

            var extension = Path.GetExtension(attachment.FileName);
            attachment.ContentType = this.contentTypeProvider.GetContentType(extension);

            return attachment;
        }
    }
}