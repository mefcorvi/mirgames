namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns count of unread topics.
    /// </summary>
    public sealed class GetForumTopicsUnreadCountQuery : SingleItemQuery<int>
    {
    }
}