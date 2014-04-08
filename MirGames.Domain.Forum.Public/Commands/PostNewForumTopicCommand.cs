namespace MirGames.Domain.Forum.Commands
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Posts new command.
    /// </summary>
    [Authorize(Roles = "User")]
    public sealed class PostNewForumTopicCommand : Command<int>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        public IEnumerable<int> Attachments { get; set; }
    }
}
