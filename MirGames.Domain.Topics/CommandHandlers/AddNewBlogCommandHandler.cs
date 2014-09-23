// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AddNewBlogCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Topics.CommandHandlers
{
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Entities;
    using MirGames.Domain.Topics.Exceptions;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    public sealed class AddNewBlogCommandHandler : CommandHandler<AddNewBlogCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddNewBlogCommandHandler"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public AddNewBlogCommandHandler(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        protected override int Execute(AddNewBlogCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                if (writeContext.Set<Blog>().Any(b => b.Alias == command.Alias))
                {
                    throw new BlogAlreadyRegisteredException(command.Alias);
                }

                var blog = new Blog
                {
                    Alias = command.Alias,
                    Description = command.Description,
                    Title = command.Title,
                    EntityId = command.EntityId,
                    EntityType = command.EntityType
                };

                writeContext.Set<Blog>().Add(blog);
                writeContext.SaveChanges();

                return blog.Id;
            }
        }
    }
}