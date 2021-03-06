﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicCreatedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.EventListeners
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Domain.Users.Queries;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.SearchEngine;

    /// <summary>
    /// Handles the topic created event.
    /// </summary>
    internal sealed class TopicCreatedEventListener : EventListenerBase<TopicCreatedEvent>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicCreatedEventListener" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public TopicCreatedEventListener(ISearchEngine searchEngine, ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            Contract.Requires(searchEngine != null);
            Contract.Requires(commandProcessor != null);
            Contract.Requires(queryProcessor != null);

            this.searchEngine = searchEngine;
            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(TopicCreatedEvent @event)
        {
            this.searchEngine.Index(
                @event.TopicId,
                "Topic",
                @event.Title + " " + @event.Text,
                new SearchIndexTerm("tags", @event.Tags) { IsIndexed = true, IsNormalized = false });

            if (!@event.BlogId.HasValue)
            {
                var userIdentifiers = this.queryProcessor.Process(new GetUsersIdentifiersQuery()).Except(new[] { @event.AuthorId });

                this.commandProcessor.Execute(new NotifyUsersCommand
                {
                    UserIdentifiers = userIdentifiers.ToArray(),
                    NotificationTemplate = new NewBlogTopicNotification
                    {
                        TopicId = @event.TopicId,
                        BlogId = @event.BlogId
                    }
                });
            }
        }
    }
}
