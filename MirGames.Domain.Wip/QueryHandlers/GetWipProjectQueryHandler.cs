// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectQueryHandler.cs">
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
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.Services;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Public.Queries;

    /// <summary>
    /// Returns the WIP project.
    /// </summary>
    internal sealed class GetWipProjectQueryHandler : SingleItemQueryHandler<GetWipProjectQuery, WipProjectViewModel>
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
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// The empty logo provider.
        /// </summary>
        private readonly IProjectEmptyLogoProvider emptyLogoProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="textProcessor">The text processor.</param>
        /// <param name="emptyLogoProvider">The empty logo provider.</param>
        public GetWipProjectQueryHandler(
            IQueryProcessor queryProcessor,
            ISettings settings,
            IAuthorizationManager authorizationManager,
            ITextProcessor textProcessor,
            IProjectEmptyLogoProvider emptyLogoProvider)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(settings != null);
            Contract.Requires(authorizationManager != null);
            Contract.Requires(textProcessor != null);
            Contract.Requires(emptyLogoProvider != null);

            this.queryProcessor = queryProcessor;
            this.settings = settings;
            this.authorizationManager = authorizationManager;
            this.textProcessor = textProcessor;
            this.emptyLogoProvider = emptyLogoProvider;
        }

        /// <inheritdoc />
        protected override WipProjectViewModel Execute(IReadContext readContext, GetWipProjectQuery query, ClaimsPrincipal principal)
        {
            var project = readContext
                .Query<Project>()
                .SingleOrDefault(p => p.ProjectId == query.ProjectId || p.Alias == query.Alias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.ProjectId);
            }

            var projectViewModel = new WipProjectViewModel
                {
                    CreationDate = project.CreationDate,
                    Author = new AuthorViewModel
                        {
                            Id = project.AuthorId
                        },
                    Description = project.Description,
                    ShortDescription = this.textProcessor.GetShortText(project.Description),
                    Genre = project.Genre,
                    Alias = project.Alias,
                    FollowersCount = project.FollowersCount,
                    ProjectId = project.ProjectId,
                    Title = project.Title,
                    RepositoryType = project.RepositoryType,
                    UpdatedDate = project.UpdatedDate,
                    Version = project.Version,
                    Votes = project.Votes,
                    LastCommitMessage = project.LastCommitMessage,
                    VotesCount = project.VotesCount,
                    Tags = project.TagsList.Split(',').Select(t => t.Trim()).ToArray(),
                    CanEdit = this.authorizationManager.CheckAccess(principal, "Edit", "Project", project.ProjectId),
                    CanCreateBug = this.authorizationManager.CheckAccess(principal, "CreateBug", "Project", project.ProjectId),
                    CanCreateTask = this.authorizationManager.CheckAccess(principal, "CreateTask", "Project", project.ProjectId),
                    CanCreateFeature = this.authorizationManager.CheckAccess(principal, "CreateFeature", "Project", project.ProjectId),
                    CanReadRepository = this.authorizationManager.CheckAccess(principal, "Read", "GitRepository", project.RepositoryId),
                    CanEditGallery = this.authorizationManager.CheckAccess(principal, "EditGallery", "Project", project.ProjectId),
                    IsRepositoryPrivate = !this.authorizationManager.CheckAccess(0, "Read", "GitRepository", project.RepositoryId),
                    CanCreateBlogTopic = project.BlogId.HasValue && this.authorizationManager.CheckAccess(principal, "CreateTopic", "Blog", project.BlogId),
                    BlogId = project.BlogId
                };

            var attachment = this.queryProcessor.Process(new GetAttachmentsQuery
            {
                EntityId = project.ProjectId,
                EntityType = "project-logo", 
                IsImage = true
            }).FirstOrDefault();

            projectViewModel.LogoUrl = attachment != null
                                           ? attachment.AttachmentUrl
                                           : this.emptyLogoProvider.GetLogoUrl(project.Alias);

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
