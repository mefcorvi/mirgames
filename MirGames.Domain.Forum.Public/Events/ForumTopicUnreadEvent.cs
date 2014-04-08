namespace MirGames.Domain.Forum.Events
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Raised when some of topics has been unread.
    /// </summary>
    public class ForumTopicUnreadEvent : Event
    {
        /// <summary>
        /// Gets or sets the topic unique identifier.
        /// </summary>
        public int TopicId { get; set; }

        /// <summary>
        /// Gets or sets the user identifiers.
        /// </summary>
        public IEnumerable<int> UserIdentifiers { get; set; }

        /// <summary>
        /// Gets or sets the excluded users.
        /// </summary>
        public IEnumerable<int> ExcludedUsers { get; set; } 

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        protected override string EventType
        {
            get { return "Forum.ForumTopicUnread"; }
        }
    }
}