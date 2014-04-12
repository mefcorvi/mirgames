// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicDeletedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.EventListeners
{
    using System;
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.SearchEngine;

    /// <summary>
    /// Handles the topic created event.
    /// </summary>
    internal sealed class TopicDeletedEventListener : EventListenerBase<TopicDeletedEvent>
    {
        /// <summary>
        /// The search engine.
        /// </summary>
        private readonly ISearchEngine searchEngine;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicDeletedEventListener" /> class.
        /// </summary>
        /// <param name="searchEngine">The search engine.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public TopicDeletedEventListener(ISearchEngine searchEngine, Lazy<ICommandProcessor> commandProcessor)
        {
            Contract.Requires(searchEngine != null);

            this.searchEngine = searchEngine;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(TopicDeletedEvent @event)
        {
            this.searchEngine.Remove(@event.TopicId, "Topic");
            this.commandProcessor.Value.Execute(new RemoveAttachmentsCommand { EntityId = @event.TopicId, EntityType = "topic" });
        }
    }
}
