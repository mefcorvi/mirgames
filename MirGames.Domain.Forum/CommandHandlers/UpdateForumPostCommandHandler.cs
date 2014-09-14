// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UpdateForumPostCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Forum.Events;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class UpdateForumPostCommandHandler : CommandHandler<UpdateForumPostCommand>
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
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateForumPostCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="textProcessor">The text transform.</param>
        public UpdateForumPostCommandHandler(
            IWriteContextFactory writeContextFactory,
            IEventBus eventBus,
            ICommandProcessor commandProcessor,
            ITextProcessor textProcessor)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(textProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.eventBus = eventBus;
            this.commandProcessor = commandProcessor;
            this.textProcessor = textProcessor;
        }

        /// <inheritdoc />
        public override void Execute(UpdateForumPostCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            ForumPost post;

            using (var writeContext = this.writeContextFactory.Create())
            {
                post = writeContext.Set<ForumPost>().Include(p => p.Topic).FirstOrDefault(t => t.PostId == command.PostId);

                if (post == null)
                {
                    throw new ItemNotFoundException("Post", command.PostId);
                }

                authorizationManager.EnsureAccess(principal, "Edit", "ForumPost", post.PostId);

                post.Text = this.textProcessor.GetHtml(command.Text);
                post.SourceText = command.Text;
                post.UpdatedDate = DateTime.UtcNow;

                if (post.IsStartPost && authorizationManager.CheckAccess(principal, "Edit", "ForumTopic", post.Topic.TopicId))
                {
                    var topic = post.Topic;
                    topic.Title = command.TopicTitle;
                    topic.TagsList = command.TopicsTags;
                    topic.ShortDescription = this.textProcessor.GetShortText(command.Text);

                    var oldTags = writeContext.Set<ForumTag>().Where(t => t.TopicId == topic.TopicId);
                    writeContext.Set<ForumTag>().RemoveRange(oldTags);

                    foreach (var tag in command.TopicsTags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        writeContext.Set<ForumTag>().Add(new ForumTag { TagText = tag.Trim(), TopicId = topic.TopicId });
                    }
                }

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

            this.eventBus.Raise(new ForumPostUpdatedEvent { PostId = command.PostId, TopicId = post.TopicId });
        }
    }
}