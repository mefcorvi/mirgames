﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectForEditQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Public.Queries;

    /// <summary>
    /// Returns the WIP project.
    /// </summary>
    internal sealed class GetWipProjectForEditQueryHandler : SingleItemQueryHandler<GetWipProjectForEditQuery, WipProjectForEditViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectForEditQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetWipProjectForEditQueryHandler(IQueryProcessor queryProcessor, ISettings settings, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(settings != null);
            Contract.Requires(authorizationManager != null);

            this.queryProcessor = queryProcessor;
            this.settings = settings;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public override WipProjectForEditViewModel Execute(IReadContext readContext, GetWipProjectForEditQuery query, ClaimsPrincipal principal)
        {
            var project = readContext
                .Query<Project>()
                .SingleOrDefault(p => p.ProjectId == query.ProjectId || p.Alias == query.Alias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.ProjectId);
            }

            this.authorizationManager.EnsureAccess(principal, "Edit", "Project", project.ProjectId);

            var projectViewModel = new WipProjectForEditViewModel
                {
                    CreationDate = project.CreationDate,
                    Author = new AuthorViewModel
                        {
                            Id = project.AuthorId
                        },
                    Description = project.Description,
                    Alias = project.Alias,
                    ProjectId = project.ProjectId,
                    Title = project.Title,
                    RepositoryType = project.RepositoryType,
                    UpdatedDate = project.UpdatedDate,
                    Version = project.Version,
                    Tags = project.TagsList.Split(',').Select(t => t.Trim()).ToArray(),
                    IsRepositoryPrivate = !this.authorizationManager.CheckAccess(0, "Read", "GitRepository", project.RepositoryId)
                };

            var attachment = this.queryProcessor.Process(new GetAttachmentsQuery
            {
                EntityId = project.ProjectId,
                EntityType = "project-logo", 
                IsImage = true
            }).FirstOrDefault();

            if (attachment != null)
            {
                projectViewModel.LogoUrl = attachment.AttachmentUrl;
            }

            if (project.RepositoryId.HasValue && project.RepositoryType.EqualsIgnoreCase("git"))
            {
                var repository = this.queryProcessor.Process(new GetRepositoryQuery
                {
                    RepositoryId = project.RepositoryId.GetValueOrDefault()
                });

                projectViewModel.RepositoryUrl =
                    string.Format(this.settings.GetValue<string>("Repositories.Git.Url"), repository.RepositoryName);
            }

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = new[] { projectViewModel.Author }
                    });

            return projectViewModel;
        }
    }
}