namespace MirGames.Infrastructure
{
    /// <summary>
    /// Responses for creating the new read context instances.
    /// </summary>
    public interface IReadContextFactory
    {
        /// <summary>
        /// Creates the new read context.
        /// </summary>
        /// <returns>The read context.</returns>
        IReadContext Create();
    }
}