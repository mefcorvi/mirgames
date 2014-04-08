namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns tags that are should be shown on the main page.
    /// </summary>
    public sealed class GetForumTagsQuery : Query<string>
    {
    }
}