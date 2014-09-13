// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetProjectWorkItemQueryHandler.cs">
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

    using MirGames.Domain.Exceptions;
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

    internal sealed class GetProjectWorkItemQueryHandler : SingleItemQueryHandler<GetProjectWorkItemQuery, ProjectWorkItemViewModel>
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
        /// Initializes a new instance of the <see cref="GetProjectWorkItemQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="textProcessor">The text processor.</param>
        public GetProjectWorkItemQueryHandler(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager, ITextProcessor textProcessor)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(authorizationManager != null);

            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
            this.textProcessor = textProcessor;
        }

        /// <inheritdoc />
        protected override ProjectWorkItemViewModel Execute(
            IReadContext readContext,
            GetProjectWorkItemQuery query,
            ClaimsPrincipal principal)
        {
            var projectId = this.GetProjectId(query);

            var workItem = readContext
                .Query<ProjectWorkItem>()
                .FirstOrDefault(p => p.ProjectId == projectId && p.InternalId == query.InternalId);

            if (workItem == null)
            {
                throw new ItemNotFoundException("ProjectWorkItem", query.InternalId);
            }

            var workItemViewModel = new ProjectWorkItemViewModel
            {
                CreatedDate = workItem.CreatedDate,
                Description = this.textProcessor.GetHtml(workItem.Description),
                ShortDescription = this.textProcessor.GetShortText(workItem.Description),
                InternalId = workItem.InternalId,
                ItemType = workItem.ItemType,
                Priority = workItem.Priority,
                ProjectId = workItem.ProjectId,
                State = workItem.State,
                TagsList = workItem.TagsList,
                Title = workItem.Title,
                UpdatedDate = workItem.UpdatedDate,
                WorkItemId = workItem.WorkItemId,
                Author = new AuthorViewModel { Id = workItem.AuthorId },
                AssignedTo = new AuthorViewModel { Id = workItem.AssignedTo },
                Duration = workItem.Duration,
                EndDate = workItem.EndDate,
                ParentId = workItem.ParentId,
                StartDate = workItem.StartDate,
                CanBeDeleted = this.authorizationManager.CheckAccess(principal, "DeleteWorkItem", "Project", workItem.ProjectId),
                CanBeEdited = this.authorizationManager.CheckAccess(principal, "EditWorkItem", "Project", workItem.ProjectId),
                CanBeCommented = this.authorizationManager.CheckAccess(principal, "CommentWorkItem", "Project", workItem.ProjectId)
            }; 

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = new[] { workItemViewModel.Author, workItemViewModel.AssignedTo }
            });

            return workItemViewModel;
        }

        /// <summary>
        /// Gets the project identifier.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The project identifier.</returns>
        private int GetProjectId(GetProjectWorkItemQuery query)
        {
            return this.queryProcessor.Process(new GetProjectIdQuery { ProjectAlias = query.ProjectAlias });
        }
    }
}