// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectCommitsQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
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

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Queries;

    internal sealed class GetWipProjectCommitsQueryHandler : QueryHandler<GetWipProjectCommitsQuery, WipProjectCommitViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectCommitsQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWipProjectCommitsQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override IEnumerable<WipProjectCommitViewModel> Execute(
            IReadContext readContext,
            GetWipProjectCommitsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var project = GetProject(readContext, query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return Enumerable.Empty<WipProjectCommitViewModel>();
            }

            switch (project.RepositoryType)
            {
                case "git":
                    return this.queryProcessor
                               .Process(new GetRepositoryHistoryQuery
                               {
                                   RepositoryId = project.RepositoryId.GetValueOrDefault()
                               })
                               .Select(h => new WipProjectCommitViewModel
                               {
                                   Date = h.Date,
                                   Message = h.Message,
                                   Author = new AuthorViewModel
                                   {
                                       Login = h.Author
                                   }
                               });
                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        /// <inheritdoc />
        protected override int GetItemsCount(
            IReadContext readContext,
            GetWipProjectCommitsQuery query,
            ClaimsPrincipal principal)
        {
            var project = GetProject(readContext, query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return 0;
            }

            switch (project.RepositoryType)
            {
                case "git":
                    return this.queryProcessor
                               .GetItemsCount(new GetRepositoryHistoryQuery
                               {
                                   RepositoryId = project.RepositoryId.GetValueOrDefault()
                               });
                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The project.</returns>
        private static Project GetProject(IReadContext readContext, GetWipProjectCommitsQuery query)
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