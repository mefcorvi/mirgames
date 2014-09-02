// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RouteInfo.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace System.Web.Mvc
{
    using System.IO;
    using System.Web.Routing;

    /// <summary>
    /// The route info.
    /// </summary>
    public class RouteInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RouteInfo"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public RouteInfo(RouteData data)
        {
            this.RouteData = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteInfo"/> class.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public RouteInfo(Uri uri)
        {
            var request = new HttpRequest(null, uri.AbsoluteUri, uri.Query.Trim('?'));
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);
            this.RouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));
        }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        public RouteData RouteData { get; private set; }
    }
}