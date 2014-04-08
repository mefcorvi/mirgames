namespace MirGames.Domain.Wip.Entities
{
    using System;

    using MirGames.Infrastructure;

    /// <summary>
    /// The comment entity.
    /// </summary>
    internal sealed class Project
    {
        /// <summary>
        /// Gets or sets the comment id.
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the author unique identifier.
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Gets or sets the updated date.
        /// </summary>
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the votes count.
        /// </summary>
        public int VotesCount { get; set; }

        /// <summary>
        /// Gets or sets the votes.
        /// </summary>
        public int Votes { get; set; }

        /// <summary>
        /// Gets or sets the followers count.
        /// </summary>
        public int FollowersCount { get; set; }

        /// <summary>
        /// Gets or sets the tags list.
        /// </summary>
        public string TagsList { get; set; }

        /// <summary>
        /// Gets or sets the type of the repository.
        /// </summary>
        public string RepositoryType { get; set; }

        /// <summary>
        /// Gets or sets the repository identifier.
        /// </summary>
        public int? RepositoryId { get; set; }

        /// <summary>
        /// Gets or sets the blog identifier.
        /// </summary>
        public int? BlogId { get; set; }
    }
}
