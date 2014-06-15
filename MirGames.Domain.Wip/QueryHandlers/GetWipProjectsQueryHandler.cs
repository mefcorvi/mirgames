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
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
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

        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="textProcessor">The text processor.</param>
        public GetWipProjectsQueryHandler(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager, ITextProcessor textProcessor)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(authorizationManager != null);
            Contract.Requires(textProcessor != null);

            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
            this.textProcessor = textProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetWipProjectsQuery query, ClaimsPrincipal principal)
        {
            return this.GetQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<WipProjectViewModel> Execute(IReadContext readContext, GetWipProjectsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var projects =
                this.ApplyPagination(this.GetQuery(readContext, query).OrderByDescending(p => p.UpdatedDate), pagination)
                    .ToList()
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
                            ShortDescription = this.textProcessor.GetShortText(p.Description),
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
                            CanReadRepository = this.authorizationManager.CheckAccess(principal, "Read", "GitRepository", p.RepositoryId)
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
