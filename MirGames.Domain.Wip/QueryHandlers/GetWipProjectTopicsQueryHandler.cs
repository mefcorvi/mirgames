namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetWipProjectTopicsQueryHandler : QueryHandler<GetWipProjectTopicsQuery, TopicsListItem>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWipProjectTopicsQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWipProjectTopicsQueryHandler(IQueryProcessor queryProcessor)
        {
            Contract.Requires(queryProcessor != null);

            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(
            IReadContext readContext,
            GetWipProjectTopicsQuery query,
            ClaimsPrincipal principal)
        {
            var project = GetProject(readContext, query);

            if (!project.BlogId.HasValue)
            {
                return 0;
            }

            return this.queryProcessor.GetItemsCount(new GetTopicsQuery
            {
                BlogId = project.BlogId,
                IsPublished = true,
                SearchString = query.SearchString
            });
        }

        /// <inheritdoc />
        protected override IEnumerable<TopicsListItem> Execute(
            IReadContext readContext,
            GetWipProjectTopicsQuery query,
            ClaimsPrincipal principal,
            PaginationSettings pagination)
        {
            var project = GetProject(readContext, query);

            if (!project.BlogId.HasValue)
            {
                return Enumerable.Empty<TopicsListItem>();
            }

            return this.queryProcessor.Process(
                new GetTopicsQuery
                {
                    BlogId = project.BlogId,
                    IsPublished = true,
                    SearchString = query.SearchString
                },
                pagination);
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <returns>The project.</returns>
        private static Project GetProject(IReadContext readContext, GetWipProjectTopicsQuery query)
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
