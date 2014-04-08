namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Security.Claims;
    using System.Text;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handler of re-index forum topics command.
    /// </summary>
    internal sealed class ReindexForumTopicCommandHandler : CommandHandler<ReindexForumTopicCommand>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReindexForumTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public ReindexForumTopicCommandHandler(ISearchEngine searchEngine, IQueryProcessor queryProcessor)
        {
            this.searchEngine = searchEngine;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Execute(ReindexForumTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            var posts = this.queryProcessor.Process(new GetForumTopicPostsQuery { TopicId = command.TopicId });
            var sb = new StringBuilder();

            foreach (var post in posts)
            {
                sb.AppendLine(post.Text);
            }

            this.searchEngine.Remove(command.TopicId, "ForumTopic");
            this.searchEngine.Index(command.TopicId, "ForumTopic", sb.ToString());
        }
    }
}