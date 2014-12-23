// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetCommentForEditQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    internal sealed class GetCommentForEditQueryHandler : SingleItemQueryHandler<GetCommentForEditQuery, CommentForEditViewModel>
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCommentForEditQueryHandler"/> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetCommentForEditQueryHandler(IReadContextFactory readContextFactory)
        {
            Contract.Requires(readContextFactory != null);
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override CommentForEditViewModel Execute(GetCommentForEditQuery query, ClaimsPrincipal principal)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                return readContext
                    .Query<Comment>()
                    .Where(c => c.CommentId == query.CommentId)
                    .Select(c => new CommentForEditViewModel
                    {
                        Id = c.CommentId,
                        SourceText = c.SourceText
                    })
                    .FirstOrDefault();
            }
        }
    }
}