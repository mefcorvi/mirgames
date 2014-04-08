namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of online users.
    /// </summary>
    public sealed class GetOnlineUsersQuery : Query<OnlineUserViewModel>
    {
    }
}