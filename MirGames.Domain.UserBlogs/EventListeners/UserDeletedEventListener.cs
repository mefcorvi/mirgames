// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserDeletedEventListener.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.UserBlogs.EventListeners
{
    using System.Diagnostics.Contracts;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Users.Events;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Events;

    internal sealed class UserDeletedEventListener : EventListenerBase<UserDeletedEvent>
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// The query processor.
        /// </summary>
        private readonly IQueryProcessor queryProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDeletedEventListener" /> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        /// <param name="queryProcessor">The query processor.</param>
        public UserDeletedEventListener(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            Contract.Requires(commandProcessor != null);
            Contract.Requires(queryProcessor != null);

            this.commandProcessor = commandProcessor;
            this.queryProcessor = queryProcessor;
        }

        /// <inheritdoc />
        public override void Process(UserDeletedEvent @event)
        {
            var blog = this.queryProcessor.Process(new GetBlogByEntityQuery
            {
                EntityId = @event.UserId,
                EntityType = "User"
            });

            if (blog == null)
            {
                throw new ItemNotFoundException("UserBlog", @event.UserId);
            }

            this.commandProcessor.Execute(new RemoveBlogCommand
            {
                BlogId = blog.BlogId.GetValueOrDefault()
            });
        }
    }
}
