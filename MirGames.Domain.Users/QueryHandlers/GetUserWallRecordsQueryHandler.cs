namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles GetUserWallRecords query.
    /// </summary>
    internal sealed class GetUserWallRecordsQueryHandler : QueryHandler<GetUserWallRecordsQuery, UserWallRecordViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserWallRecordsQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetUserWallRecordsQueryHandler(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        protected override IEnumerable<UserWallRecordViewModel> Execute(IReadContext readContext, GetUserWallRecordsQuery query, ClaimsPrincipal principal, PaginationSettings pagination)
        {
            var set = readContext
                .Query<WallRecord>()
                .Include(r => r.Author)
                .Where(r => r.WallUserId == query.UserId)
                .OrderByDescending(r => r.DateAdd);

            var wallRecords =
                this.ApplyPagination(set, pagination)
                    .Select(
                        r => new UserWallRecordViewModel
                            {
                                Author = new AuthorViewModel
                                    {
                                        Id = r.AuthorId
                                    },
                                DateAdd = r.DateAdd,
                                Text = r.Text
                            })
                    .ToList();

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = wallRecords.Select(r => r.Author)
                    });

            return wallRecords;
        }

        /// <inheritdoc />
        protected override int GetItemsCount(IReadContext readContext, GetUserWallRecordsQuery query, ClaimsPrincipal principal)
        {
            return readContext.Query<WallRecord>().Count(r => r.WallUserId == query.UserId);
        }
    }
}