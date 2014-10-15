// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RemoveBlogCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Acl.Public.Commands;
    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    public sealed class RemoveBlogCommandHandler : CommandHandler<RemoveBlogCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveBlogCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public RemoveBlogCommandHandler(IWriteContextFactory writeContextFactory, ICommandProcessor commandProcessor)
        {
            Contract.Requires(commandProcessor != null);
            Contract.Requires(writeContextFactory != null);

            this.writeContextFactory = writeContextFactory;
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        protected override void Execute(RemoveBlogCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                var blog = writeContext.Set<Blog>().FirstOrDefault(b => b.Id == command.BlogId);
                
                if (blog == null)
                {
                    throw new ItemNotFoundException("Blog", command.BlogId);
                }

                writeContext.Set<Blog>().Remove(blog);
                writeContext.SaveChanges();
            }

            this.commandProcessor.Execute(new RemovePermissionsCommand
            {
                EntityId = command.BlogId,
                EntityType = "Topic"
            });
        }
    }
}