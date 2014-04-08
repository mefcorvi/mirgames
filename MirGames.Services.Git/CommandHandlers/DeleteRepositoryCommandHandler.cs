namespace MirGames.Services.Git.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Entities;
    using MirGames.Services.Git.Public.Commands;
    using MirGames.Services.Git.Services;

    internal sealed class DeleteRepositoryCommandHandler : CommandHandler<DeleteRepositoryCommand>
    {
        /// <summary>
        /// The repository path provider.
        /// </summary>
        private readonly IRepositoryPathProvider repositoryPathProvider;

        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The repository security
        /// </summary>
        private readonly IRepositorySecurity repositorySecurity;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteRepositoryCommandHandler" /> class.
        /// </summary>
        /// <param name="repositoryPathProvider">The repository path provider.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="repositorySecurity">The repository security.</param>
        public DeleteRepositoryCommandHandler(
            IRepositoryPathProvider repositoryPathProvider,
            IWriteContextFactory writeContextFactory,
            IRepositorySecurity repositorySecurity)
        {
            Contract.Requires(repositoryPathProvider != null);
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(repositorySecurity != null);

            this.repositoryPathProvider = repositoryPathProvider;
            this.writeContextFactory = writeContextFactory;
            this.repositorySecurity = repositorySecurity;
        }

        /// <inheritdoc />
        public override void Execute(DeleteRepositoryCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            string repositoryName = command.RepositoryName.ToLowerInvariant();

            if (!this.repositorySecurity.CanDelete(command.RepositoryName))
            {
                throw new SecurityException("Current user has insufficient rights to delete the repository");
            }

            using (var writeContext = this.writeContextFactory.Create())
            {
                var repository =
                    writeContext.Set<Repository>().FirstOrDefault(r => r.Name == repositoryName);

                if (repository == null)
                {
                    throw new ItemNotFoundException("Repository", repositoryName);
                }

                writeContext.Set<Repository>().Remove(repository);

                string path = this.repositoryPathProvider.GetPath(repositoryName);
                Directory.Delete(path, true);

                writeContext.SaveChanges();
            }
        }
    }
}
