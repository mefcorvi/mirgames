namespace MirGames.Domain.Attachments.Services
{
    /// <summary>
    /// Processes the uploaded file.
    /// </summary>
    public interface IUploadProcessor
    {
        /// <summary>
        /// Determines whether this instance can process the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>True whether this instance can process the specified entity type.</returns>
        bool CanProcess(string entityType);

        /// <summary>
        /// Determines whether the specified file is valid.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>True whether the specified file is valid.</returns>
        bool IsValid(string filePath);

        /// <summary>
        /// Processes the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void Process(string filePath);
    }
}
