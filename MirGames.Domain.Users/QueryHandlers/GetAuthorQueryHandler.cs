namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles the GetUsersQuery.
    /// </summary>
    internal sealed class GetAuthorQueryHandler : SingleItemQueryHandler<GetAuthorQuery, AuthorViewModel>
    {
        /// <summary>
        /// The query handler.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthorQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query handler.</param>
        public GetAuthorQueryHandler(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override AuthorViewModel Execute(IReadContext readContext, GetAuthorQuery query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);

            return this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = new[] { new AuthorViewModel { Id = query.UserId } }
                    }).SingleOrDefault();
        }
    }
}