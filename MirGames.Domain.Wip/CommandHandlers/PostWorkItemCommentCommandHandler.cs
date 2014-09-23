// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PostWorkItemCommentCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Wip.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Wip.Commands;
    using MirGames.Domain.Wip.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the post work item comment command.
    /// </summary>
    internal sealed class PostWorkItemCommentCommandHandler : CommandHandler<PostWorkItemCommentCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostWorkItemCommentCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public PostWorkItemCommentCommandHandler(IWriteContextFactory writeContextFactory, ICommandProcessor commandProcessor)
        {
            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        protected override int Execute(
            PostWorkItemCommentCommand command, 
            ClaimsPrincipal principal, 
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            int userId = principal.GetUserId().GetValueOrDefault();

            ProjectWorkItemComment comment;

            using (IWriteContext writeContext = this.writeContextFactory.Create())
            {
                ProjectWorkItem workItem = writeContext
                    .Set<ProjectWorkItem>()
                    .FirstOrDefault(w => w.WorkItemId == command.WorkItemId);

                if (workItem == null)
                {
                    throw new ItemNotFoundException("WorkItem", command.WorkItemId);
                }

                authorizationManager.EnsureAccess(principal, "CommentWorkItem", "Project", workItem.ProjectId);

                comment = new ProjectWorkItemComment
                {
                    Date = DateTime.UtcNow, 
                    Text = command.Text, 
                    UpdatedDate = DateTime.UtcNow, 
                    UserId = userId, 
                    UserIp = principal.GetHostAddress(), 
                    UserLogin = principal.GetUserLogin(), 
                    WorkItemId = command.WorkItemId
                };

                writeContext.Set<ProjectWorkItemComment>().Add(comment);
                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(new PublishAttachmentsCommand
            {
                EntityId = comment.CommentId,
                Identifiers = command.Attachments
            });

            return comment.CommentId;
        }
    }
}