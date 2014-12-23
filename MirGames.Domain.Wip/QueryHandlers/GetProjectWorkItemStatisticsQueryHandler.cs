// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetProjectWorkItemStatisticsQueryHandler.cs">
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

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Returns statistics of the specified project.
    /// </summary>
    internal sealed class GetProjectWorkItemStatisticsQueryHandler :
        SingleItemQueryHandler<GetProjectWorkItemStatisticsQuery, ProjectWorkItemStatisticsViewModel>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectWorkItemStatisticsQueryHandler" /> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetProjectWorkItemStatisticsQueryHandler(IAuthorizationManager authorizationManager, IReadContextFactory readContextFactory)
        {
            Contract.Requires(authorizationManager != null);
            Contract.Requires(readContextFactory != null);

            this.authorizationManager = authorizationManager;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override ProjectWorkItemStatisticsViewModel Execute(
            GetProjectWorkItemStatisticsQuery query,
            ClaimsPrincipal principal)
        {
            Dictionary<WorkItemType, int> statistics;
            using (var readContext = this.readContextFactory.Create())
            {
                var project = readContext.Query<Project>().FirstOrDefault(t => t.Alias == query.ProjectAlias);

                if (project == null)
                {
                    throw new ItemNotFoundException("Project", query.ProjectAlias);
                }

                this.authorizationManager.EnsureAccess(principal, "ViewStatistics", "Project", project.ProjectId);

                statistics = readContext
                    .Query<ProjectWorkItem>()
                    .Where(
                        t =>
                        t.ProjectId == project.ProjectId && t.State != WorkItemState.Closed
                        && t.State != WorkItemState.Removed)
                    .GroupBy(t => t.ItemType, (key, value) => new { Type = key, Count = value.Count() })
                    .ToDictionary(t => t.Type, t => t.Count);
            }

            return new ProjectWorkItemStatisticsViewModel
            {
                OpenBugsCount = GetCountByType(statistics, WorkItemType.Bug),
                OpenFeaturesCount = GetCountByType(statistics, WorkItemType.Feature),
                OpenTasksCount = GetCountByType(statistics, WorkItemType.Task)
            };
        }

        /// <summary>
        /// Gets count of the items of the specified type.
        /// </summary>
        /// <param name="statistics">The statistics.</param>
        /// <param name="itemType">Type of the item.</param>
        /// <returns>Count of the items of the specified type.</returns>
        private static int GetCountByType(Dictionary<WorkItemType, int> statistics, WorkItemType itemType)
        {
            return statistics.ContainsKey(itemType) ? statistics[itemType] : 0;
        }
    }
}
