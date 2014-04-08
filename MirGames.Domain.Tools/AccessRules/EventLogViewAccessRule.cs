namespace MirGames.Domain.Tools.AccessRules
{
    using System.Security.Claims;

    using MirGames.Domain.Tools.Entities;
    using MirGames.Infrastructure.Security;

    /// <summary>
    /// Determines the access to view event log.
    /// </summary>
    internal sealed class EventLogViewAccessRule : AccessRule<EventLog>
    {
        /// <inheritdoc />
        protected override string Action
        {
            get { return "View"; }
        }

        /// <inheritdoc />
        protected override bool CheckAccess(ClaimsPrincipal principal, EventLog resource)
        {
            return false;
        }
    }
}
