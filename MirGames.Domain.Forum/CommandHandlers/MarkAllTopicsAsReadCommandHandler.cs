// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="MarkAllTopicsAsReadCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Security.Claims;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Notifications;
    using MirGames.Domain.Notifications.Commands;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the reply forum topic command.
    /// </summary>
    internal sealed class MarkAllTopicsAsReadCommandHandler : CommandHandler<MarkAllTopicsAsReadCommand>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkAllTopicsAsReadCommandHandler" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public MarkAllTopicsAsReadCommandHandler(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
            Contract.Requires(commandProcessor != null);
        }

        /// <inheritdoc />
        protected override void Execute(MarkAllTopicsAsReadCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            this.commandProcessor.Execute(new RemoveNotificationsCommand
            {
                Filter = n => n is NewForumAnswerNotification || n is NewForumTopicNotification,
                UserIdentifiers = new[] { principal.GetUserId().GetValueOrDefault() }
            });
        }
    }
}