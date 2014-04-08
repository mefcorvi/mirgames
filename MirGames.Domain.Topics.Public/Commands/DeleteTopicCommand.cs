namespace MirGames.Domain.Topics.Commands
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The add new topic command.
    /// </summary>
    [Authorize(Roles = "User")]
    public class DeleteTopicCommand : Command
    {
        /// <summary>
        /// Gets or sets the topic id.
        /// </summary>
        public int TopicId { get; set; }
    }
}
