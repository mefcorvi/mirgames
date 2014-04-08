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