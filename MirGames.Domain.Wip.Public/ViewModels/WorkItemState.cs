namespace MirGames.Domain.Wip.ViewModels
{
    /// <summary>
    /// The work item state.
    /// </summary>
    public enum WorkItemState
    {
        /// <summary>
        /// The undefined.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// The open.
        /// </summary>
        Open = 1,

        /// <summary>
        /// The closed.
        /// </summary>
        Closed = 2,

        /// <summary>
        /// The active.
        /// </summary>
        Active = 3,

        /// <summary>
        /// The queued.
        /// </summary>
        Queued = 4
    }
}