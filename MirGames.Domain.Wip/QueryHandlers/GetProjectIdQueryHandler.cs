namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    internal sealed class GetProjectIdQueryHandler : SingleItemQueryHandler<GetProjectIdQuery, int>
    {
        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProjectIdQueryHandler"/> class.
        /// </summary>
        /// <param name="authorizationManager">The authorization manager.</param>
        public GetProjectIdQueryHandler(IAuthorizationManager authorizationManager)
        {
            Contract.Requires(authorizationManager != null);

            this.authorizationManager = authorizationManager;
        }

        public override int Execute(
            IReadContext readContext,
            GetProjectIdQuery query,
            ClaimsPrincipal principal)
        {
            var project = readContext.Query<Project>().FirstOrDefault(p => p.Alias == query.ProjectAlias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.ProjectAlias);
            }

            this.authorizationManager.EnsureAccess(principal, "Read", project);

            return project.ProjectId;
        }
    }
}