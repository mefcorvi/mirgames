namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Handles GetWallRecordByIdQuery query.
    /// </summary>
    internal sealed class GetWallRecordByIdQueryHandler : SingleItemQueryHandler<GetWallRecordByIdQuery, UserWallRecordViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWallRecordByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public GetWallRecordByIdQueryHandler(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override UserWallRecordViewModel Execute(IReadContext readContext, GetWallRecordByIdQuery query, ClaimsPrincipal principal)
        {
            var wallRecord = readContext
                .Query<WallRecord>()
                .Include(r => r.Author)
                .SingleOrDefault(r => r.Id == query.WallRecordId);

            if (wallRecord == null)
            {
                return null;
            }

            var wallRecordViewModel = new UserWallRecordViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = wallRecord.AuthorId,
                        },
                    DateAdd = wallRecord.DateAdd,
                    Text = wallRecord.Text
                };

            this.queryProcessor.Process(
                new ResolveAuthorsQuery
                    {
                        Authors = new[] { wallRecordViewModel.Author }
                    });

            return wallRecordViewModel;
        }
    }
}