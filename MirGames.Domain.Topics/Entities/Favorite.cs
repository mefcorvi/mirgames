namespace MirGames.Domain.Topics.Entities
{
    /// <summary>
    /// Favorite entity.
    /// </summary>
    internal sealed class Favorite
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets the target id.
        /// </summary>
        public int? TargetId { get; set; }

        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        public string TargetType { get; set; }

        /// <summary>
        /// Gets or sets the target publish.
        /// </summary>
        public bool? TargetPublish { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        public string Tags { get; set; }
    }
}
