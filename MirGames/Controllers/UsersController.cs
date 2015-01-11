// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UsersController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;

    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Notifications.Queries;
    using MirGames.Domain.Notifications.ViewModels;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.Commands;
    using MirGames.Domain.Users.Queries;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Filters;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The topics controller.
    /// </summary>
    public partial class UsersController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public UsersController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public virtual ActionResult Index(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.LastVisit,
                SearchString = searchString
            };

            this.PageData["action"] = "Index";

            return this.ShowUsersList(usersQuery, "Active", "Активные пользователи", page);
        }

        /// <inheritdoc />
        public virtual ActionResult Top(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.Rating,
                SearchString = searchString
            };

            this.PageData["action"] = "Top";

            return this.ShowUsersList(usersQuery, "Top", "Рейтинг пользователей", page);
        }

        /// <inheritdoc />
        [Authorize(Roles = "Administrator")]
        public virtual ActionResult NotActivated(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.LastVisit,
                Filter = GetUsersQuery.UserTypes.NotActivated,
                SearchString = searchString
            };

            this.PageData["action"] = "NotActivated";

            return this.ShowUsersList(usersQuery, "NotActivated", "Неактивированные пользователи", page);
        }

        /// <inheritdoc />
        public virtual ActionResult Online(int page = 1, string searchString = null)
        {
            var usersQuery = new GetUsersQuery
            {
                SortBy = GetUsersQuery.SortType.Login,
                Filter = GetUsersQuery.UserTypes.Activated | GetUsersQuery.UserTypes.Online,
                SearchString = searchString
            };

            this.PageData["action"] = "Online";

            return this.ShowUsersList(usersQuery, "Online", "Пользователи онлайн", page);
        }

        /// <summary>
        /// The profile action.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>The action result.</returns>
        public virtual new ActionResult Profile(int userId)
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });

            if (user == null)
            {
                return this.HttpNotFound();
            }

            var profileViewModel = new ProfileViewModel { User = user };
            var userBlog = this.QueryProcessor.Process(new GetBlogByEntityQuery { EntityId = userId, EntityType = "User" });

            if (userBlog != null)
            {
                profileViewModel.Blog = userBlog;
                profileViewModel.Topics = this.QueryProcessor.Process(new GetTopicsQuery { BlogId = userBlog.BlogId }, new PaginationSettings(0, 20));
            }

            this.PageData["userId"] = userId;
            this.PageData["blogId"] = profileViewModel.Blog.BlogId;
            this.ViewBag.SectionMode = "Profile";

            if (this.CurrentUser != null && userId == this.CurrentUser.Id)
            {
                this.ViewBag.CurrentSection = "CurrentUser";
            }

            return this.View(
                "Profile",
                profileViewModel);
        }

        /// <inheritdoc />
        public virtual ActionResult Topics(int userId, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            
            var query = new GetTopicsByUserQuery { UserId = userId };
            var topics = this.QueryProcessor.Process(query, paginationSettings);
            var topicsCount = this.QueryProcessor.GetItemsCount(query);

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Topics";

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                topicsCount,
                p => this.Url.ActionCached(MVC.Users.Topics(userId, p)));

            if (this.CurrentUser != null && userId == this.CurrentUser.Id)
            {
                this.ViewBag.CurrentSection = "CurrentUser";
            }

            return this.View(
                "Topics",
                new TopicsPageViewModel { User = user, Topics = topics });
        }

        /// <inheritdoc />
        public virtual ActionResult Comments(int userId, int page = 1)
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var query = new GetCommentsQuery { AuthorId = userId };
            var comments = this.QueryProcessor.Process(query, paginationSettings);
            var commentsCount = this.QueryProcessor.GetItemsCount(query);

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                commentsCount,
                p => this.Url.ActionCached(MVC.Users.Comments(userId, p)));

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Comments";

            if (this.CurrentUser != null && userId == this.CurrentUser.Id)
            {
                this.ViewBag.CurrentSection = "CurrentUser";
            }

            return this.View(
                "Comments",
                new CommentsPageViewModel { User = user, Comments = comments });
        }

        /// <inheritdoc />
        public virtual ActionResult Forum(int userId, int page = 1)
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = userId });
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var query = new GetForumPostsQuery { AuthorId = userId };
            var posts = this.QueryProcessor.Process(query, paginationSettings);
            var postsCount = this.QueryProcessor.GetItemsCount(query);

            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                postsCount,
                p => this.Url.ActionCached(MVC.Users.Forum(userId, p)));

            this.PageData["userId"] = userId;
            this.ViewBag.SectionMode = "Forum";

            if (this.CurrentUser != null && userId == this.CurrentUser.Id)
            {
                this.ViewBag.CurrentSection = "CurrentUser";
            }

            return this.View(
                "ForumPosts",
                new ForumPageViewModel { User = user, Posts = posts });
        }

        /// <summary>
        /// The profile action.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User, ReadOnlyUser")]
        public virtual ActionResult Settings()
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = this.CurrentUser.Id });
            var authProviders = this.QueryProcessor.Process(new GetOAuthProvidersQuery());

            this.ViewBag.SectionMode = "Settings";
            this.ViewBag.CurrentSection = "CurrentUser";
            this.PageData["timeZone"] = this.CurrentUser.TimeZone;
            this.PageData["user"] = user;
            this.PageData["oauthProviders"] = authProviders;

            return this.View(user);
        }

        /// <summary>
        /// Shows the user feed.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User, ReadOnlyUser")]
        public virtual ActionResult Feed()
        {
            var user = this.QueryProcessor.Process(new GetUserByIdQuery { UserId = this.CurrentUser.Id });
            this.ViewBag.UserModel = user;

            var notifications = this.QueryProcessor.Process(new GetNotificationsQuery());

            var result = this.QueryProcessor
                             .Process(new GetNotificationDetailsQuery
                             {
                                 Notifications = notifications.Select(n => n.Data).ToArray()
                             })
                             .OrderByDescending(n => n.NotificationDate)
                             .ToList();

            this.ViewBag.SectionMode = "Feed";
            this.ViewBag.CurrentSection = "CurrentUser";

            return this.View(result);
        }

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public virtual ActionResult DeleteUser(DeleteUserCommand command)
        {
            Contract.Requires(command != null);

            this.CommandProcessor.Execute(command);
            return this.Json(new { result = "ok" });
        }

        /// <summary>
        /// Posts new wall record.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public virtual ActionResult NewWallRecord(PostWallRecordCommand command)
        {
            Contract.Requires(command != null);

            var recordId = this.CommandProcessor.Execute(command);
            var wallRecord = this.QueryProcessor.Process(new GetWallRecordByIdQuery { WallRecordId = recordId });
            return this.PartialView("_WallRecord", wallRecord);
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "Users";
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Shows the users list.
        /// </summary>
        /// <param name="usersQuery">The users query.</param>
        /// <param name="sectionMode">The section mode.</param>
        /// <param name="sectionTitle">The section title.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// The users list.
        /// </returns>
        private ActionResult ShowUsersList(GetUsersQuery usersQuery, string sectionMode, string sectionTitle, int page)
        {
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var users = this.QueryProcessor.Process(usersQuery, paginationSettings);
            var usersCount = this.QueryProcessor.GetItemsCount(usersQuery);

            this.ViewBag.UsersCount = usersCount;
            this.ViewBag.SectionMode = sectionMode;
            this.ViewBag.SectionTitle = sectionTitle;
            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings,
                usersCount,
                p => this.Url.ActionCached(MVC.Users.Index(p, usersQuery.SearchString)));

            this.PageData["searchString"] = usersQuery.SearchString;

            return this.View("Index", users);
        }

        /// <summary>
        /// The profile view model.
        /// </summary>
        public class ProfileViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the topics.
            /// </summary>
            public IEnumerable<TopicsListItem> Topics { get; set; }

            /// <summary>
            /// Gets or sets the blog.
            /// </summary>
            public BlogViewModel Blog { get; set; }
        }

        /// <summary>
        /// The topics page view model.
        /// </summary>
        public class TopicsPageViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the topics.
            /// </summary>
            public IEnumerable<TopicsListItem> Topics { get; set; }
        }

        /// <summary>
        /// The topics page view model.
        /// </summary>
        public class CommentsPageViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the comments.
            /// </summary>
            public IEnumerable<CommentViewModel> Comments { get; set; }
        }

        /// <summary>
        /// The topics page view model.
        /// </summary>
        public class ForumPageViewModel
        {
            /// <summary>
            /// Gets or sets the user.
            /// </summary>
            public UserViewModel User { get; set; }

            /// <summary>
            /// Gets or sets the comments.
            /// </summary>
            public IEnumerable<ForumPostViewModel> Posts { get; set; }
        }
    }
}
