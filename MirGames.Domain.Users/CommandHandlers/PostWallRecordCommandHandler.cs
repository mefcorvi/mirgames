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