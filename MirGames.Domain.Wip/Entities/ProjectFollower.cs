namespace MirGames.Domain.Wip.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The comment entity.
    /// </summary>
    internal sealed class ProjectFollower
    {
        /// <summary>
        /// Gets or sets the project follower unique identifier.
        /// </summary>
        public int ProjectFollowerId { get; set; }

        /// <summary>
        /// Gets or sets the project unique identifier.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the follower unique identifier.
        /// </summary>
        public int FollowerId { get; set; }

        /// <summary>
        /// Gets or sets the following date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime FollowingDate { get; set; }
    }
}
