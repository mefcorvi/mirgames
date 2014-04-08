namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the list of user's claims.
    /// </summary>
    public sealed class GetCurrentUserClaimsQuery : Query<UserClaimViewModel>
    {
    }
}