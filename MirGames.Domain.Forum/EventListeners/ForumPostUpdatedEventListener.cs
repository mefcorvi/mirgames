// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumPostUpdatedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.EventListeners
{
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about topic replied.
    /// </summary>
    internal sealed class ForumPostUpdatedEventListener : EventListenerBase<ForumPostUpdatedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ForumPostUpdatedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumPostUpdatedEventListener(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(ForumPostUpdatedEvent @event)
        {
            this.commandProcessor.Execute(new ReindexForumTopicCommand { TopicId = @event.TopicId });
        }
    }
}