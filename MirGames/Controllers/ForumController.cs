// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Forum.Queries;
    using MirGames.Domain.Forum.ViewModels;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Filters;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The forum controller.
    /// </summary>
    public class ForumController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForumController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ForumController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <summary>
        /// The index action.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="onlyUnread">if set to <c>true</c> then only unread topics will be returned.</param>
        /// <param name="page">The page.</param>
        /// <returns>
        /// The action result.
        /// </returns>
        public ActionResult Index(string tag = null, string searchString = null, bool onlyUnread = false, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var topicsQuery = new GetForumTopicsQuery
                {
                    Tag = tag,
                    SearchString = searchString,
                    OnlyUnread = onlyUnread
                };

            var paginationSettings = new PaginationSettings(page - 1, 20);

            var topics = this.QueryProcessor.Process(topicsQuery, paginationSettings);
            var topicsCount = this.QueryProcessor.GetItemsCount(topicsQuery);

            var topicsPagination = new Dictionary<int, PaginationViewModel>();
            foreach (var topic in topics)
            {
                int topicId = topic.TopicId;
                topicsPagination[topicId] =
                    new PaginationViewModel(
                        new PaginationSettings(PaginationSettings.GetItemPage(topic.PostsCount, 20), 20),
                        topic.PostsCount,
                        p => this.GetTopicPageUrl(p, topicId))
                        {
                            ShowPrevNextNavigation = false,
                            HightlightCurrentPage = false
                        };
            }

            var tags = this.QueryProcessor.Process(new GetForumTagsQuery());
            ViewBag.Tags = tags;
            ViewBag.Tag = tag;
            ViewBag.OnlyUnread = onlyUnread;
            ViewBag.RssUrl = Url.Action("Rss", "Forum");
            ViewBag.Pagination = new PaginationViewModel(
                 paginationSettings, topicsCount, p => Url.Action("Index", "Forum", new { tag, searchString, page = p }));
            ViewBag.TopicsPagination = topicsPagination;
            
            this.ViewBag.PageData["tag"] = tag;
            this.ViewBag.PageData["searchString"] = searchString;

            ViewBag.TopicsCount = topicsCount;
            ViewBag.SectionMode = "Active";

            return this.View(topics);
        }

        /// <inheritdoc />
        public ActionResult Rss(string tag = null, string searchString = null)
        {
            var topicsQuery = new GetForumPostsQuery();

            var posts = this.QueryProcessor.Process(topicsQuery, new PaginationSettings(0, 20));
            var feed = new SyndicationFeed(
                "Новые сообщения на форуме MirGames.ru", "Новые сообщения", this.GetAbsoluteUri(Url.Action("Index", "Forum")))
            {
                Items = posts.Select(this.CreateTopicSyndicationItem).ToList()
            };

            return new RssActionResult(feed);
        }

        /// <summary>
        /// Creates the topic.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User")]
        public ActionResult New()
        {
            return this.View();
        }

        /// <summary>
        /// Shows topic with the specified ID.
        /// </summary>
        /// <param name="topicId">The topic id.</param>
        /// <param name="page">The page.</param>
        /// <returns>The action result.</returns>
        public ActionResult Topic(int topicId, int page = 1)
        {
            var topic = this.QueryProcessor.Process(new GetForumTopicQuery { TopicId = topicId });

            if (topic == null)
            {
                return this.HttpNotFound();
            }

            var postsQuery = new GetForumTopicPostsQuery { TopicId = topicId };
            var postsCount = this.QueryProcessor.GetItemsCount(postsQuery);

            int pageIdx = page - 1;
            if (page < 1)
            {
                pageIdx = PaginationSettings.GetItemPage(postsCount, 20);
            }

            var pagination = new PaginationSettings(pageIdx, 20);
            var posts = this.QueryProcessor.Process(postsQuery, pagination);

            if (User.IsInRole("User") && !topic.IsRead)
            {
                this.CommandProcessor.Execute(new MarkTopicAsReadCommand { TopicId = topic.TopicId });
            }

            this.ViewBag.Pagination = new PaginationViewModel(pagination, postsCount, p => this.GetTopicPageUrl(p, topicId));
            this.ViewBag.Posts = posts;
            this.ViewBag.PageData["topicId"] = topicId;
            this.ViewBag.PageData["pagesCount"] = this.ViewBag.Pagination.PagesCount;
            this.ViewBag.PageData["page"] = page;
            this.ViewBag.PageData["pageSize"] = 20;

            return this.View(topic);
        }

        /// <inheritdoc />
        public ActionResult EditPostDialog()
        {
            return this.PartialView("_EditPostDialog");
        }

        /// <inheritdoc />
        public ActionResult DeletePostDialog()
        {
            return this.PartialView("_DeletePostDialog");
        }

        /// <inheritdoc />
        public ActionResult DeleteTopicDialog()
        {
            return this.PartialView("_DeleteTopicDialog");
        }

        /// <inheritdoc />
        [Authorize(Roles = "User")]
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public JsonResult MarkAllTopicsAsRead()
        {
            this.CommandProcessor.Execute(new MarkAllTopicsAsReadCommand());
            return Json(new { result = true });
        }

        /// <summary>
        /// Posts the new topic.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The new topic.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public ActionResult PostNewTopic(PostNewForumTopicCommand command)
        {
            var topicId = this.CommandProcessor.Execute(command);
            return this.Json(new { topicId });
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "Forum";
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Creates the topic syndication item.
        /// </summary>
        /// <param name="post">The post.</param>
        /// <returns>The syndication item.</returns>
        private SyndicationItem CreateTopicSyndicationItem(ForumPostViewModel post)
        {
            var topicUrl = this.Url.Action("Topic", "Forum", new { topicId = post.TopicId });

            var item = new SyndicationItem(
                string.Format("{0} > {1} (#{2})", post.TopicTitle, post.Author.Login, post.PostId),
                post.Text,
                this.GetAbsoluteUri(topicUrl),
                "post" + post.PostId,
                post.CreatedDate)
                {
                    PublishDate = post.CreatedDate,
                };

            item.Authors.Add(this.GetSyndicationPerson(post.Author));

            return item;
        }

        /// <summary>
        /// Gets the syndication person.
        /// </summary>
        /// <param name="author">The author.</param>
        /// <returns>The person.</returns>
        private SyndicationPerson GetSyndicationPerson(AuthorViewModel author)
        {
            return new SyndicationPerson(
                null,
                author.Login,
                this.Url.Action("Profile", "Users", new { userId = author.Id }));
        }

        /// <summary>
        /// Gets the topic page URL.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="topicId">The topic unique identifier.</param>
        /// <returns>The topic page URL.</returns>
        private string GetTopicPageUrl(int page, int topicId)
        {
            return this.Url.Action(
                "Topic",
                "Forum",
                new { page, topicId }) + "#posts";
        }
    }
}
