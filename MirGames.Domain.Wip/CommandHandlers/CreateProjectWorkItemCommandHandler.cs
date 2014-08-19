// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CreateProjectWorkItemCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Creates the project work item.
    /// </summary>
    internal sealed class CreateProjectWorkItemCommandHandler : CommandHandler<CreateNewProjectWorkItemCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateProjectWorkItemCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public CreateProjectWorkItemCommandHandler(
            IWriteContextFactory writeContextFactory,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override int Execute(
            CreateNewProjectWorkItemCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(command.Tags != null);

            int userId = principal.GetUserId().GetValueOrDefault();
            ProjectWorkItem projectWorkItem;

            using (var writeContext = this.writeContextFactory.Create())
            {
                var project = writeContext.Set<Project>().FirstOrDefault(p => p.Alias == command.ProjectAlias);

                if (project == null)
                {
                    throw new ItemNotFoundException("Project", command.ProjectAlias);
                }

                EnsureAccess(command, principal, authorizationManager, project);

                var newInternalId = writeContext
                                        .Set<ProjectWorkItem>()
                                        .Where(p => p.ProjectId == project.ProjectId)
                                        .Max(p => (int?)p.InternalId)
                                        .GetValueOrDefault() + 1;

                projectWorkItem = new ProjectWorkItem
                {
                    AuthorId = userId,
                    CreatedDate = DateTime.UtcNow,
                    Description = command.Description,
                    Duration = null,
                    EndDate = null,
                    ItemType = command.Type,
                    ParentId = null,
                    ProjectId = project.ProjectId,
                    StartDate = null,
                    State = WorkItemState.Open,
                    UpdatedDate = DateTime.UtcNow,
                    Title = command.Title,
                    TagsList = command.Tags ?? string.Empty,
                    InternalId = newInternalId,
                    AssignedTo = command.AssignedTo ?? userId
                };

                writeContext.Set<ProjectWorkItem>().Add(projectWorkItem);
                writeContext.SaveChanges();

                if (!string.IsNullOrEmpty(command.Tags))
                {
                    foreach (var tag in command.Tags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        writeContext.Set<ProjectWorkItemTag>()
                                    .Add(new ProjectWorkItemTag
                                    {
                                        TagText = tag.Trim(),
                                        WorkItemId = projectWorkItem.WorkItemId
                                    });
                    }

                    writeContext.SaveChanges();
                }
            }

            this.commandProcessor.Execute(new PublishAttachmentsCommand
            {
                EntityId = projectWorkItem.WorkItemId,
                Identifiers = command.Attachments
            });

            return projectWorkItem.InternalId;
        }

        /// <summary>
        /// Ensures the access.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="principal">The principal.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="project">The project.</param>
        private static void EnsureAccess(
            CreateNewProjectWorkItemCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager,
            Project project)
        {
            if (command.Type == WorkItemType.Bug)
            {
                authorizationManager.EnsureAccess(principal, "CreateBug", "Project", project.ProjectId);
            }

            if (command.Type == WorkItemType.Feature)
            {
                authorizationManager.EnsureAccess(principal, "CreateFeature", "Project", project.ProjectId);
            }

            if (command.Type == WorkItemType.Task)
            {
                authorizationManager.EnsureAccess(principal, "CreateTask", "Project", project.ProjectId);
            }
        }
    }
}