// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TraceCommandHandlerDecorator.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure.CommandHandlerDecorators
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Security.Claims;

    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Traces the command execution time.
    /// </summary>
    internal sealed class TraceCommandHandlerDecorator : ICommandHandlerDecorator
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// Initializes a new instance of the <see cref="TraceCommandHandlerDecorator"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="eventLog">The event log.</param>
        public TraceCommandHandlerDecorator(ISettings settings, IEventLog eventLog)
        {
            Contract.Requires(eventLog != null);
            Contract.Requires(settings != null);

            this.settings = settings;
            this.eventLog = eventLog;
        }

        /// <inheritdoc />
        public float Order
        {
            get { return float.MaxValue; }
        }

        /// <inheritdoc />
        public ICommandHandler Decorate(ICommandHandler commandHandler)
        {
            bool isTraceEnabled = this.settings.GetValue("CommandBus.TraceEnabled", false);
            return isTraceEnabled ? new TraceCommandHandler(commandHandler, this.eventLog) : commandHandler;
        }

        /// <summary>
        /// The decorated handler.
        /// </summary>
        private class TraceCommandHandler : ICommandHandler
        {
            /// <summary>
            /// The inner.
            /// </summary>
            private readonly ICommandHandler inner;

            /// <summary>
            /// The event log.
            /// </summary>
            private readonly IEventLog eventLog;

            /// <summary>
            /// Initializes a new instance of the <see cref="TraceCommandHandler" /> class.
            /// </summary>
            /// <param name="inner">The inner.</param>
            /// <param name="eventLog">The event log.</param>
            public TraceCommandHandler(ICommandHandler inner, IEventLog eventLog)
            {
                Contract.Requires(inner != null);
                this.inner = inner;
                this.eventLog = eventLog;
            }

            /// <inheritdoc />
            public Type CommandType
            {
                get { return this.inner.CommandType; }
            }

            /// <inheritdoc />
            public object Execute(Command command, ClaimsPrincipal principal, IAuthorizationManager authorizationManager)
            {
                var sw = new Stopwatch();
                sw.Start();

                var result = this.inner.Execute(command, principal, authorizationManager);

                sw.Stop();
                this.TraceCommand(command, sw.ElapsedMilliseconds);

                return result;
            }

            /// <summary>
            /// Traces the command.
            /// </summary>
            /// <param name="command">The command.</param>
            /// <param name="elapsedMilliseconds">The elapsed milliseconds.</param>
            private void TraceCommand(Command command, long elapsedMilliseconds)
            {
                var commandType = command.GetType();
                var attributes = commandType.GetCustomAttributes(typeof(DisableTracingAttribute), true).Cast<DisableTracingAttribute>();

                if (attributes.Any())
                {
                    return;
                }

                this.eventLog.Log(
                    EventLogType.Verbose,
                    "CommandBus.Execute",
                    string.Format("Execution of command \"{0}\" has completed in \"{1}\" ms", commandType.Name, elapsedMilliseconds),
                    command);
            }
        }
    }
}
