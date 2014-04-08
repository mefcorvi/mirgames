namespace MirGames.Domain.Exceptions
{
    using System;

    /// <summary>
    /// Exception raised when item have not been found.
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="itemId">The topic id.</param>
        public ItemNotFoundException(string type, object itemId) : base("Item " + type + "#" + itemId + " have not been found")
        {
        }
    }
}