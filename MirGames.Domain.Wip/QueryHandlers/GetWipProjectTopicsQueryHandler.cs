// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectTopicsQueryHandler.cs">
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
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetWipProjectTopicsQueryHandler : QueryHandler<GetWipProjectTopicsQuery, TopicsListItem>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectTopicsQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetWipProjectTopicsQueryHandler(IQueryProcessor queryProcessor, IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(
            GetWipProjectTopicsQuery query,
            ClaimsPrincipal principal)
        {
            var project = this.GetProject(query);

            if (!project.BlogId.HasValue)
            {
                return 0;
            }

            return this.queryProcessor.GetItemsCount(new GetTopicsQuery
            {
                BlogId = project.BlogId,
                IsPublished = true,
                SearchString = query.SearchString
            });
        }

        /// <inheritdoc />
        protected override IEnumerable<TopicsListItem> Execute(
            GetWipProjectTopicsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var project = this.GetProject(query);

            if (!project.BlogId.HasValue)
            {
                return Enumerable.Empty<TopicsListItem>();
            }

            return this.queryProcessor.Process(
                new GetTopicsQuery
                {
                    BlogId = project.BlogId,
                    IsPublished = true,
                    SearchString = query.SearchString
                },
                pagination);
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The project.</returns>
        private Project GetProject(GetWipProjectTopicsQuery query)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var project = readContext.Query<Project>().SingleOrDefault(p => p.Alias == query.Alias);

                if (project == null)
                {
                    throw new ItemNotFoundException("Project", query.Alias);
                }

                return project;
            }
        }
    }
}
