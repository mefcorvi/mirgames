namespace MirGames.Infrastructure
{
    using MirGames.Infrastructure.Commands;

    /// <summary>
    /// The command processor.
    /// </summary>
    public interface ICommandProcessor
    {
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        void Execute(Command command);

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>The result.</returns>
        T Execute<T>(Command<T> command);

        /// <summary>
        /// Executes the with result.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The result.</returns>
        object ExecuteWithResult(Command command);
    }
}