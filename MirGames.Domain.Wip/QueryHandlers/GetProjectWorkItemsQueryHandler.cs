// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetProjectWorkItemsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

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

    internal sealed class GetProjectWorkItemsQueryHandler : QueryHandler<GetProjectWorkItemsQuery, ProjectWorkItemViewModel>
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
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectWorkItemsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="textProcessor">The text processor.</param>
        public GetProjectWorkItemsQueryHandler(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager, ITextProcessor textProcessor)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(authorizationManager != null);

            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
            this.textProcessor = textProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetProjectWorkItemsQuery query, ClaimsPrincipal principal)
        {
            return this.GetQuery(readContext, query).Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<ProjectWorkItemViewModel> Execute(
            IReadContext readContext,
            GetProjectWorkItemsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var workItems = this.ApplyPagination(this.GetQuery(readContext, query), pagination).ToList();

            var workItemViewModels = workItems
                .Select(x => new ProjectWorkItemViewModel
                {
                    CreatedDate = x.CreatedDate,
                    Description = x.Description,
                    ShortDescription = this.textProcessor.GetShortText(x.Description),
                    InternalId = x.InternalId,
                    ItemType = x.ItemType,
                    Priority = x.Priority,
                    ProjectId = x.ProjectId,
                    State = x.State,
                    TagsList = x.TagsList,
                    Title = x.Title,
                    UpdatedDate = x.UpdatedDate,
                    WorkItemId = x.WorkItemId,
                    Author = new AuthorViewModel { Id = x.AuthorId },
                    Duration = x.Duration,
                    EndDate = x.EndDate,
                    ParentId = x.ParentId,
                    StartDate = x.StartDate,
                    CanBeDeleted = this.authorizationManager.CheckAccess(principal, "DeleteWorkItem", "Project", x.ProjectId),
                    CanBeEdited = this.authorizationManager.CheckAccess(principal, "EditWorkItem", "Project", x.ProjectId),
                    CanBeCommented = this.authorizationManager.CheckAccess(principal, "CommentWorkItem", "Project", x.ProjectId)
                })
                .ToList();

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = workItemViewModels.Select(x => x.Author)
            });

            return workItemViewModels;
        }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The project identifier.</returns>
        private int GetProjectId(GetProjectWorkItemsQuery query)
        {
            return this.queryProcessor.Process(new GetProjectIdQuery { ProjectAlias = query.ProjectAlias });
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The queryable set.</returns>
        private IQueryable<ProjectWorkItem> GetQuery(IReadContext readContext, GetProjectWorkItemsQuery query)
        {
            var projectId = this.GetProjectId(query);

            var workItems = readContext
                .Query<ProjectWorkItem>()
                .Where(p => p.ProjectId == projectId && p.State != WorkItemState.Removed);

            if (!string.IsNullOrEmpty(query.Tag))
            {
                var tags = readContext.Query<ProjectWorkItemTag>().Where(t => t.TagText == query.Tag);
                workItems = workItems.Join(
                    tags,
                    workItem => workItem.WorkItemId,
                    tag => tag.WorkItemId,
                    (topic, tag) => topic);
            }

            if (query.WorkItemType.HasValue)
            {
                workItems = workItems.Where(w => w.ItemType == query.WorkItemType.Value);
            }

            if (query.WorkItemState.HasValue)
            {
                workItems = workItems.Where(w => w.State == query.WorkItemState.Value);
            }
            else
            {
                workItems = workItems.Where(w => w.State != WorkItemState.Closed && w.State != WorkItemState.Removed);
            }

            switch (query.OrderBy)
            {
                case WorkItemsOrderType.StartDate:
                    return workItems.OrderByDescending(t => t.StartDate).ThenByDescending(t => t.Priority);
                case WorkItemsOrderType.Priority:
                    return workItems.OrderByDescending(t => t.Priority).ThenByDescending(t => t.StartDate);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
