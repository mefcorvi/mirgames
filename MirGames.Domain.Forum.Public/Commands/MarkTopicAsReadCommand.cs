namespace MirGames.Domain.Forum.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Marks the specified topic as read.
    /// </summary>
    [Authorize(Roles = "User")]
    public sealed class MarkTopicAsReadCommand : Command
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }
    }
}