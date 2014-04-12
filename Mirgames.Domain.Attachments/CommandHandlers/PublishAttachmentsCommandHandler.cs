// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PublishAttachmentsCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Attachments.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the publishing of attachments
    /// </summary>
    internal sealed class PublishAttachmentsCommandHandler : CommandHandler<PublishAttachmentsCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishAttachmentsCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public PublishAttachmentsCommandHandler(IWriteContextFactory writeContextFactory)
        {
            Contract.Requires(writeContextFactory != null);

            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Execute(PublishAttachmentsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command.Identifiers != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var identifiers = command.Identifiers.ToArray();
                var attachments = writeContext.Set<Attachment>().Where(x => identifiers.Contains(x.AttachmentId) && !x.IsPublished).ToList();

                foreach (var attachment in attachments)
                {
                    authorizationManager.EnsureAccess(principal, "Publish", attachment);
                    attachment.IsPublished = true;
                    attachment.EntityId = command.EntityId;
                }

                writeContext.SaveChanges();
            }
        }
    }
}