namespace MirGames.Domain.Wip.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Public.Commands;

    internal sealed class ArchiveProjectRepositoryCommandHandler : CommandHandler<ArchiveProjectRepositoryCommand>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArchiveProjectRepositoryCommandHandler" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public ArchiveProjectRepositoryCommandHandler(ICommandProcessor commandProcessor, IReadContextFactory readContextFactory)
        {
            Contract.Requires(commandProcessor != null);
            Contract.Requires(readContextFactory != null);

            this.commandProcessor = commandProcessor;
            this.readContextFactory = readContextFactory;
        }

        public override void Execute(
            ArchiveProjectRepositoryCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(command.OutputStream != null);
            Contract.Requires(command.OutputStream.CanWrite);

            Project project;

            using (var readContext = this.readContextFactory.Create())
            {
                project = GetProject(readContext, command);
            }

            if (project.RepositoryId.HasValue && project.RepositoryType.EqualsIgnoreCase("git"))
            {
                this.ArchiveGitRepository(command.OutputStream, project.RepositoryId.Value);
            }
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="command">The command.</param>
        /// <returns>The project.</returns>
        private static Project GetProject(IReadContext readContext, ArchiveProjectRepositoryCommand command)
        {
            var project = readContext.Query<Project>().SingleOrDefault(p => p.Alias == command.ProjectAlias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", command.ProjectAlias);
            }

            return project;
        }

        /// <summary>
        /// Archives the git repository.
        /// </summary>
        /// <param name="outputStream">The output stream.</param>
        /// <param name="repositoryId">The repository identifier.</param>
        private void ArchiveGitRepository(Stream outputStream, int repositoryId)
        {
            this.commandProcessor.Execute(new ArchiveRepositoryCommand
            {
                OutputStream = outputStream,
                RepositoryId = repositoryId
            });
        }
    }
}
