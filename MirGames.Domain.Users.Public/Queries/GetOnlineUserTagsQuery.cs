namespace MirGames.Domain.Users.Queries
{
    using System.Collections.Generic;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns set of online user tags.
    /// </summary>
    public sealed class GetOnlineUserTagsQuery : SingleItemQuery<IDictionary<int, IEnumerable<string>>>
    {
    }
}