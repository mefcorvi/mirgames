namespace MirGames.Services.Git.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Queries;
    using MirGames.Services.Git.Public.ViewModels;

    internal sealed class GetRepositoryQueryHandler : SingleItemQueryHandler<GetRepositoryQuery, GitRepositoryViewModel>
    {
        /// <inheritdoc />
        public override GitRepositoryViewModel Execute(IReadContext readContext, GetRepositoryQuery query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);

            var repository = readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == query.RepositoryId);

            if (repository == null)
            {
                throw new ItemNotFoundException("GitRepository", query.RepositoryId);
            }

            return new GitRepositoryViewModel
            {
                RepositoryName = repository.Name
            };
        }
    }
}