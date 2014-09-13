// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetProjectIdQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Wip.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
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

        protected override int Execute(
            IReadContext readContext,
            GetProjectIdQuery query,
            ClaimsPrincipal principal)
        {
            var project = readContext.Query<Project>().FirstOrDefault(p => p.Alias == query.ProjectAlias);

            if (project == null)
            {
                throw new ItemNotFoundException("Project", query.ProjectAlias);
            }

            this.authorizationManager.EnsureAccess(principal, "Read", "Project", project.ProjectId);

            return project.ProjectId;
        }
    }
}