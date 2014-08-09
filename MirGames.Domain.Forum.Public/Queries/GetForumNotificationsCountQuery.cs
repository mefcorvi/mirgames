namespace MirGames.Domain.Forum.Queries
{
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns count of the forum notifications.
    /// </summary>
    public sealed class GetForumNotificationsCountQuery : SingleItemQuery<int>
    {
    }
}