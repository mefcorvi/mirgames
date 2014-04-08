namespace MirGames.Domain.Topics.Queries
{
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns tags that are should be shown on the main page.
    /// </summary>
    public sealed class GetMainTagsQuery : Query<string>
    {
    }
}