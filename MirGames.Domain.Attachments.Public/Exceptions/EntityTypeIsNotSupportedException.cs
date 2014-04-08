namespace MirGames.Domain.Attachments.Exceptions
{
    using System;

    /// <summary>
    /// Thrown when entity type is not supported.
    /// </summary>
    public class EntityTypeIsNotSupportedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeIsNotSupportedException"/> class.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        public EntityTypeIsNotSupportedException(string entityType) : base(string.Format("Entity type {0} is not supported", entityType))
        {
        }
    }
}