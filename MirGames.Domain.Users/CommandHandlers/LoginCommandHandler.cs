namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the login command.
    /// </summary>
    internal sealed class LoginCommandHandler : CommandHandler<LoginCommand, string>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// The password hash provider.
        /// </summary>
        private readonly IPasswordHashProvider passwordHashProvider;

        /// <summary>
        /// The client host address provider.
        /// </summary>
        private readonly IClientHostAddressProvider clientHostAddressProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="eventLog">The event log.</param>
        /// <param name="passwordHashProvider">The password hash provider.</param>
        /// <param name="clientHostAddressProvider">The client host address provider.</param>
        public LoginCommandHandler(
            IWriteContextFactory writeContextFactory,
            IEventLog eventLog,
            IPasswordHashProvider passwordHashProvider,
            IClientHostAddressProvider clientHostAddressProvider)
        {
            Contract.Requires(writeContextFactory != null);
            Contract.Requires(eventLog != null);
            Contract.Requires(passwordHashProvider != null);
            Contract.Requires(clientHostAddressProvider != null);

            this.writeContextFactory = writeContextFactory;
            this.eventLog = eventLog;
            this.passwordHashProvider = passwordHashProvider;
            this.clientHostAddressProvider = clientHostAddressProvider;
        }

        /// <inheritdoc />
        public override string Execute(LoginCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            var sessionId = Guid.NewGuid().ToString("N");

            using (var writeContext = this.writeContextFactory.Create())
            {
                var user = writeContext.Set<User>().FirstOrDefault(u => u.Login == command.EmailOrLogin || u.Mail == command.EmailOrLogin);

                if (user == null)
                {
                    return null;
                }

                string password = this.passwordHashProvider.GetHash(command.Password, user.PasswordSalt);
                if (user.Password != password)
                {
                    return null;
                }

                writeContext.Set<UserSession>().Add(
                    new UserSession
                        {
                            CreateDate = DateTime.UtcNow,
                            LastDate = DateTime.UtcNow,
                            CreationIP = this.clientHostAddressProvider.GetHostAddress(),
                            LastVisitIP = this.clientHostAddressProvider.GetHostAddress(),
                            UserId = user.Id,
                            Id = sessionId
                        });

                writeContext.SaveChanges();
            }

            this.eventLog.LogInformation("LoginCommandHandler", "User \"{0}\" singed-in with session \"{1}\"", command.EmailOrLogin, sessionId);

            return sessionId;
        }
    }
}