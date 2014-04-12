// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="WebApiConfig.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Web.Http;

namespace MirGames
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(name: "QueryAllApi", routeTemplate: "api/all", defaults: new { controller = "WebApi", action = "GetAll" });
            config.Routes.MapHttpRoute(name: "QueryOneApi", routeTemplate: "api/one", defaults: new { controller = "WebApi", action = "GetOne" });
            config.Routes.MapHttpRoute(name: "CommandApi", routeTemplate: "api", defaults: new { controller = "WebApi", action = "Post" });
        }
    }
}
