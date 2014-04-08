namespace MirGames.Domain.Wip.Exceptions
{
    using System;

    /// <summary>
    /// Exception occured when project have been already created.
    /// </summary>
    public sealed class ProjectAlreadyCreatedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectAlreadyCreatedException"/> class.
        /// </summary>
        /// <param name="projectAlias">The project alias.</param>
        public ProjectAlreadyCreatedException(string projectAlias)
        {
            this.ProjectAlias = projectAlias;
        }

        /// <summary>
        /// Gets or sets the project alias.
        /// </summary>
        public string ProjectAlias { get; set; }
    }
}