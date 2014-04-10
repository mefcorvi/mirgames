namespace MirGames.Services.Git.Public.Events
{
    using MirGames.Infrastructure.Events;

    public class RepositoryUpdatedEvent : Event
    {
        /// <summary>
        /// Gets or sets the repository identifier.
        /// </summary>
        public int RepositoryId { get; set; }

        /// <inheritodc />
        protected override string EventType
        {
            get { return "Git.RepositoryUpdated"; }
        }
    }
}