// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetProjectWorkItemCommentQueryHandler.cs">
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
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Domain.Wip.Queries;
    using MirGames.Domain.Wip.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    internal sealed class GetProjectWorkItemCommentQueryHandler : SingleItemQueryHandler<GetProjectWorkItemCommentQuery, ProjectWorkItemCommentViewModel>
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
        /// Initializes a new instance of the <see cref="GetProjectWorkItemCommentQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="readContextFactory">The read context factory.</param>
        public GetProjectWorkItemCommentQueryHandler(IQueryProcessor queryProcessor, IReadContextFactory readContextFactory)
        {
            Contract.Requires(queryProcessor != null);
            Contract.Requires(readContextFactory != null);

            this.queryProcessor = queryProcessor;
            this.readContextFactory = readContextFactory;
        }

        /// <inheritdoc />
        protected override ProjectWorkItemCommentViewModel Execute(
            GetProjectWorkItemCommentQuery query,
            ClaimsPrincipal principal)
        {
            ProjectWorkItemComment comment;
            using (var readContext = this.readContextFactory.Create())
            {
                comment = readContext
                    .Query<ProjectWorkItemComment>()
                    .FirstOrDefault(p => p.CommentId == query.CommentId);
            }

            if (comment == null)
            {
                throw new ItemNotFoundException("Comment", query.CommentId);
            }

            var commentViewModel = new ProjectWorkItemCommentViewModel
                {
                    CommentId = comment.CommentId,
                    Date = comment.Date,
                    Text = comment.Text,
                    UpdatedDate = comment.UpdatedDate,
                    Author = new AuthorViewModel
                    {
                        Id = comment.UserId,
                        Login = comment.UserLogin
                    },
                    WorkItemId = comment.WorkItemId
                };

            this.queryProcessor.Process(new ResolveAuthorsQuery
            {
                Authors = new[] { commentViewModel.Author }
            });

            return commentViewModel;
        }
    }
}