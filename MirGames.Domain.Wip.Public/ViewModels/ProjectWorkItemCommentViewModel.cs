namespace MirGames.Domain.Wip.ViewModels
{
    using System;

    using MirGames.Domain.Users.ViewModels;

    public sealed class ProjectWorkItemCommentViewModel
    {
        /// <summary>
        /// Gets or sets the comment identifier.
        /// </summary>
        public int CommentId { get; set; }

        /// <summary>
        /// Gets or sets the work item identifier.
        /// </summary>
        public int WorkItemId { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be edited.
        /// </summary>
        public bool CanBeEdited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance can be deleted.
        /// </summary>
        public bool CanBeDeleted { get; set; }
    }
}