namespace MirGames.Domain.Users.Queries
{
    using System.Collections.Generic;

    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of users.
    /// </summary>
    public sealed class ResolveAuthorsQuery : SingleItemQuery<IEnumerable<AuthorViewModel>>
    {
        /// <summary>
        /// Gets or sets the user identifiers.
        /// </summary>
        public IEnumerable<AuthorViewModel> Authors { get; set; } 
    }
}