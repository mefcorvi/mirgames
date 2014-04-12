// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ReplyForumTopicCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class ReplyForumTopicCommandHandler : CommandHandler<ReplyForumTopicCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The text transform.
        /// </summary>
        private readonly ITextTransform textTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyForumTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textTransform">The text transform.</param>
        public ReplyForumTopicCommandHandler(
            IWriteContextFactory writeContextFactory,
            IEventBus eventBus,
            IQueryProcessor queryProcessor,
            ICommandProcessor commandProcessor,
            ITextTransform textTransform)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(queryProcessor != null);
            Contract.Requires(eventBus != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
            this.queryProcessor = queryProcessor;
            this.commandProcessor = commandProcessor;
            this.textTransform = textTransform;
        }

        /// <inheritdoc />
        public override int Execute(ReplyForumTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            int authorId = principal.GetUserId().GetValueOrDefault();
            var author = this.queryProcessor.Process(
                new GetAuthorQuery
                    {
                        UserId = authorId
                    });

            ForumTopic topic;
            ForumPost post;

            using (var writeContext = this.writeContextFactory.Create())
            {
                topic = writeContext.Set<ForumTopic>().FirstOrDefault(t => t.TopicId == command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                authorizationManager.EnsureAccess(principal, "Reply", topic);

                post = new ForumPost
                {
                    AuthorId = author.Id,
                    AuthorLogin = author.Login,
                    AuthorIP = principal.GetHostAddress(),
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsHidden = false,
                    Text = this.textTransform.Transform(command.Text),
                    SourceText = command.Text,
                    TopicId = command.TopicId
                };

                writeContext.Set<ForumPost>().Add(post);
                
                topic.PostsCount++;
                topic.LastPostAuthorId = authorId;
                topic.UpdatedDate = DateTime.UtcNow;
                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new Attachments.Commands.PublishAttachmentsCommand
                        {
                            EntityId = post.PostId,
                            Identifiers = command.Attachments
                        });
            }

            this.eventBus.Raise(new ForumTopicRepliedEvent { TopicId = topic.TopicId, AuthorId = author.Id, PostId = post.PostId, RepliedDate = post.CreatedDate });

            return post.PostId;
        }
    }
}