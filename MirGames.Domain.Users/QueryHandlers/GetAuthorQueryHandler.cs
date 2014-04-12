// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetAuthorQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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