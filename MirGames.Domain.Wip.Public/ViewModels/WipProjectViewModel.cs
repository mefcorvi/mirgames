namespace MirGames.Domain.Wip.ViewModels
{
    using System;
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;

    /// <summary>
    /// The WIP project view model.
    /// </summary>
    public sealed class WipProjectViewModel
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
        /// Gets or sets the author.
        /// </summary>
        public AuthorViewModel Author { get; set; }

        /// <summary>
        /// Gets or sets the logo URL.
        /// </summary>
        public string LogoUrl { get; set; }

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
        /// Gets or sets the tags.
        /// </summary>
        public IEnumerable<string> Tags { get; set; }

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
        /// Gets or sets the repository URL.
        /// </summary>
        public string RepositoryUrl { get; set; }
    }
}