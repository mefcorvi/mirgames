namespace MirGames.Domain.Wip.Entities
{
    /// <summary>
    /// Link between project and tags.
    /// </summary>
    internal sealed class ProjectTag
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the project id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the tag text.
        /// </summary>
        public string TagText { get; set; }
    }
}
