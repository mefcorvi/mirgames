// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EventLog.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Logging
{
    using System;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// The event log.
    /// </summary>
    internal sealed class EventLog : IEventLog
    {
        /// <summary>
        /// My trace source.
        /// </summary>
        private static readonly TraceSource TraceSource = new TraceSource("EventLog", SourceLevels.All);

        /// <summary>
        /// The claims principal provider.
        /// </summary>
        private readonly Func<ClaimsPrincipal> claimsPrincipalProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLog" /> class.
        /// </summary>
        /// <param name="claimsPrincipalProvider">The claims principal provider.</param>
        public EventLog(Func<ClaimsPrincipal> claimsPrincipalProvider)
        {
            Contract.Requires(claimsPrincipalProvider != null);

            this.claimsPrincipalProvider = claimsPrincipalProvider;
        }

        /// <inheritdoc />
        public void Log(EventLogType eventLogType, string source, string message, object details)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(message));

            string detailsString = this.GetStringRepresentation(details);

            var principal = this.claimsPrincipalProvider.Invoke();
            string login = principal.Identity.Name;

            Task.Factory.StartNew(
                () => this.LogEventSync(eventLogType, source, message, login, detailsString));
        }

        /// <summary>
        /// Logs the event synchronous.
        /// </summary>
        /// <param name="eventLogType">Type of the event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="login">The login.</param>
        /// <param name="detailsString">The details string.</param>
        private void LogEventSync(EventLogType eventLogType, string source, string message, string login, string detailsString)
        {
            try
            {
                this.LogToDatabase(eventLogType, source, message, login, detailsString);
            }
            catch (Exception e)
            {
                TraceSource.TraceEvent(
                    TraceEventType.Error,
                    1,
                    "Exception occured during saving a log message to the database: {0}. Original log message: [{1}] {2} - {3}",
                    e,
                    eventLogType,
                    login,
                    message);
            }
        }

        /// <summary>
        /// Gets the string representation.
        /// </summary>
        /// <param name="details">The details.</param>
        /// <returns>The string representation.</returns>
        private string GetStringRepresentation(object details)
        {
            try
            {
                return JsonConvert.SerializeObject(details, new JsonSerializerSettings { Error = (sender, args) => args.ErrorContext.Handled = true });
            }
            catch (Exception)
            {
                return details.ToString();
            }
        }

        /// <summary>
        /// Save the log message to the database.
        /// </summary>
        /// <param name="eventLogType">Type of the event log.</param>
        /// <param name="source">The source.</param>
        /// <param name="message">The message.</param>
        /// <param name="login">The login.</param>
        /// <param name="details">The details.</param>
        private void LogToDatabase(EventLogType eventLogType, string source, string message, string login, string details)
        {
            using (var context = new DbContext("Name=MirGames"))
            {
                context.Database.ExecuteSqlCommand(
                    "INSERT INTO eventLog (date, login, eventType, source, message, details) VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
                    DateTime.UtcNow,
                    login,
                    (int)eventLogType,
                    source,
                    message,
                    details);
            }
        }
    }
}