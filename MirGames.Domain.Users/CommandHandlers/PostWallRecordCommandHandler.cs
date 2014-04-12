// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="PostWallRecordCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class PostWallRecordCommandHandler : CommandHandler<PostWallRecordCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostWallRecordCommandHandler"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public PostWallRecordCommandHandler(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        public override int Execute(PostWallRecordCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                var user = writeContext.Set<User>().SingleOrDefault(t => t.Id == command.UserId);

                if (user == null)
                {
                    throw new ItemNotFoundException("Topic", command.UserId);
                }

                authorizationManager.EnsureAccess(principal, "PostWallRecord", user);

                var newWallRecord = new WallRecord
                    {
                        AuthorId = principal.GetUserId().GetValueOrDefault(),
                        AuthorIp = principal.GetHostAddress(),
                        DateAdd = DateTime.UtcNow,
                        RepliesCount = 0,
                        Text = command.Text,
                        WallUserId = user.Id,
                        LastReplyId = string.Empty
                    };

                writeContext.Set<WallRecord>().Add(newWallRecord);
                writeContext.SaveChanges();

                return newWallRecord.Id;
            }
        }
    }
}