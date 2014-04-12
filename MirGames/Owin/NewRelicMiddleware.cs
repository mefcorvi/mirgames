// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="NewRelicMiddleware.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Owin
{
    using System.Threading.Tasks;

    using Microsoft.Owin;

    /// <summary>
    /// The new relic middleware.
    /// </summary>
    public class NewRelicMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewRelicMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware.</param>
        public NewRelicMiddleware(OwinMiddleware next) : base(next)
        {
        }

        /// <inheritdoc />
        public override async Task Invoke(IOwinContext context)
        {
            if (context.Request.Path.HasValue && context.Request.Path.Value.Contains("signalr"))
            {
                NewRelic.Api.Agent.NewRelic.IgnoreTransaction();
            }

            await this.Next.Invoke(context);
        }
    }
}