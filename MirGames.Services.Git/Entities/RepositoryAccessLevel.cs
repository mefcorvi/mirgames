namespace MirGames.Services.Git.Entities
{
    public enum RepositoryAccessLevel
    {
        /// <summary>
        /// The unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The owner.
        /// </summary>
        Owner = 1,

        /// <summary>
        /// The contributor.
        /// </summary>
        Contributor = 2,

        /// <summary>
        /// The reader.
        /// </summary>
        Reader = 3
    }
}