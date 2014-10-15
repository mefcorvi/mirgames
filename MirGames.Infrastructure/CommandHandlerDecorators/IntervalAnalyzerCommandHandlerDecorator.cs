// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IntervalAnalyzerCommandHandlerDecorator.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.CommandHandlerDecorators
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Security.Claims;

    using MirGames.Infrastructure.Cache;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Exception;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Checks the interval of execution.
    /// </summary>
    internal sealed class IntervalAnalyzerCommandHandlerDecorator : ICommandHandlerDecorator
    {
        private readonly IClientHostAddressProvider clientHostAddressProvider;

        /// <summary>
        /// The cache manager.
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalAnalyzerCommandHandlerDecorator" /> class.
        /// </summary>
        /// <param name="cacheManagerFactory">The cache manager factory.</param>
        /// <param name="clientHostAddressProvider">The client host address provider.</param>
        public IntervalAnalyzerCommandHandlerDecorator(
            ICacheManagerFactory cacheManagerFactory,
            IClientHostAddressProvider clientHostAddressProvider)
        {
            Contract.Requires(cacheManagerFactory != null);
            Contract.Requires(clientHostAddressProvider != null);

            this.clientHostAddressProvider = clientHostAddressProvider;
            this.cacheManager = cacheManagerFactory.Create("CommandIntervals");
        }

        /// <inheritdoc />
        public float Order
        {
            get { return 1; }
        }

        /// <inheritdoc />
        public ICommandHandler Decorate(ICommandHandler commandHandler)
        {
            Contract.Requires(commandHandler != null);

            var apiAttribute = commandHandler.CommandType.GetCustomAttribute<ApiAttribute>();

            if (apiAttribute != null && apiAttribute.ExecutionInterval > 0)
            {
                var executionInterval = TimeSpan.FromMilliseconds(apiAttribute.ExecutionInterval);
                return new DecoratedHandler(this.cacheManager, this.clientHostAddressProvider, commandHandler, executionInterval);
            }

            return commandHandler;
        }

        private class DecoratedHandler : ICommandHandler
        {
            /// <summary>
            /// The underlying command handler.
            /// </summary>
            private readonly ICommandHandler underlyingCommandHandler;

            /// <summary>
            /// The cache manager.
            /// </summary>
            private readonly ICacheManager cacheManager;

            /// <summary>
            /// The client host address provider.
            /// </summary>
            private readonly IClientHostAddressProvider clientHostAddressProvider;

            /// <summary>
            /// The execution interval.
            /// </summary>
            private readonly TimeSpan executionInterval;

            /// <summary>
            /// Initializes a new instance of the <see cref="DecoratedHandler" /> class.
            /// </summary>
            /// <param name="cacheManager">The cache manager.</param>
            /// <param name="clientHostAddressProvider">The client host address provider.</param>
            /// <param name="underlyingCommandHandler">The underlying command handler.</param>
            /// <param name="executionInterval">The execution interval.</param>
            public DecoratedHandler(
                ICacheManager cacheManager,
                IClientHostAddressProvider clientHostAddressProvider,
                ICommandHandler underlyingCommandHandler,
                TimeSpan executionInterval)
            {
                Contract.Requires(underlyingCommandHandler != null);
                Contract.Requires(cacheManager != null);
                Contract.Requires(executionInterval != null);
                Contract.Requires(clientHostAddressProvider != null);

                this.underlyingCommandHandler = underlyingCommandHandler;
                this.cacheManager = cacheManager;
                this.clientHostAddressProvider = clientHostAddressProvider;
                this.executionInterval = executionInterval;
            }

            /// <inheritdoc />
            public Type CommandType
            {
                get { return this.underlyingCommandHandler.CommandType; }
            }

            /// <inheritdoc />
            public object Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
            {
                string hostAddress = this.clientHostAddressProvider.GetHostAddress();
                var key = string.Format("{0}:{1}:{2}", this.CommandType.FullName, hostAddress, principal.Identity.Name);

                if (this.cacheManager.Contains(key))
                {
                    throw new ExecutionIntervalException(key, this.executionInterval);
                }

                var result = this.underlyingCommandHandler.Execute(command, principal, authorizationManager);
                this.cacheManager.AddOrUpdate(key, true, DateTimeOffset.Now.Add(this.executionInterval));

                return result;
            }
        }
    }
}