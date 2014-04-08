namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The logout command handler.
    /// </summary>
    internal sealed class LogoutCommandHandler : CommandHandler<LogoutCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The online users manager.
        /// </summary>
        private readonly IOnlineUsersManager onlineUsersManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="onlineUsersManager">The online users manager.</param>
        public LogoutCommandHandler(IWriteContextFactory writeContextFactory, IOnlineUsersManager onlineUsersManager)
        {
            this.writeContextFactory = writeContextFactory;
            this.onlineUsersManager = onlineUsersManager;
        }

        /// <inheritdoc />
        public override void Execute(LogoutCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);
            Contract.Requires(principal.GetSessionId() != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                string sessionId = principal.GetSessionId();

                var oldSession = writeContext.Set<UserSession>().SingleOrDefault(s => s.UserId == userId && s.Id == sessionId);

                if (oldSession != null)
                {
                    writeContext.Set<UserSession>().Remove(oldSession);
                }

                writeContext.SaveChanges();
            }

            this.onlineUsersManager.MarkUserAsOffline(principal);
        }
    }
}