namespace MirGames.Domain.Tools.Queries
{
    using System;

    using MirGames.Domain.Tools.ViewModels;
    using MirGames.Infrastructure.Commands;
    using MirGames.Infrastructure.Logging;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// Returns topic by identifier.
    /// </summary>
    [Api]
    public class GetEventLogQuery : Query<EventLogViewModel>
    {
        /// <summary>
        /// Gets or sets the type of the log.
        /// </summary>
        public EventLogType? LogType { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets the to date.
        /// </summary>
        public DateTime? To { get; set; }
    }
}