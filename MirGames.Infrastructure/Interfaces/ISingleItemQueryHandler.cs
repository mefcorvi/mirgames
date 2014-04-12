// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ISingleItemQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure
{
    using System.Security.Claims;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Abstraction of the query that returns only a one item.
    /// </summary>
    /// <typeparam name="TResult">Type of entity.</typeparam>
    public interface ISingleItemQueryHandler<TResult> : IQueryHandler<TResult>
    {
        /// <summary>
        /// Executes the current query.
        /// </summary>
        /// <param name="readContext">The read context.</param>
        /// <param name="query">The query.</param>
        /// <param name="principal">The principal.</param>
        /// <returns>The query result.</returns>
        TResult Execute(IReadContext readContext, SingleItemQuery<TResult> query, ClaimsPrincipal principal);
    }
}