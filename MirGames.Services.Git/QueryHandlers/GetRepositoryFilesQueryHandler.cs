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
    using MirGames.Services.Git.Extensions;
    using MirGames.Services.Git.Public.Exceptions;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;
    using MirGames.Services.Git.Services;

    internal sealed class GetRepositoryFilesQueryHandler : QueryHandler<GetRepositoryFilesQuery, GitRepositoryFileItemViewModel>
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
        /// Initializes a new instance of the <see cref="GetRepositoryFilesQueryHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        public GetRepositoryFilesQueryHandler(IRepositoryPathProvider repositoryPathProvider, IRepositorySecurity repositorySecurity)
        {
            Contract.Requires(repositoryPathProvider != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.repositorySecurity = repositorySecurity;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(
            IReadContext readContext,
            GetRepositoryFilesQuery query,
            ClaimsPrincipal principal)
        {
            var repository = GetRepository(readContext, query);
            var repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);

            return gitRepository.Index.Count;
        }

        /// <inheritdoc />
        protected override IEnumerable<GitRepositoryFileItemViewModel> Execute(
            IReadContext readContext,
            GetRepositoryFilesQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var repository = GetRepository(readContext, query);

            if (!this.repositorySecurity.CanRead(repository.Name))
            {
                throw new UnauthorizedAccessException(string.Format("User have no rights to read repository {0}", repository.Name));
            }

            var repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);

            if (gitRepository.Head == null || gitRepository.Head.Tip == null)
            {
                return Enumerable.Empty<GitRepositoryFileItemViewModel>();
            }

            var tree = gitRepository.Head.Tip.Tree;

            if (!string.IsNullOrEmpty(query.RelativePath))
            {
                var treeEntry = gitRepository.Head.Tip[query.RelativePath];

                if (treeEntry == null)
                {
                    throw new RepositoryPathNotFoundException(query.RelativePath);
                }

                if (treeEntry.TargetType == TreeEntryTargetType.Tree)
                {
                    tree = (Tree)treeEntry.Target;
                }
            }

            var entriesWithCommits = gitRepository.GetTreeCommits(tree);

            return entriesWithCommits.Select(
                indexEntry => new GitRepositoryFileItemViewModel
                {
                    Path = string.Format("{0}/{1}{2}", query.RelativePath.TrimEnd('/'), indexEntry.Key.Name, indexEntry.Key.TargetType == TreeEntryTargetType.Tree ? "/" : string.Empty),
                    Name = indexEntry.Key.Name,
                    ItemType = GetItemType(indexEntry.Key),
                    CommitId = indexEntry.Value.Sha,
                    Message = indexEntry.Value.MessageShort,
                    UpdatedDate = indexEntry.Value.Author.When.DateTime
                })
                .OrderByDescending(fileItem => fileItem.ItemType)
                .ThenBy(fileItem => fileItem.Name);
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The repository.</returns>
        private static Entities.Repository GetRepository(IReadContext readContext, GetRepositoryFilesQuery query)
        {
            var repository = readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == query.RepositoryId);

            if (repository == null)
            {
                throw new ItemNotFoundException("GitRepository", query.RepositoryId);
            }

            return repository;
        }

        /// <summary>
        /// Gets the type of the item.
        /// </summary>
        /// <param name="indexEntry">The index entry.</param>
        /// <returns>The type of the item.</returns>
        private static GitRepositoryFileItemType GetItemType(TreeEntry indexEntry)
        {
            switch (indexEntry.TargetType)
            {
                case TreeEntryTargetType.Blob:
                    return GitRepositoryFileItemType.File;
                case TreeEntryTargetType.Tree:
                    return GitRepositoryFileItemType.Directory;
                case TreeEntryTargetType.GitLink:
                    return GitRepositoryFileItemType.Link;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}