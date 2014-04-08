namespace MirGames.Services.Git.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using LibGit2Sharp;

    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;
    using MirGames.Services.Git.Services;

    internal sealed class GetRepositoryHistoryQueryHandler : QueryHandler<GetRepositoryHistoryQuery, GitRepositoryHistoryItemViewModel>
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
        /// Initializes a new instance of the <see cref="GetRepositoryHistoryQueryHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        public GetRepositoryHistoryQueryHandler(IRepositoryPathProvider repositoryPathProvider, IRepositorySecurity repositorySecurity)
        {
            Contract.Requires(repositoryPathProvider != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.repositorySecurity = repositorySecurity;
        }

        /// <inheritdoc />
        protected override IEnumerable<GitRepositoryHistoryItemViewModel> Execute(IReadContext readContext, GetRepositoryHistoryQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var repository = GetRepository(readContext, query);

            if (!this.repositorySecurity.CanRead(repository.Name))
            {
                throw new UnauthorizedAccessException(string.Format("User have no rights to read repository {0}", repository.Name));
            }

            string repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);
            
            return gitRepository.Commits.Select(
                    commit =>
                    new GitRepositoryHistoryItemViewModel
                    {
                        Author = commit.Author.Name,
                        Message = commit.Message.Trim(),
                        Date = commit.Author.When.DateTime,
                    });
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetRepositoryHistoryQuery query, ClaimsPrincipal principal)
        {
            var repository = GetRepository(readContext, query);

            string repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);
            
            return gitRepository.Commits.Count();
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The repository.</returns>
        private static Entities.Repository GetRepository(IReadContext readContext, GetRepositoryHistoryQuery query)
        {
            var repository = readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == query.RepositoryId);

            if (repository == null)
            {
                throw new ItemNotFoundException("GitRepository", query.RepositoryId);
            }

            return repository;
        }
    }
}