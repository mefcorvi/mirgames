namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns claims of the current user.
    /// </summary>
    internal sealed class GetCurrentUserClaimsQueryHandler : QueryHandler<GetCurrentUserClaimsQuery, UserClaimViewModel>
    {
        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetCurrentUserClaimsQuery query, ClaimsPrincipal principal)
        {
            return principal.Claims.Count();
        }

        /// <inheritdoc />
        protected override IEnumerable<UserClaimViewModel> Execute(IReadContext readContext, GetCurrentUserClaimsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            return this.ApplyPagination(principal.Claims.OrderBy(c => c.Type), pagination).Select(
                c => new UserClaimViewModel
                    {
                        Type = c.Type,
                        Value = c.Value
                    });
        }
    }
}