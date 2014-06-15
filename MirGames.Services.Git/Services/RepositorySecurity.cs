// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RepositorySecurity.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.Services
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Domain.Security;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Security;
    using MirGames.Services.Git.Entities;

    internal sealed class RepositorySecurity : IRepositorySecurity
    {
        /// <summary>
        /// The read context factory.
        /// </summary>
        private readonly IReadContextFactory readContextFactory;

        /// <summary>
        /// The principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> principalProvider;

        /// <summary>
        /// The authorization manager.
        /// </summary>
        private readonly IAuthorizationManager authorizationManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositorySecurity" /> class.
        /// </summary>
        /// <param name="readContextFactory">The read context factory.</param>
        /// <param name="principalProvider">The principal provider.</param>
        /// <param name="authorizationManager">The authorization manager.</param>
        public RepositorySecurity(
            IReadContextFactory readContextFactory,
            Func<ClaimsPrincipal> principalProvider,
            IAuthorizationManager authorizationManager)
        {
            Contract.Requires(principalProvider != null);
            Contract.Requires(readContextFactory != null);

            this.readContextFactory = readContextFactory;
            this.principalProvider = principalProvider;
            this.authorizationManager = authorizationManager;
        }

        /// <inheritdoc />
        public bool CanWrite(string repositoryName)
        {
            var principal = this.principalProvider.Invoke();
            var repositoryId = this.GetRepositoryId(repositoryName);

            return this.authorizationManager.CheckAccess(principal, "Write", "GitRepository", repositoryId);
        }

        /// <inheritdoc />
        public bool CanRead(string repositoryName)
        {
            var principal = this.principalProvider.Invoke();
            var repositoryId = this.GetRepositoryId(repositoryName);

            return this.authorizationManager.CheckAccess(principal, "Read", "GitRepository", repositoryId);
        }

        /// <inheritdoc />
        public bool CanDelete(string repositoryName)
        {
            var principal = this.principalProvider.Invoke();
            var repositoryId = this.GetRepositoryId(repositoryName);

            return this.authorizationManager.CheckAccess(principal, "Delete", "GitRepository", repositoryId);
        }

        /// <summary>
        /// Gets the repository access.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>The repository access.</returns>
        /// <exception cref="System.Exception">Repository have not been found</exception>
        private int GetRepositoryId(string repositoryName)
        {
            using (var readContext = this.readContextFactory.Create())
            {
                var repository = readContext.Query<Repository>().FirstOrDefault(r => r.Name == repositoryName);

                if (repository == null)
                {
                    throw new Exception(string.Format("Repository \"{0}\" has not been found", repositoryName));
                }

                return repository.Id;
            }
        }
    }
}