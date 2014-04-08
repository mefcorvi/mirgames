namespace MirGames.Infrastructure
{
    /// <summary>
    /// Returns the host address of the current client.
    /// </summary>
    public interface IClientHostAddressProvider
    {
        /// <summary>
        /// Gets the get host address.
        /// </summary>
        string GetHostAddress();
    }
}
