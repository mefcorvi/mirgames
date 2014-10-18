// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PostNewCommentCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the post new comment command.
    /// </summary>
    internal sealed class PostNewCommentCommandHandler : CommandHandler<PostNewCommentCommand, int>
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
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostNewCommentCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textProcessor">The text processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public PostNewCommentCommandHandler(
            IWriteContextFactory writeContextFactory,
            ICommandProcessor commandProcessor,
            ITextProcessor textProcessor,
            IEventBus eventBus)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(textProcessor != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
            this.textProcessor = textProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override int Execute(PostNewCommentCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            var comment = new Comment
            {
                UserId = principal.GetUserId().GetValueOrDefault(),
                UserLogin = principal.GetUserLogin(),
                UserIP = principal.GetHostAddress(),
                Text = this.textProcessor.GetHtml(command.Text),
                SourceText = command.Text,
                Rating = 0,
                Date = DateTime.UtcNow
            };

            Topic topic;

            using (var writeContext = this.writeContextFactory.Create())
            {
                topic = writeContext.Set<Topic>().SingleOrDefault(t => t.Id == command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                authorizationManager.EnsureAccess(principal, "Comment", "Topic", topic.Id);
                comment.TopicId = topic.Id;

                topic.CountComment = writeContext.Set<Comment>().Count(c => c.TopicId == topic.Id) + 1;
                writeContext.Set<Comment>().Add(comment);

                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = comment.CommentId,
                            Identifiers = command.Attachments
                        });
            }

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "Edit", "Delete" },
                EntityId = comment.CommentId,
                EntityType = "Comment",
                UserId = principal.GetUserId(),
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            });

            this.eventBus.Raise(new CommentCreatedEvent
            {
                BlogId = topic.BlogId,
                TopicId = comment.TopicId,
                CommentId = comment.CommentId,
                AuthorId = comment.UserId.GetValueOrDefault()
            });

            return comment.CommentId;
        }
    }
}