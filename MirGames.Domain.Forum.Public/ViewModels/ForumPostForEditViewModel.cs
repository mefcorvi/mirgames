namespace MirGames.Domain.Forum.ViewModels
{
    /// <summary>
    /// The forum posts list item.
    /// </summary>
    public class ForumPostForEditViewModel
    {
        /// <summary>
        /// Gets or sets the post unique identifier.
        /// </summary>
        public int PostId { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string SourceText { get; set; }

        /// <summary>
        /// Gets or sets the topic title.
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// Gets or sets the topic tags.
        /// </summary>
        public string TopicTags { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can change title.
        /// </summary>
        public bool CanChangeTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user can change tags.
        /// </summary>
        public bool CanChangeTags { get; set; }
    }
}