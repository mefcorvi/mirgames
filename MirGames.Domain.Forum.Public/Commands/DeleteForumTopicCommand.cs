namespace MirGames.Domain.Forum.Commands
{
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Posts new reply in the topic.
    /// </summary>
    [Authorize(Roles = "TopicsAuthor")]
    [Api]
    public sealed class DeleteForumTopicCommand : Command
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        [Required]
        public int TopicId { get; set; }
    }
}