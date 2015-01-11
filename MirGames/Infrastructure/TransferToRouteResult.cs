// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TransferToRouteResult.cs">
// Copyright 2015 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    using System.Diagnostics.Contracts;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class TransferToRouteResult : ActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferToRouteResult"/> class.
        /// </summary>
        /// <param name="routeValues">The route values.</param>
        public TransferToRouteResult(RouteValueDictionary routeValues)
            : this(null, routeValues)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferToRouteResult"/> class.
        /// </summary>
        /// <param name="routeName">Name of the route.</param>
        /// <param name="routeValues">The route values.</param>
        public TransferToRouteResult(string routeName, RouteValueDictionary routeValues)
        {
            this.RouteName = routeName ?? string.Empty;
            this.RouteValues = routeValues ?? new RouteValueDictionary();
        }

        /// <summary>
        /// Gets or sets the name of the route.
        /// </summary>
        public string RouteName { get; set; }

        /// <summary>
        /// Gets or sets the route values.
        /// </summary>
        public RouteValueDictionary RouteValues { get; set; }

        /// <inheritdoc />
        public override void ExecuteResult(ControllerContext context)
        {
            Contract.Requires(context != null);

            var urlHelper = new UrlHelper(context.RequestContext);
            var url = urlHelper.RouteUrl(this.RouteName, this.RouteValues);

            var actualResult = new TransferResult(url);
            actualResult.ExecuteResult(context);
        }
    }
}