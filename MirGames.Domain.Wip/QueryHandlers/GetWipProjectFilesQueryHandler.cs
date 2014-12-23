// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectFilesQueryHandler.cs">
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

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Exception;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Exceptions;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;

    internal sealed class GetWipProjectFilesQueryHandler : QueryHandler<GetWipProjectFilesQuery, WipProjectRepositoryItemViewModel>
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
        /// Initializes a new instance of the <see cref="GetWipProjectFilesQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetWipProjectFilesQueryHandler(IQueryProcessor queryProcessor, IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override IEnumerable<WipProjectRepositoryItemViewModel> Execute(
            GetWipProjectFilesQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var project = this.GetProject(query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return Enumerable.Empty<WipProjectRepositoryItemViewModel>();
            }

            switch (project.RepositoryType)
            {
                case "git":
                    try
                    {
                        return this.queryProcessor
                                   .Process(new GetRepositoryFilesQuery
                                   {
                                       RepositoryId = project.RepositoryId.GetValueOrDefault(),
                                       RelativePath = query.RelativePath
                                   })
                                   .Select(h => new WipProjectRepositoryItemViewModel
                                   {
                                       Name = h.Name,
                                       Path = h.Path,
                                       ItemType = GetGitItemType(h.ItemType),
                                       CommitId = h.CommitId,
                                       Message = h.Message,
                                       UpdatedDate = h.UpdatedDate
                                   });
                    }
                    catch (QueryProcessingFailedException e)
                    {
                        if (e.InnerException is RepositoryPathNotFoundException)
                        {
                            throw new ItemNotFoundException("Path", query.RelativePath, e);
                        }

                        throw;
                    }
                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        /// <inheritdoc />
        protected override int GetItemsCount(
            GetWipProjectFilesQuery query,
            ClaimsPrincipal principal)
        {
            var project = this.GetProject(query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return 0;
            }

            switch (project.RepositoryType)
            {
                case "git":
                    return this.queryProcessor
                               .GetItemsCount(new GetRepositoryFilesQuery
                               {
                                   RepositoryId = project.RepositoryId.GetValueOrDefault(),
                                   RelativePath = query.RelativePath
                               });
                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        private static WipProjectRepositoryItemType GetGitItemType(GitRepositoryFileItemType itemType)
        {
            switch (itemType)
            {
                case GitRepositoryFileItemType.File:
                    return WipProjectRepositoryItemType.File;
                case GitRepositoryFileItemType.Directory:
                    return WipProjectRepositoryItemType.Directory;
                case GitRepositoryFileItemType.Link:
                    return WipProjectRepositoryItemType.Other;
                default:
                    throw new ArgumentOutOfRangeException("itemType");
            }
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The project.</returns>
        private Project GetProject(GetWipProjectFilesQuery query)
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