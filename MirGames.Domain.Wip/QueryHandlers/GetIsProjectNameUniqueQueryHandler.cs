namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns the WIP project.
    /// </summary>
    internal sealed class GetIsProjectNameUniqueQueryHandler : SingleItemQueryHandler<GetIsProjectNameUniqueQuery, bool>
    {
        /// <inheritdoc />
        public override bool Execute(IReadContext readContext, GetIsProjectNameUniqueQuery query, ClaimsPrincipal principal)
        {
            return !readContext
                .Query<Project>()
                .Any(p => p.Alias == query.Alias);
        }
    }
}
