// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="VoteForForumPostCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Forum.CommandHandlers
{
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Entities;
    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Votes for the forum post.
    /// </summary>
    internal sealed class VoteForForumPostCommandHandler : CommandHandler<VoteForForumPostCommand, int>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoteForForumPostCommandHandler"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public VoteForForumPostCommandHandler(IWriteContextFactory writeContextFactory)
        {
            Contract.Requires(writeContextFactory != null);
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        protected override int Execute(VoteForForumPostCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            int userId = principal.GetUserId().GetValueOrDefault();

            using (var writeContext = this.writeContextFactory.Create())
            {
                var forumPost = writeContext.Set<ForumPost>().FirstOrDefault(p => p.PostId == command.PostId);

                if (forumPost == null)
                {
                    throw new ItemNotFoundException("ForumPost", command.PostId);
                }

                if (forumPost.AuthorId == userId)
                {
                    throw new ValidationException("User could not vote for himself");
                }

                var forumPostVote = writeContext.Set<ForumPostVote>().FirstOrDefault(p => p.PostId == command.PostId && p.UserId == userId);
                int vote = command.Positive ? 1 : -1;

                if (forumPostVote != null)
                {
                    forumPost.VotesRating -= forumPostVote.Vote;
                    forumPost.VotesRating += vote;
                    forumPostVote.Vote = vote;
                }
                else
                {
                    forumPost.VotesRating += vote;
                    forumPost.VotesCount += 1;

                    forumPostVote = new ForumPostVote
                    {
                        PostId = forumPost.PostId,
                        UserId = userId,
                        Vote = vote
                    };

                    writeContext.Set<ForumPostVote>().Add(forumPostVote);
                }

                writeContext.SaveChanges();

                return forumPost.VotesRating;
            }
        }
    }
}
