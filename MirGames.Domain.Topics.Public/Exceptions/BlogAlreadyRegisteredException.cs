namespace MirGames.Domain.Topics.Exceptions
{
    using System;

    public sealed class BlogAlreadyRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogAlreadyRegisteredException"/> class.
        /// </summary>
        /// <param name="projectAlias">The project alias.</param>
        public BlogAlreadyRegisteredException(string projectAlias)
        {
            this.ProjectAlias = projectAlias;
        }

        /// <summary>
        /// Gets or sets the project alias.
        /// </summary>
        public string ProjectAlias { get; set; }
    }
}