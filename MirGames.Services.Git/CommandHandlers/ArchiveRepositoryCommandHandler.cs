namespace MirGames.Services.Git.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using LibGit2Sharp;

    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Public.Commands;
    using MirGames.Services.Git.Services;

    /// <summary>
    /// Archives the repository.
    /// </summary>
    internal sealed class ArchiveRepositoryCommandHandler : CommandHandler<ArchiveRepositoryCommand>
    {
        /// <summary>
        /// The repository path provider.
        /// </summary>
        private readonly IRepositoryPathProvider repositoryPathProvider;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The repository security.
        /// </summary>
        private readonly IRepositorySecurity repositorySecurity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveRepositoryCommandHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="readContextFactory">The write context factory.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        public ArchiveRepositoryCommandHandler(
            IRepositoryPathProvider repositoryPathProvider,
            IReadContextFactory readContextFactory,
            IRepositorySecurity repositorySecurity)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(readContextFactory != null);
            Contract.Requires(repositorySecurity != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.readContextFactory = readContextFactory;
            this.repositorySecurity = repositorySecurity;
        }

        /// <inheritdoc />
        public override void Execute(ArchiveRepositoryCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command.OutputStream != null);
            Contract.Requires(command.OutputStream.CanWrite);

            Entities.Repository repository;

            using (var readContext = this.readContextFactory.Create())
            {
                repository = GetRepository(readContext, command);
            }

            if (!this.repositorySecurity.CanRead(repository.Name))
            {
                throw new UnauthorizedAccessException(string.Format("User have no rights to read repository {0}", repository.Name));
            }

            string repositoryPath = this.repositoryPathProvider.GetPath(repository.Name);
            var gitRepository = new Repository(repositoryPath);

            using (var archiver = new ZipArchiver(command.OutputStream))
            {
                gitRepository.ObjectDatabase.Archive(gitRepository.Head.Tip.Tree, archiver);
            }
        }

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="command">The command.</param>
        /// <returns>The repository.</returns>
        private static Entities.Repository GetRepository(IReadContext readContext, ArchiveRepositoryCommand command)
        {
            var repository = readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == command.RepositoryId);

            if (repository == null)
            {
                throw new ItemNotFoundException("GitRepository", command.RepositoryId);
            }

            return repository;
        }
    }
}