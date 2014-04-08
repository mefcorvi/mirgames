namespace MirGames.Services.Git.Public.Services
{
    using System.IO;

    public interface IGitService
    {
        /// <summary>
        /// Receives the pack.
        /// </summary>
        /// <param name="repositoryName">The repository identifier.</param>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        void ReceivePack(string repositoryName, Stream input, Stream output);

        /// <summary>
        /// Uploads the pack.
        /// </summary>
        /// <param name="repositoryName">The repository identifier.</param>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        void UploadPack(string repositoryName, Stream input, Stream output);

        /// <summary>
        /// Gets the information refs.
        /// </summary>
        /// <param name="repositoryName">The repository identifier.</param>
        /// <param name="service">The service.</param>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        void GetInfoRefs(string repositoryName, string service, Stream input, Stream output);
    }
}
