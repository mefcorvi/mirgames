// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CreateNewWipProjectCommandHandler.cs">
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

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Public.Commands;

    internal sealed class CreateNewWipProjectCommandHandler : CommandHandler<CreateNewWipProjectCommand, string>
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
        /// Initializes a new instance of the <see cref="CreateNewWipProjectCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public CreateNewWipProjectCommandHandler(
            IWriteContextFactory writeContextFactory,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        protected override string Execute(
            CreateNewWipProjectCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(command.Alias != null);

            command.Alias = command.Alias.ToLowerInvariant();
            int userId = principal.GetUserId().GetValueOrDefault();
            Project project;

            using (var writeContext = this.writeContextFactory.Create())
            {
                if (writeContext.Set<Project>().Any(p => p.Alias == command.Alias))
                {
                    throw new ProjectAlreadyCreatedException(command.Alias);
                }

                var tags = command
                    .Tags
                    .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(t => !string.IsNullOrWhiteSpace(t))
                    .ToList();

                project = new Project
                {
                    Alias = command.Alias,
                    AuthorId = userId,
                    CreationDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Description = command.Description,
                    FollowersCount = 0,
                    RepositoryType = command.RepositoryType,
                    TagsList = string.Join(", ", tags),
                    Title = command.Title,
                    Version = "1",
                    Votes = 0,
                    VotesCount = 0
                };

                writeContext.Set<Project>().Add(project);
                writeContext.SaveChanges();

                project.BlogId = this.CreateBlog(project);
                project.RepositoryId = this.CreateRepository(project);
                writeContext.SaveChanges();

                foreach (var tag in tags)
                {
                    writeContext.Set<ProjectTag>().Add(new ProjectTag { TagText = tag.Trim(), ProjectId = project.ProjectId });
                }

                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(new PublishAttachmentsCommand
            {
                EntityId = project.ProjectId,
                Identifiers = new[] { command.LogoAttachmentId }
            });

            this.commandProcessor.Execute(new PublishAttachmentsCommand
            {
                EntityId = project.ProjectId,
                Identifiers = command.Attachments
            });

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "Read" },
                EntityId = project.RepositoryId,
                EntityType = "GitRepository",
            });
             
            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "CommentWorkItem", "CreateFeature", "CreateBug", "Read", "ViewStatistics" },
                EntityId = project.ProjectId,
                EntityType = "Project",
            });

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "Edit", "EditGallery", "CreateTask", "DeleteWorkItem", "EditWorkItem", "ChangeStateWorkItem", "AssignWorkItem" },
                EntityId = project.ProjectId,
                EntityType = "Project",
                UserId = project.AuthorId
            });

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "CreateTopic", "EditTopic", "DeleteTopic" },
                EntityId = project.BlogId.Value,
                EntityType = "Blog",
                UserId = project.AuthorId
            });

            return project.Alias;
        }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The repository identifier.</returns>
        private int CreateRepository(Project project)
        {
            return this.commandProcessor.Execute(new InitRepositoryCommand
            {
                RepositoryName = project.Alias,
                Title = "Репозиторий проекта " + project.Title
            });
        }

        /// <summary>
        /// Creates the blog.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns>The blog identifier.</returns>
        private int CreateBlog(Project project)
        {
            return this.commandProcessor.Execute(new AddNewBlogCommand
            {
                Alias = project.Alias,
                Description = "Проект " + project.Title,
                Title = project.Title,
                EntityId = project.ProjectId,
                EntityType = "Project"
            });
        }
    }
}
