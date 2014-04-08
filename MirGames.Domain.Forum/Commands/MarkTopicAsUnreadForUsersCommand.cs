namespace MirGames.Domain.Forum.Commands
{
    using System;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Marks the specified topic as unread for the all users.
    /// </summary>
    [Authorize(Roles = "User")]
    internal sealed class MarkTopicAsUnreadForUsersCommand : Command
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the topic date.
        /// </summary>
        public DateTime TopicDate { get; set; }
    }
}