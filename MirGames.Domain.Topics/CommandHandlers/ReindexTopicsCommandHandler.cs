namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Data.Entity;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handler of re-index topics command.
    /// </summary>
    public class ReindexTopicsCommandHandler : CommandHandler<ReindexTopicsCommand>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The write context factory
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexTopicsCommandHandler" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="writeContextFactory">The write context factory.</param>
        public ReindexTopicsCommandHandler(ISearchEngine searchEngine, IWriteContextFactory writeContextFactory)
        {
            this.searchEngine = searchEngine;
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override void Execute(ReindexTopicsCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            this.searchEngine.ClearIndex();
            using (var writeContext = this.writeContextFactory.Create())
            {
                var topics = writeContext.Set<Topic>().Include(t => t.Content);
                
                foreach (var topic in topics)
                {
                    this.searchEngine.Index(topic.Id, "Topic", topic.TopicTitle + " " + topic.Content.TopicText + " " + topic.TagsList);
                }
            }
        }
    }
}