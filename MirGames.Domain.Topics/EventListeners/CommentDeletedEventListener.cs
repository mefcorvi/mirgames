// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommentDeletedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.EventListeners
{
    using MirGames.Domain.Attachments.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    /// <summary>
    /// Event about deleted comment
    /// </summary>
    internal sealed class CommentDeletedEventListener : EventListenerBase<CommentDeletedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDeletedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public CommentDeletedEventListener(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(CommentDeletedEvent @event)
        {
            this.commandProcessor.Execute(new RemoveAttachmentsCommand { EntityId = @event.CommentId, EntityType = "comment" });
        }
    }
}