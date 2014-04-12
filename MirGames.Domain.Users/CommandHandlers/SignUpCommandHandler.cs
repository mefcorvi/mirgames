// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SignUpCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Entities;
    using MirGames.Domain.Users.Exceptions;
    using MirGames.Domain.Users.Notifications;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Notifications;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// The sign up command handler.
    /// </summary>
    internal sealed class SignUpCommandHandler : CommandHandler<SignUpCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// The password hash provider.
        /// </summary>
        private readonly IPasswordHashProvider passwordHashProvider;

        /// <summary>
        /// The notification manager.
        /// </summary>
        private readonly INotificationManager notificationManager;

        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignUpCommandHandler" /> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        /// <param name="passwordHashProvider">The password hash provider.</param>
        /// <param name="notificationManager">The notification manager.</param>
        /// <param name="settings">The settings.</param>
        public SignUpCommandHandler(IWriteContextFactory writeContextFactory, IPasswordHashProvider passwordHashProvider, INotificationManager notificationManager, ISettings settings)
        {
            this.writeContextFactory = writeContextFactory;
            this.passwordHashProvider = passwordHashProvider;
            this.notificationManager = notificationManager;
            this.settings = settings;
        }

        /// <inheritdoc />
        public override void Execute(SignUpCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            using (var writeContext = this.writeContextFactory.Create())
            {
                bool isUserRegistered = writeContext.Set<User>().Any(u => u.Login == command.Login || u.Mail == command.Email);

                if (isUserRegistered)
                {
                    throw new UserAlreadyRegisteredException(command.Login, command.Email);
                }

                string salt = Guid.NewGuid().ToString();

                User user = User.Create(command.Login, command.Email, this.passwordHashProvider.GetHash(command.Password, salt));
                user.RegistrationIP = principal.GetHostAddress();
                user.PasswordSalt = salt;
                user.UserActivationKey = Guid.NewGuid().ToString().GetMd5Hash();
                user.IsActivated = false;
                writeContext.Set<User>().Add(user);
                writeContext.SaveChanges();

                var activationUrl = string.Format(this.settings.GetValue<string>("Activation.Url"), user.UserActivationKey);

                this.notificationManager.SendNotification(
                    user.Mail,
                    new ActivationRequestedNotification
                        {
                            ActivationCode = user.UserActivationKey,
                            ActivationLink = activationUrl
                        });
            }
        }
    }
}