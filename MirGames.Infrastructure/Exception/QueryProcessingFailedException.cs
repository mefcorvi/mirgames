// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="QueryProcessingFailedException.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Exception
{
    using System;

    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Exception raised when query processing is failed.
    /// </summary>
    public class QueryProcessingFailedException : MirGamesException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessingFailedException"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        public QueryProcessingFailedException(Query query) : this(query, null)
        {
            this.Query = query;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryProcessingFailedException"/> class.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="innerException">The inner exception.</param>
        public QueryProcessingFailedException(Query query, Exception innerException)
            : base("Processing of query was failed. Type of query: " + query.GetType().Name, innerException)
        {
        }

        /// <summary>
        /// Gets the query.
        /// </summary>
        public Query Query { get; private set; }
    }
}