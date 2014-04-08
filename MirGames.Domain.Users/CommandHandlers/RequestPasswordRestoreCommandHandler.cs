namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Exceptions;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Events;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the requests for password restoring.
    /// </summary>
    internal sealed class RequestPasswordRestoreCommandHandler : CommandHandler<RequestPasswordRestoreCommand>
    {
        /// <summary>
        /// The write context factory
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The password hash provider.
        /// </summary>
        private readonly IPasswordHashProvider passwordHashProvider;

        /// <summary>
        /// The event bus.
        /// </summary>
        private readonly IEventBus eventBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPasswordRestoreCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="passwordHashProvider">The password hash provider.</param>
        /// <param name="eventBus">The event bus.</param>
        public RequestPasswordRestoreCommandHandler(IWriteContextFactory writeContextFactory, IPasswordHashProvider passwordHashProvider, IEventBus eventBus)
        {
            this.writeContextFactory = writeContextFactory;
            this.passwordHashProvider = passwordHashProvider;
            this.eventBus = eventBus;
        }

        /// <inheritdoc />
        public override void Execute(RequestPasswordRestoreCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            User user;
            string secretKey = Guid.NewGuid().ToString().GetMd5Hash();

            using (var writeContext = this.writeContextFactory.Create())
            {
                user = writeContext.Set<User>().SingleOrDefault(u => u.Mail == command.EmailOrLogin || u.Login == command.EmailOrLogin);

                if (user == null)
                {
                    throw new ItemNotFoundException("User", command.EmailOrLogin);
                }

                var passwordRestoreRequest = new PasswordRestoreRequest
                    {
                        CreationDate = DateTime.UtcNow,
                        NewPassword = this.passwordHashProvider.GetHash(command.NewPasswordHash, user.PasswordSalt),
                        SecretKey = secretKey,
                        UserId = user.Id
                    };

                var oldRequets = writeContext.Set<PasswordRestoreRequest>().Where(r => r.UserId == user.Id);
                writeContext.Set<PasswordRestoreRequest>().RemoveRange(oldRequets);
                writeContext.Set<PasswordRestoreRequest>().Add(passwordRestoreRequest);

                writeContext.SaveChanges();
            }

            this.eventBus.Raise(new PasswordRestoreRequestedEvent { UserId = user.Id, SecretKey = secretKey });
        }
    }
}