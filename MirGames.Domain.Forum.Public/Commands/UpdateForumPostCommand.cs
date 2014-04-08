namespace MirGames.Domain.Forum.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Posts new reply in the topic.
    /// </summary>
    [Authorize(Roles = "TopicsAuthor")]
    [Api]
    public sealed class UpdateForumPostCommand : Command
    {
        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        [Required]
        public IEnumerable<int> Attachments { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the topics tags.
        /// </summary>
        public string TopicsTags { get; set; }

        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        [Required]
        public int PostId { get; set; }
    }
}