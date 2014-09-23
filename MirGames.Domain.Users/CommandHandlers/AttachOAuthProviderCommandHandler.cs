// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AttachOAuthProviderCommandHandler.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.CommandHandlers
{
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
    /// Attaches the provider to the current user.
    /// </summary>
    internal sealed class AttachOAuthProviderCommandHandler : CommandHandler<AttachOAuthProviderCommand>
    {
        /// <summary>
        /// The write context factory.
        /// </summary>
        private readonly IWriteContextFactory writeContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachOAuthProviderCommandHandler"/> class.
        /// </summary>
        /// <param name="writeContextFactory">The write context factory.</param>
        public AttachOAuthProviderCommandHandler(IWriteContextFactory writeContextFactory)
        {
            this.writeContextFactory = writeContextFactory;
        }

        /// <inheritdoc />
        protected override void Execute(AttachOAuthProviderCommand command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principal.GetUserId() != null);

            using (var writeContext = this.writeContextFactory.Create())
            {
                string providerName = command.ProviderName.ToLower();
                int userId = principal.GetUserId().GetValueOrDefault();

                var provider = writeContext.Set<OAuthProvider>().FirstOrDefault(p => p.Name == providerName);

                if (provider == null)
                {
                    throw new ItemNotFoundException("OAuth Provider", providerName);
                }

                var oldTokens = writeContext.Set<OAuthToken>().Where(t => t.ProviderId == provider.Id && t.UserId == userId);
                writeContext.Set<OAuthToken>().RemoveRange(oldTokens);
                writeContext.SaveChanges();

                var newToken = new OAuthToken
                    {
                        ProviderId = provider.Id,
                        ProviderUserId = command.ProviderUserId,
                        UserId = userId
                    };

                writeContext.Set<OAuthToken>().Add(newToken);
                writeContext.SaveChanges();

                var tokenData = command.Data.Select(
                    pair => new OAuthTokenData
                        {
                            Key = pair.Key,
                            Value = pair.Value,
                            TokenId = newToken.Id
                        });

                writeContext.Set<OAuthTokenData>().AddRange(tokenData);
                writeContext.SaveChanges();
            }
        }
    }
}