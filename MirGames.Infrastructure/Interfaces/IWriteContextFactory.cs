namespace MirGames.Infrastructure
{
    /// <summary>
    /// Responses for creating the new write context instances.
    /// </summary>
    public interface IWriteContextFactory
    {
        /// <summary>
        /// Creates the new write context.
        /// </summary>
        /// <returns>The write context.</returns>
        IWriteContext Create();
    }
}