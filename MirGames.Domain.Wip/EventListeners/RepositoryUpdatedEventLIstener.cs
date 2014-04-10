namespace MirGames.Domain.Wip.EventListeners
{
    using System;
    using System.Linq;

    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Services.Git.Public.Events;

    internal sealed class RepositoryUpdatedEventLIstener : EventListenerBase<RepositoryUpdatedEvent>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryUpdatedEventLIstener"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public RepositoryUpdatedEventLIstener(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Process(RepositoryUpdatedEvent @event)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                var projects = writeContext
                    .Set<Project>()
                    .Where(p => p.RepositoryType == "git" && p.RepositoryId == @event.RepositoryId);

                projects.ForEach(p => p.UpdatedDate = DateTime.UtcNow);

                writeContext.SaveChanges();
            }
        }
    }
}
