namespace MirGames.Domain.Wip.Entities
{
    using System;

    public sealed class ProjectWorkItemComment
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
        /// Gets or sets the user identifier.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user login.
        /// </summary>
        public string UserLogin { get; set; }

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
        /// Gets or sets the user IP.
        /// </summary>
        public string UserIp { get; set; }
    }
}