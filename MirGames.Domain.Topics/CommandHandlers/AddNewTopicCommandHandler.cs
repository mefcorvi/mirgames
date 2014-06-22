// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AddNewTopicCommandHandler.cs">
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
    using System.Security.Claims;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Attachments.Commands;
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
    /// Handles the login command.
    /// </summary>
    internal sealed class AddNewTopicCommandHandler : CommandHandler<AddNewTopicCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The text hash provider.
        /// </summary>
        private readonly ITextHashProvider textHashProvider;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// The short text extractor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="textHashProvider">The text hash provider.</param>
        /// <param name="eventBus">The event bus.</param>
        /// <param name="textProcessor">The short text extractor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public AddNewTopicCommandHandler(
            IWriteContextFactory writeContextFactory,
            ITextHashProvider textHashProvider,
            IEventBus eventBus,
            ITextProcessor textProcessor,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(textHashProvider != null);
            Contract.Requires(eventBus != null);
            Contract.Requires(textProcessor != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.textHashProvider = textHashProvider;
            this.eventBus = eventBus;
            this.textProcessor = textProcessor;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override int Execute(AddNewTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            var topic = new Topic
            {
                AuthorId = principal.GetUserId().GetValueOrDefault(),
                CountComment = 0,
                TextHash = this.textHashProvider.GetHash(command.Text),
                TopicTitle = command.Title,
                TagsList = command.Tags,
                CreationDate = DateTime.UtcNow,
                UserIp = principal.GetHostAddress(),
                CountFavorite = 0,
                CountRead = 0,
                CountVote = 0,
                CountVoteAbstain = 0,
                CountVoteDown = 0,
                CountVoteUp = 0,
                CutText = "Read more...",
                IsPublished = true,
                IsPublishedDraft = false,
                IsPublishedIndex = false,
                IsRepost = false,
                IsTutorial = false,
                Rating = 0,
                SourceAuthor = null,
                SourceLink = null,
                TopicType = "topic",
                ForbidComment = false,
                EditDate = null,
                BlogId = command.BlogId
            };

            authorizationManager.EnsureAccess(principal, "AddNew", "Topic");

            if (command.BlogId.HasValue)
            {
                authorizationManager.EnsureAccess(principal, "CreateTopic", "Blog", command.BlogId);
            }

            using (var writeContext = this.writeContextFactory.Create())
            {
                writeContext.Set<Topic>().Add(topic);
                writeContext.SaveChanges();

                foreach (var tag in command.Tags.Split(','))
                {
                    writeContext.Set<TopicTag>().Add(new TopicTag { TagText = tag.Trim(), TopicId = topic.Id });
                }

                writeContext.Set<TopicContent>().Add(
                    new TopicContent
                        {
                            Topic = topic,
                            TopicText = this.textProcessor.GetHtml(command.Text),
                            TopicTextSource = command.Text,
                            TopicTextShort = this.textProcessor.GetShortHtml(command.Text),
                            TopicExtra = "Read more..."
                        });

                writeContext.SaveChanges();
            }

            if (!command.Attachments.IsNullOrEmpty())
            {
                this.commandProcessor.Execute(
                    new PublishAttachmentsCommand
                        {
                            EntityId = topic.Id,
                            Identifiers = command.Attachments
                        });
            }

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "Edit", "Delete" },
                EntityId = topic.Id,
                IsDenied = false,
                EntityType = "Topic",
                UserId = topic.AuthorId,
                ExpirationDate = DateTime.Now.AddDays(7)
            });

            this.eventBus.Raise(
                new TopicCreatedEvent
                    {
                        Title = command.Title,
                        TopicId = topic.Id,
                        Tags = command.Tags,
                        Text = command.Text,
                        AuthorId = principal.GetUserId().GetValueOrDefault()
                    });

            return topic.Id;
        }
    }
}