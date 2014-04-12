// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GetCommentByIdQueryHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The single item query handler.
    /// </summary>
    internal sealed class GetCommentByIdQueryHandler : SingleItemQueryHandler<GetCommentByIdQuery, CommentViewModel>
    {
        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// The text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetCommentByIdQueryHandler" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        /// <param name="textTransform">The text transform.</param>
        public GetCommentByIdQueryHandler(IQueryProcessor queryProcessor, IAuthorizationManager authorizationManager, ITextTransform textTransform)
        {
            this.queryProcessor = queryProcessor;
            this.authorizationManager = authorizationManager;
            this.textTransform = textTransform;
        }

        /// <inheritdoc />
        public override CommentViewModel Execute(IReadContext readContext, GetCommentByIdQuery query, ClaimsPrincipal principal)
        {
            var comment = readContext
                .Query<Comment>()
                .Include(c => c.Topic)
                .FirstOrDefault(c => c.CommentId == query.CommentId);

            if (comment == null)
            {
                return null;
            }

            var commentViewModel = new CommentViewModel
                {
                    Author = new AuthorViewModel
                        {
                            Id = comment.UserId,
                            Login = comment.UserLogin
                        },
                    CreationDate = comment.Date,
                    UpdatedDate = comment.UpdatedDate,
                    Text = comment.Text ?? this.textTransform.Transform(comment.SourceText),
                    Id = comment.CommentId,
                    TopicId = comment.TopicId,
                    TopicTitle = comment.Topic.TopicTitle
                };

            this.queryProcessor.Process(new ResolveAuthorsQuery { Authors = new[] { commentViewModel.Author } });

            commentViewModel.CanBeEdited = this.authorizationManager.CheckAccess(principal, "Edit", comment);
            commentViewModel.CanBeDeleted = this.authorizationManager.CheckAccess(principal, "Delete", comment);

            return commentViewModel;
        }
    }
}