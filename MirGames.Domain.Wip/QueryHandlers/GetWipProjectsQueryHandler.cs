// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Queries;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.Services;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Returns the WIP projects.
    /// </summary>
    internal sealed class GetWipProjectsQueryHandler : QueryHandler<GetWipProjectsQuery, WipProjectViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The empty logo provider.
        /// </summary>
        private readonly IProjectEmptyLogoProvider emptyLogoProvider;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="emptyLogoProvider">The empty logo provider.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetWipProjectsQueryHandler(
            IQueryProcessor queryProcessor,
            IAuthorizationManager authorizationManager,
            IProjectEmptyLogoProvider emptyLogoProvider,
            IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(authorizationManager != null);
            Contract.Requires(emptyLogoProvider != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
            this.emptyLogoProvider = emptyLogoProvider;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(GetWipProjectsQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return this.GetQuery(readContext, query).Count();
            }
        }

        /// <inheritdoc />
        protected override IEnumerable<WipProjectViewModel> Execute(GetWipProjectsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            List<Project> projectsData;
            using (var readContext = this.readContextFactory.Create())
            {
                projectsData = this.ApplyPagination(this.GetQuery(readContext, query).OrderByDescending(p => p.UpdatedDate), pagination)
                                   .ToList();
            }

            var projects = projectsData
                    .Select(
                        p => new WipProjectViewModel
                        {
                            CreationDate = p.CreationDate,
                            Author = new AuthorViewModel
                            {
                                Id = p.AuthorId
                            },
                            Alias = p.Alias,
                            Description = p.Description,
                            Genre = p.Genre,
                            ShortDescription = p.ShortDescription,
                            FollowersCount = p.FollowersCount,
                            ProjectId = p.ProjectId,
                            Title = p.Title,
                            UpdatedDate = p.UpdatedDate,
                            Version = p.Version,
                            Votes = p.Votes,
                            VotesCount = p.VotesCount,
                            LastCommitMessage = p.LastCommitMessage,
                            Tags = p.TagsList.Split(',').Select(t => t.Trim()).ToArray(),
                            CanEdit = this.authorizationManager.CheckAccess(principal, "Edit", "Project", p.ProjectId),
                            CanCreateBug = this.authorizationManager.CheckAccess(principal, "CreateBug", "Project", p.ProjectId),
                            CanCreateTask = this.authorizationManager.CheckAccess(principal, "CreateTask", "Project", p.ProjectId),
                            CanCreateFeature = this.authorizationManager.CheckAccess(principal, "CreateFeature", "Project", p.ProjectId),
                            CanReadRepository = this.authorizationManager.CheckAccess(principal, "Read", "GitRepository", p.RepositoryId),
                            CanEditGallery = this.authorizationManager.CheckAccess(principal, "EditGallery", "Project", p.ProjectId),
                            IsRepositoryPrivate = !this.authorizationManager.CheckAccess(0, "Read", "GitRepository", p.RepositoryId),
                            IsSiteEnabled = p.IsSiteEnabled,
                            CanCreateBlogTopic = p.BlogId.HasValue && this.authorizationManager.CheckAccess(principal, "CreateTopic", "Blog", p.BlogId),
                            BlogId = p.BlogId
                        })
                    .ToList();

            projects.ForEach(project =>
            {
                var attachment = this.queryProcessor.Process(new GetAttachmentsQuery
                {
                    EntityId = project.ProjectId,
                    EntityType = "project-logo",
                    IsImage = true
                }).FirstOrDefault();

                if (attachment != null)
                {
                    project.LogoUrl = attachment.AttachmentUrl;
                }
                else
                {
                    project.LogoUrl = this.emptyLogoProvider.GetLogoUrl(project.Alias);
                }
            });

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = projects.Select(p => p.Author)
                    });

            return projects;
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The queryable results.</returns>
        private IQueryable<Project> GetQuery(IReadContext readContext, GetWipProjectsQuery query)
        {
            IQueryable<Project> projects = readContext.Query<Project>();

            if (!string.IsNullOrWhiteSpace(query.Tag))
            {
                var tags = readContext.Query<ProjectTag>().Where(t => t.TagText == query.Tag);
                projects = projects.Join(
                    tags,
                    project => project.ProjectId,
                    tag => tag.ProjectId,
                    (topic, tag) => topic);
            }

            return projects;
        }
    }
}
