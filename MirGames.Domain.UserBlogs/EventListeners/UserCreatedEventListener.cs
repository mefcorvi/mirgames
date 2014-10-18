// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserCreatedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.UserBlogs.EventListeners
{
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    internal sealed class UserCreatedEventListener : EventListenerBase<UserCreatedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCreatedEventListener"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public UserCreatedEventListener(ICommandProcessor commandProcessor)
        {
            Contract.Requires(commandProcessor != null);
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public override void Process(UserCreatedEvent @event)
        {
            var newBlogCommand = new AddNewBlogCommand
            {
                Alias = "user-" + @event.UserId,
                Description = "Блог " + @event.UserName,
                EntityId = @event.UserId,
                EntityType = "User",
                Title = @event.UserName
            };

            var blogId = this.commandProcessor.Execute(newBlogCommand);

            this.commandProcessor.Execute(new SetPermissionCommand
            {
                Actions = new[] { "CreateTopic", "DeleteTopic", "EditTopic" },
                EntityId = blogId,
                EntityType = "Blog",
                UserId = @event.UserId
            });
        }
    }
}
