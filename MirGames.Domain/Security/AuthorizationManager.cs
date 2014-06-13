// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AuthorizationManager.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Security
{
    using MirGames.Domain.Acl.Public.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Provides direct access methods for evaluating authorization policy
    /// </summary>
    internal sealed class AuthorizationManager : IAuthorizationManager
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationManager"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        public AuthorizationManager(IQueryProcessor queryProcessor)
        {
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public bool CheckAccess(int userId, string action, string entityType, int? entityId)
        {
            return this.queryProcessor.Process(new IsActionAllowedQuery
            {
                ActionName = action,
                EntityId = entityId,
                EntityType = entityType,
                UserId = userId
            });
        }
    }
}