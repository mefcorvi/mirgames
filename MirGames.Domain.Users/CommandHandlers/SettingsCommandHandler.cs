// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="SettingsCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Domain.Users.Entities;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Handles the setting changing command.
    /// </summary>
    /// <typeparam name="T">Type of the command.</typeparam>
    internal abstract class SettingsCommandHandler<T> : CommandHandler<T> where T : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCommandHandler{T}"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        protected SettingsCommandHandler(IWriteContextFactory writeContextFactory)
        {
            this.WriteContextFactory = writeContextFactory;
        }

        /// <summary>
        /// Gets or sets the write context factory.
        /// </summary>
        private IWriteContextFactory WriteContextFactory { get; set; }

        /// <inheritdoc />
        protected sealed override void Execute(T command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            using (var writeContext = this.WriteContextFactory.Create())
            {
                int userId = principal.GetUserId().GetValueOrDefault();
                var user = writeContext.Set<User>().Single(u => u.Id == userId);

                this.ChangeSetting(user.Settings, command);

                writeContext.SaveChanges();
            }
        }

        /// <summary>
        /// Changes the setting.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="command">The command.</param>
        protected abstract void ChangeSetting(IDictionary<string, object> settings, T command);
    }
}