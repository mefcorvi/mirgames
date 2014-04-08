namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns wall record by record identifier.
    /// </summary>
    public sealed class GetWallRecordByIdQuery : SingleItemQuery<UserWallRecordViewModel>
    {
        /// <summary>
        /// Gets or sets the wall record unique identifier.
        /// </summary>
        public int WallRecordId { get; set; }
    }
}