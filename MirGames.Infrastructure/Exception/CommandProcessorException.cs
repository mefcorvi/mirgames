namespace MirGames.Infrastructure.Exception
{
    using System;

    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// Exception thrown whether command executing have been failed.
    /// </summary>
    public class CommandProcessorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessorException" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        public CommandProcessorException(Command command)
            : this(command, "Execution of command have been failed")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessorException" /> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="message">The message.</param>
        public CommandProcessorException(Command command, string message)
            : base(message)
        {
            this.Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandProcessorException"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="innerException">The inner exception.</param>
        public CommandProcessorException(Command command, Exception innerException)
            : base("Execution of command have been failed", innerException)
        {
            this.Command = command;
        }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public Command Command { get; set; }
    }
}