// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AppController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Web.Mvc;

    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// The base application controller.
    /// </summary>
    public abstract partial class AppController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        protected AppController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
        {
            this.CommandProcessor = commandProcessor;
            this.QueryProcessor = queryProcessor;
            this.CurrentUser = null;
        }

        /// <summary>
        /// Gets the page data.
        /// </summary>
        public Dictionary<string, object> PageData
        {
            get { return this.ViewBag.PageData; }
        } 

        /// <summary>
        /// Gets the command processor.
        /// </summary>
        protected ICommandProcessor CommandProcessor { get; private set; }

        /// <summary>
        /// Gets the query processor.
        /// </summary>
        protected IQueryProcessor QueryProcessor { get; private set; }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        protected CurrentUserViewModel CurrentUser { get; private set; }

        /// <inheritdoc />
        protected new ClaimsPrincipal User
        {
            get { return base.User as ClaimsPrincipal; }
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.ViewBag.PageData = new Dictionary<string, object>();

            if (this.User.Identity.IsAuthenticated)
            {
                this.CurrentUser = this.QueryProcessor.Process(new GetCurrentUserQuery());
                this.PageData["currentUser"] = this.CurrentUser;
                this.PageData["isAdmin"] = this.User.IsInRole("Administrator");
            }

            this.PageData["onlineUsers"] = this.ViewBag.OnlineUsers = this.QueryProcessor.Process(
                new GetOnlineUsersQuery(),
                new PaginationSettings(0, 20)).EnsureCollection();
            
            this.PageData["onlineUserTags"] = this.ViewBag.OnlineUserTags = this.QueryProcessor.Process(new GetOnlineUserTagsQuery());
        }

        /// <inheritdoc />
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            int forumTopicsUnread = 0;
            int blogTopicsUnread = 0;

            if (this.User.Identity.IsAuthenticated)
            {
                var forumNotificationsCountQuery = new GetForumNotificationsCountQuery();
                forumTopicsUnread = this.QueryProcessor.Process(forumNotificationsCountQuery);

                var topicsNotificationsCountQuery = new GetTopicsNotificationsCountQuery();
                blogTopicsUnread = this.QueryProcessor.Process(topicsNotificationsCountQuery);
            }

            this.ViewBag.ForumTopicsUnread = forumTopicsUnread;
            this.PageData["forumTopicsUnreadCount"] = forumTopicsUnread;

            this.ViewBag.BlogTopicsUnread = blogTopicsUnread;
            this.PageData["blogTopicsUnreadCount"] = blogTopicsUnread;

            this.ViewBag.User = this.CurrentUser;
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Gets the absolute URI.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <returns>The absolute URI.</returns>
        protected Uri GetAbsoluteUri(string relativeUrl)
        {
            var url = this.Request.Url;
            if (url != null)
            {
                var urlBuilder = new UriBuilder(url.AbsoluteUri) { Path = relativeUrl };
                return urlBuilder.Uri;
            }

            return null;
        }
    }
}
