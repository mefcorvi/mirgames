// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="CommandsHub.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Hubs
{
    using System.Diagnostics.Contracts;

    using Microsoft.AspNet.SignalR;

    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Commands;

    using Newtonsoft.Json;

    /// <summary>
    /// The commands hub.
    /// </summary>
    public class CommandsHub : Hub
    {
        /// <summary>
        /// The command processor.
        /// </summary>
        private readonly ICommandProcessor commandProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandsHub"/> class.
        /// </summary>
        /// <param name="commandProcessor">The command processor.</param>
        public CommandsHub(ICommandProcessor commandProcessor)
        {
            this.commandProcessor = commandProcessor;
        }

        /// <inheritdoc />
        public object Execute(string commandJson)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(commandJson), "Command should be specified in JSON format.");

            var commandInstance = JsonConvert.DeserializeObject<Command>(commandJson);
            var result = this.commandProcessor.ExecuteWithResult(commandInstance);

            return result;
        }
    }
}