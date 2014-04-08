namespace MirGames.Domain.Wip.QueryHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Services.Git.Public.Queries;

    internal sealed class GetWipProjectFileQueryHandler : SingleItemQueryHandler<GetWipProjectFileQuery, WipProjectFileViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectFileQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWipProjectFileQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override WipProjectFileViewModel Execute(
            IReadContext readContext,
            GetWipProjectFileQuery query,
            ClaimsPrincipal principal)
        {
            var project = GetProject(readContext, query);

            if (!project.RepositoryId.HasValue || string.IsNullOrEmpty(project.RepositoryType))
            {
                return null;
            }

            switch (project.RepositoryType)
            {
                case "git":
                    var gitFile = this.queryProcessor
                               .Process(new GetRepositoryFileQuery
                               {
                                   RepositoryId = project.RepositoryId.GetValueOrDefault(),
                                   FilePath = query.FilePath
                               });

                    return new WipProjectFileViewModel
                    {
                        Content = gitFile.Content,
                        FileName = gitFile.FileName,
                        UpdatedDate = gitFile.UpdatedDate,
                    };

                default:
                    throw new IndexOutOfRangeException(string.Format("{0} is not supported type of repositories.", project.RepositoryType));
            }
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The project.</returns>
        private static Project GetProject(IReadContext readContext, GetWipProjectFileQuery query)
        {
            var project = readContext.Query<Project>().SingleOrDefault(p => p.Alias == query.Alias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.Alias);
            }

            return project;
        }
    }
}