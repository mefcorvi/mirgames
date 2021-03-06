// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetRepositoryFileQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.QueryHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using LibGit2Sharp;

    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Extensions;
    using MirGames.Services.Git.Public.Exceptions;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;
    using MirGames.Services.Git.Services;

    internal sealed class GetRepositoryFileQueryHandler : SingleItemQueryHandler<GetRepositoryFileQuery, GitRepositoryFileViewModel>
    {
        /// <summary>
        /// The repository path provider.
        /// </summary>
        private readonly IRepositoryPathProvider repositoryPathProvider;

        /// <summary>
        /// The repository security.
        /// </summary>
        private readonly IRepositorySecurity repositorySecurity;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRepositoryFileQueryHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetRepositoryFileQueryHandler(
            IRepositoryPathProvider repositoryPathProvider,
            IRepositorySecurity repositorySecurity,
            IReadContextFactory readContextFactory)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(repositorySecurity != null);
            Contract.Requires(readContextFactory != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.repositorySecurity = repositorySecurity;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override GitRepositoryFileViewModel Execute(GetRepositoryFileQuery query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(query.FilePath));
            
            var repository = this.GetRepository(query);

            if (!this.repositorySecurity.CanRead(repository.Name))
            {
                throw new UnauthorizedAccessException(string.Format("User have no rights to read repository {0}", repository.Name));
            }

            var repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);

            if (gitRepository.Head == null || gitRepository.Head.Tip == null)
            {
                throw new RepositoryPathNotFoundException(query.FilePath);                
            }

            var treeEntry = gitRepository.Head.Tip[query.FilePath];

            if (treeEntry == null)
            {
                throw new RepositoryPathNotFoundException(query.FilePath);
            }

            if (treeEntry.TargetType == TreeEntryTargetType.Blob)
            {
                var blob = (Blob)treeEntry.Target;
                var commit = gitRepository.GetCommit(treeEntry);

                return new GitRepositoryFileViewModel
                {
                    FileName = treeEntry.Name,
                    Content = blob.GetContentStream(),
                    CommitId = commit.Sha,
                    Message = commit.MessageShort,
                    UpdatedDate = commit.Author.When.UtcDateTime
                };
            }

            return null;
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The repository.</returns>
        private Entities.Repository GetRepository(GetRepositoryFileQuery query)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var repository =
                    readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == query.RepositoryId);

                if (repository == null)
                {
                    throw new ItemNotFoundException("GitRepository", query.RepositoryId);
                }

                return repository;
            }
        }
    }
}