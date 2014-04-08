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
        public IWriteContextFactory WriteContextFactory { get; set; }

        /// <inheritdoc />
        public sealed override void Execute(T command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
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