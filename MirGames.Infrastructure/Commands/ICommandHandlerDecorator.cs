namespace MirGames.Infrastructure.Commands
{
    /// <summary>
    /// Decorates the command handler.
    /// </summary>
    public interface ICommandHandlerDecorator
    {
        /// <summary>
        /// Gets the order.
        /// </summary>
        float Order { get; }

        /// <summary>
        /// Decorates the specified query handler.
        /// </summary>
        /// <param name="queryHandler">The query handler.</param>
        /// <returns>The decorated query handler.</returns>
        ICommandHandler Decorate(ICommandHandler queryHandler);
    }
}