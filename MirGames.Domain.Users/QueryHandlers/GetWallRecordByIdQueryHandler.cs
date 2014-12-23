// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetWallRecordByIdQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.QueryHandlers
{
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
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
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWallRecordByIdQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetWallRecordByIdQueryHandler(IQueryProcessor queryProcessor, IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override UserWallRecordViewModel Execute(GetWallRecordByIdQuery query, ClaimsPrincipal principal)
        {
            WallRecord wallRecord;
            using (var readContext = this.readContextFactory.Create())
            {
                wallRecord = readContext
                    .Query<WallRecord>()
                    .Include(r => r.Author)
                    .SingleOrDefault(r => r.Id == query.WallRecordId);
            }

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