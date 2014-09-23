// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ValidationCommandHandlerDecorator.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.CommandHandlerDecorators
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Security;

    using ValidationAttribute = MirGames.Infrastructure.Commands.ValidationAttribute;

    /// <summary>
    /// Traces the command execution time.
    /// </summary>
    internal sealed class ValidationCommandHandlerDecorator : ICommandHandlerDecorator
    {
        /// <inheritdoc />
        public float Order
        {
            get { return 0; }
        }

        /// <inheritdoc />
        public ICommandHandler Decorate(ICommandHandler commandHandler)
        {
            return new ValidationCommandHandler(commandHandler);
        }

        /// <summary>
        /// The decorated handler.
        /// </summary>
        private class ValidationCommandHandler : ICommandHandler
        {
            /// <summary>
            /// The inner.
            /// </summary>
            private readonly ICommandHandler inner;

            /// <summary>
            /// Initializes a new instance of the <see cref="ValidationCommandHandler" /> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            public ValidationCommandHandler(ICommandHandler inner)
            {
                Contract.Requires(inner != null);
                this.inner = inner;
            }

            /// <inheritdoc />
            public Type CommandType
            {
                get { return this.inner.CommandType; }
            }

            /// <inheritdoc />
            public object Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
            {
                var commandType = command.GetType();
                var attributes = commandType.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>();

                if (!attributes.All(attribute => attribute.IsValid(command, principal, authorizationManager)))
                {
                    throw new InvalidCommandException(command);
                }

                var validationContext = new ValidationContext(command);
                Validator.ValidateObject(command, validationContext, true);

                return this.inner.Execute(command, principal, authorizationManager);
            }
        }
    }
}
