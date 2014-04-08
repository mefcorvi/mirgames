namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of users.
    /// </summary>
    public sealed class GetAuthorQuery : SingleItemQuery<AuthorViewModel>
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        public int UserId { get; set; } 
    }
}