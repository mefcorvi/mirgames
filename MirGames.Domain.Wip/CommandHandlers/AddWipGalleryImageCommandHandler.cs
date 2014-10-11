// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AddWipGalleryImageCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Adds the new gallery image.
    /// </summary>
    internal sealed class AddWipGalleryImageCommandHandler : CommandHandler<AddWipGalleryImageCommand>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddWipGalleryImageCommandHandler" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public AddWipGalleryImageCommandHandler(IReadContextFactory readContextFactory, ICommandProcessor commandProcessor)
        {
            Contract.Requires(readContextFactory != null);
            Contract.Requires(commandProcessor != null);

            this.readContextFactory = readContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        protected override void Execute(AddWipGalleryImageCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Project project;

            using (var readContext = this.readContextFactory.Create())
            {
                project = GetProject(readContext, command);
            }

            authorizationManager.EnsureAccess(principal, "EditGallery", "Project", project.ProjectId);

            this.commandProcessor.Execute(new PublishAttachmentsCommand
            {
                EntityId = project.ProjectId,
                Identifiers = command.Attachments
            });
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="command">The command.</param>
        /// <returns>The project.</returns>
        private static Project GetProject(IReadContext readContext, AddWipGalleryImageCommand command)
        {
            var project = readContext.Query<Project>().SingleOrDefault(p => p.Alias == command.ProjectAlias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", command.ProjectAlias);
            }

            return project;
        }
    }
}
