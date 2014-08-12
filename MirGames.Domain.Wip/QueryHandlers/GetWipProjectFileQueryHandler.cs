// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWipProjectFileQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.QueryHandlers
{
    using System;
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

    internal sealed class GetWipProjectFileQueryHandler : SingleItemQueryHandler<GetWipProjectFileQuery, WipProjectFileViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectFileQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWipProjectFileQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override WipProjectFileViewModel Execute(
            IReadContext readContext,
            GetWipProjectFileQuery query,
            ClaimsPrincipal principal)
        {
            var project = GetProject(readContext, query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return null;
            }

            switch (project.RepositoryType)
            {
                case "git":
                    try
                    {
                        var gitFile = this.queryProcessor
                                          .Process(new GetRepositoryFileQuery
                                          {
                                              RepositoryId = project.RepositoryId.GetValueOrDefault(),
                                              FilePath = query.FilePath
                                          });

                        return new WipProjectFileViewModel
                        {
                            Content = gitFile.Content,
                            FileName = gitFile.FileName,
                            UpdatedDate = gitFile.UpdatedDate,
                            CommitId = gitFile.CommitId,
                            Message = gitFile.Message
                        };
                    }
                    catch (QueryProcessingFailedException e)
                    {
                        if (e.InnerException is RepositoryPathNotFoundException)
                        {
                            throw new ItemNotFoundException("WipProjectFile", query.FilePath);
                        }

                        throw e.InnerException;
                    }

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
        private static Project GetProject(IReadContext readContext, GetWipProjectFileQuery query)
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