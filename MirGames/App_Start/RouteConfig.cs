// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RouteConfig.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    /// <summary>
    /// The route config.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the routes.
        /// </summary>
        /// <param name="routes">The routes.</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "SecureInfoRefs",
                "git/{project}.git/info/refs",
                new { controller = "Git", action = "GetInfoRefs" });

            routes.MapRoute(
                "SecureUploadPack",
                "git/{project}.git/git-upload-pack",
                new { controller = "Git", action = "UploadPack" });

            routes.MapRoute(
                "SecureReceivePack",
                "git/{project}.git/git-receive-pack",
                new { controller = "Git", action = "ReceivePack" });

            routes.MapRoute(
                "AboutPage",
                "about",
                new { controller = "Pages", action = "About" });

            routes.MapRoute(
                "RulesPage",
                "rules",
                new { controller = "Pages", action = "Rules" });

            routes.MapRoute(
                "HelpPage",
                "help",
                new { controller = "Pages", action = "Help" });

            routes.MapRoute(
                "MarkdownPage",
                "help/markdown",
                new { controller = "Pages", action = "Markdown" });

            routes.MapRoute(
                "ApiPage",
                "help/api",
                new { controller = "Pages", action = "Api" });

            routes.MapRoute(
                "OAuthItem",
                "auth",
                new { controller = "OAuth", action = "Index" });

            routes.MapRoute(
                "OAuthAuthorizeItem",
                "auth/{provider}",
                new { controller = "OAuth", action = "Authorize" });

            routes.MapRoute(
                "AccountSettings",
                "settings",
                new { controller = "Users", action = "Settings" });

            routes.MapRoute(
                "UsersItem",
                "users/{userId}",
                new { controller = "Users", action = "Profile" },
                new { userId = @"\d+" });

            routes.MapRoute(
                "UsersTopics",
                "users/{userId}/topics",
                new { controller = "Users", action = "Topics" },
                new { userId = @"\d+" });

            routes.MapRoute(
                "UsersComments",
                "users/{userId}/comments",
                new { controller = "Users", action = "Comments" },
                new { userId = @"\d+" });

            routes.MapRoute(
                "UsersForum",
                "users/{userId}/forum",
                new { controller = "Users", action = "Forum" },
                new { userId = @"\d+" });

            routes.MapRoute(
                "AttachmentItem",
                "attachment/{attachmentId}",
                new { controller = "Attachment", action = "Index" },
                new { attachmentId = @"\d+" });

            routes.MapRouteLowercase(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional });
        }
    }

    public class LowercaseRoute : Route
    {
        public LowercaseRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
        }

        public LowercaseRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
        }

        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
        }

        public LowercaseRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var lowercaseValues = new RouteValueDictionary(values);

            if (lowercaseValues.ContainsKey("controller"))
            {
                lowercaseValues["controller"] = lowercaseValues["controller"].ToString().ToLowerInvariant();
            }

            if (lowercaseValues.ContainsKey("action"))
            {
                lowercaseValues["action"] = lowercaseValues["action"].ToString().ToLowerInvariant();
            }

            return base.GetVirtualPath(requestContext, lowercaseValues);
        }
    }

    public static class RouteCollectionExtensions
    {
        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url)
        {
            return routes.MapRouteLowercase(name, url, null /* defaults */, (object)null /* constraints */);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapRouteLowercase(name, url, defaults, (object)null /* constraints */);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.MapRouteLowercase(name, url, defaults, constraints, null /* namespaces */);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return routes.MapRouteLowercase(name, url, null /* defaults */, null /* constraints */, namespaces);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.MapRouteLowercase(name, url, defaults, null /* constraints */, namespaces);
        }

        public static Route MapRouteLowercase(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            Route route = new LowercaseRoute(url, new MvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary()
            };

            if ((namespaces != null) && (namespaces.Length > 0))
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url)
        {
            return context.MapRouteLowercase(name, url, (object)null /* defaults */);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return context.MapRouteLowercase(name, url, defaults, (object)null /* constraints */);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            return context.MapRouteLowercase(name, url, defaults, constraints, null /* namespaces */);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, string[] namespaces)
        {
            return context.MapRouteLowercase(name, url, (object)null /* defaults */, namespaces);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces)
        {
            return context.MapRouteLowercase(name, url, defaults, null /* constraints */, namespaces);
        }

        public static Route MapRouteLowercase(this AreaRegistrationContext context, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (namespaces == null && context.Namespaces != null)
            {
                namespaces = context.Namespaces.ToArray();
            }

            Route route = context.Routes.MapRouteLowercase(name, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = context.AreaName;

            // disabling the namespace lookup fallback mechanism keeps this areas from accidentally picking up
            // controllers belonging to other areas
            bool useNamespaceFallback = (namespaces == null || namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = useNamespaceFallback;

            return route;
        }
    }
}