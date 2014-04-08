namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns an user by principal data.
    /// </summary>
    public sealed class GetCurrentUserQuery : SingleItemQuery<CurrentUserViewModel>
    {
    }
}