namespace MirGames.Domain.Users.Queries
{
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of available OAuth providers.
    /// </summary>
    [Api]
    public sealed class GetOAuthProvidersQuery : Query<OAuthProviderViewModel>
    {
    }
}