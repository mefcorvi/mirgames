namespace MirGames.Infrastructure.Commands
{
    using MirGames.Infrastructure.Exception;

    /// <summary>
    /// Exception occurred when validation of the command is failed.
    /// </summary>
    public class InvalidCommandException : CommandProcessorException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class.
        /// </summary>
        public InvalidCommandException(Command command) : base(command, "Validation of the command is failed")
        {
        }
    }
}