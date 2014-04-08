namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns user's wall records.
    /// </summary>
    public sealed class GetUserWallRecordsQuery : Query<UserWallRecordViewModel>
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}