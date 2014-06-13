// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SaveTopicCommandHandler.cs">
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

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.TextTransform;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.SearchEngine;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class SaveTopicCommandHandler : CommandHandler<SaveTopicCommand>
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
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The text processor.
        /// </summary>
        private readonly ITextProcessor textProcessor;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveTopicCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="textHashProvider">The text hash provider.</param>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="textProcessor">The short text extractor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public SaveTopicCommandHandler(
            IWriteContextFactory writeContextFactory,
            ITextHashProvider textHashProvider,
            ISearchEngine searchEngine,
            ITextProcessor textProcessor,
            ICommandProcessor commandProcessor)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(textHashProvider != null);
            Contract.Requires(searchEngine != null);
            Contract.Requires(textProcessor != null);
            Contract.Requires(commandProcessor != null);

            this.writeContextFactory = writeContextFactory;
            this.textHashProvider = textHashProvider;
            this.searchEngine = searchEngine;
            this.textProcessor = textProcessor;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Execute(SaveTopicCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            Topic topic;

            using (var writeContext = this.writeContextFactory.Create())
            {
                topic = writeContext.Set<Topic>().SingleOrDefault(t => t.Id == command.TopicId);
                authorizationManager.EnsureAccess(principal, "Edit", "Topic", command.TopicId);

                if (topic == null)
                {
                    throw new ItemNotFoundException("Topic", command.TopicId);
                }

                topic.TextHash = this.textHashProvider.GetHash(command.Text);
                topic.TopicTitle = command.Title;
                topic.TagsList = command.Tags;
                topic.EditDate = DateTime.UtcNow;
                topic.UserIp = principal.GetHostAddress();

                foreach (var oldTag in writeContext.Set<TopicTag>().Where(t => t.TopicId == command.TopicId))
                {
                    writeContext.Set<TopicTag>().Remove(oldTag);
                }

                var topicContent = writeContext.Set<TopicContent>().Single(content => content.TopicId == command.TopicId);
                topicContent.TopicText = this.textProcessor.GetHtml(command.Text);
                topicContent.TopicTextSource = command.Text;
                topicContent.TopicTextShort = this.textProcessor.GetShortHtml(command.Text);
                
                foreach (var tag in command.Tags.Split(','))
                {
                    writeContext.Set<TopicTag>().Add(new TopicTag { TagText = tag.Trim(), TopicId = command.TopicId });
                }

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

            this.searchEngine.Remove(topic.Id, "Topic");
            this.searchEngine.Index(topic.Id, "Topic", command.Title + " " + command.Text + " " + command.Tags);
        }
    }
}