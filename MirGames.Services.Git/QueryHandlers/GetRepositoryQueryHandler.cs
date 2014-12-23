// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetRepositoryQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetRepositoryQueryHandler"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetRepositoryQueryHandler(IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override GitRepositoryViewModel Execute(GetRepositoryQuery query, ClaimsPrincipal principal)
        {
            Contract.Requires(query != null);

            using (var readContext = this.readContextFactory.Create())
            {
                var repository =
                    readContext.Query<Entities.Repository>().SingleOrDefault(r => r.Id == query.RepositoryId);

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
}