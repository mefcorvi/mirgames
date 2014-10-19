// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkAllBlogTopicsAsReadCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Security;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Events;
    using MirGames.Domain.Topics.Notifications;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class MarkAllBlogTopicsAsReadCommandHandler : CommandHandler<MarkAllBlogTopicsAsReadCommand>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkAllBlogTopicsAsReadCommandHandler" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="eventBus">The event bus.</param>
        public MarkAllBlogTopicsAsReadCommandHandler(ICommandProcessor commandProcessor, IEventBus eventBus)
        {
            Contract.Requires(commandProcessor != null);
            Contract.Requires(eventBus != null);

            this.commandProcessor = commandProcessor;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override void Execute(
            MarkAllBlogTopicsAsReadCommand command,
            ClaimsPrincipal principal,
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            int userId = principal.GetUserId().GetValueOrDefault();

            this.commandProcessor.Execute(new RemoveNotificationsCommand
            {
                Filter = n => n is NewBlogTopicNotification || n is NewTopicCommentNotification,
                UserIdentifiers = new[] { principal.GetUserId().GetValueOrDefault() }
            });

            this.eventBus.Raise(new AllBlogTopicsReadEvent
            {
                UserId = userId
            });
        }
    }
}