// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TransferResult.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    using System.Diagnostics.Contracts;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Transfers execution to the supplied url.
    /// </summary>
    public class TransferResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferResult"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public TransferResult(string url)
        {
            this.Url = url;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        public string Url { get; private set; }

        /// <inheritdoc />
        public override void ExecuteResult(ControllerContext context)
        {
            Contract.Requires(context != null);

            var httpContext = HttpContext.Current;

            // MVC 3 running on IIS 7+
            if (HttpRuntime.UsingIntegratedPipeline)
            {
                httpContext.Server.TransferRequest(this.Url, true);
            }
            else
            {
                // Pre MVC 3
                httpContext.RewritePath(this.Url, false);

                IHttpHandler httpHandler = new MvcHttpHandler();
                httpHandler.ProcessRequest(httpContext);
            }
        }
    }
}