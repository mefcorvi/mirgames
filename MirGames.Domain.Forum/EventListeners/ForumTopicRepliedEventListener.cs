// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumTopicRepliedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.EventListeners
{
    using System;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    internal sealed class ForumTopicRepliedEventListener : EventListenerBase<ForumTopicRepliedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly Lazy<ICommandProcessor> commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumTopicRepliedEventListener"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumTopicRepliedEventListener(Lazy<ICommandProcessor> commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumTopicRepliedEvent @event)
        {
            this.commandProcessor.Value.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
            this.commandProcessor.Value.Execute(new MarkTopicAsUnreadForUsersCommand { TopicId = @event.TopicId, TopicDate = @event.RepliedDate });
            this.commandProcessor.Value.Execute(new MarkTopicAsReadCommand { TopicId = @event.TopicId });
        }
    }
}