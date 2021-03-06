// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CreateAttachmentCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Attachments.Entities;
    using MirGames.Domain.Attachments.Events;
    using MirGames.Domain.Attachments.Exceptions;
    using MirGames.Domain.Attachments.Services;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;
    using MirGames.Infrastructure.Utilities;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class CreateAttachmentCommandHandler : CommandHandler<CreateAttachmentCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The content type provider.
        /// </summary>
        private readonly IContentTypeProvider contentTypeProvider;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The upload processors.
        /// </summary>
        private readonly IEnumerable<IUploadProcessor> uploadProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateAttachmentCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="contentTypeProvider">The content type provider.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="uploadProcessors">The upload processors.</param>
        /// <param name="eventBus">The event bus.</param>
        public CreateAttachmentCommandHandler(
            IWriteContextFactory writeContextFactory,
            IContentTypeProvider contentTypeProvider,
            ISettings settings,
            ICommandProcessor commandProcessor,
            IEnumerable<IUploadProcessor> uploadProcessors,
            IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(contentTypeProvider != null);
            Contract.Requires(settings != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.contentTypeProvider = contentTypeProvider;
            this.settings = settings;
            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
            this.uploadProcessors = uploadProcessors.EnsureCollection();
        }

        /// <inheritdoc />
        protected override int Execute(CreateAttachmentCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            int userId = principal.GetUserId().GetValueOrDefault();

            var uploadProcessor =
                this.uploadProcessors.FirstOrDefault(processor => processor.CanProcess(command.EntityType));

            if (uploadProcessor == null)
            {
                throw new EntityTypeIsNotSupportedException(command.EntityType);
            }

            var fileInfo = new FileInfo(command.FilePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("Attachment could not be created because specified file have not been found", command.FilePath);
            }

            if (!uploadProcessor.IsValid(command.FilePath))
            {
                throw new FileLoadException("The specified file could not be loaded");
            }

            var directory = this.settings.GetValue<string>("Attachments.Directory");

            string relativeFilePath = Guid.NewGuid().ToString().GetMd5Hash();
            string filePath = Path.Combine(directory, relativeFilePath);
            fileInfo.MoveTo(filePath);

            bool isImage = this.contentTypeProvider.GetContentType(Path.GetExtension(command.FileName)).Contains("image");

            var attachment = new Attachment
            {
                CreatedDate = DateTime.UtcNow,
                EntityId = null,
                EntityType = command.EntityType,
                FileName = command.FileName,
                FilePath = relativeFilePath,
                IsPublished = false,
                UserId = userId,
                FileSize = (int)fileInfo.Length,
                AttachmentType = isImage ? "image" : "file"
            };

            authorizationManager.EnsureAccess(principal, "Create", "Attachment", attachment.AttachmentId);
            uploadProcessor.Process(filePath);

            using (var writeContext = this.writeContextFactory.Create())
            {
                writeContext.Set<Attachment>().Add(attachment);
                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "Publish", "Remove" },
                EntityId = attachment.AttachmentId,
                IsDenied = false,
                EntityType = "Attachment",
                UserId = userId
            });

            this.eventBus.Raise(new AttachmentCreatedEvent
            {
                AttachmentId = attachment.AttachmentId,
                EntityId = attachment.EntityId,
                EntityType = attachment.EntityType,
                UserId = attachment.UserId
            });

            return attachment.AttachmentId;
        }
    }
}