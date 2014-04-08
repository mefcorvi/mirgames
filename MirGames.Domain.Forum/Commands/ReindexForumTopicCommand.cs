namespace MirGames.Domain.Forum.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Re-indexes the forum topic.
    /// </summary>
    internal sealed class ReindexForumTopicCommand : Command
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }
    }
}