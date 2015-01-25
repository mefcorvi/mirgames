// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ForumController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Areas.Forum.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;

    using MirGames.Controllers;
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
    public partial class ForumController : AppController
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

        /// <inheritdoc />
        public virtual ActionResult Index()
        {
            var forums = this.QueryProcessor.Process(new GetForumsQuery()).ToList();
            this.ViewBag.Subsection = "Forums";

            return this.View(forums.Where(f => f.IsRetired == false));
        }

        /// <inheritdoc />
        public virtual ActionResult Unread(string tag = null, string searchString = null, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var topicsQuery = new GetForumTopicsQuery
            {
                Tag = tag,
                SearchString = searchString,
                OnlyUnread = true
            };

            var paginationSettings = new PaginationSettings(page - 1, 20);

            var topics = this.QueryProcessor.Process(topicsQuery, paginationSettings);
            var topicsCount = this.QueryProcessor.GetItemsCount(topicsQuery);

            var topicsPagination = new Dictionary<int, PaginationViewModel>();
            foreach (var topic in topics)
            {
                int topicId = topic.TopicId;
                string alias = topic.Forum.Alias;
                topicsPagination[topicId] =
                    new PaginationViewModel(
                        new PaginationSettings(PaginationSettings.GetItemPage(topic.PostsCount, 20), 20),
                        topic.PostsCount,
                        p => this.GetTopicPageUrl(p, topicId, alias))
                    {
                        ShowPrevNextNavigation = false,
                        HightlightCurrentPage = false
                    };
            }

            this.ViewBag.RssUrl = this.Url.ActionCached(MVC.Forum.Forum.Rss());
            this.ViewBag.Pagination = new PaginationViewModel(
                 paginationSettings, topicsCount, p => this.Url.ActionCached(MVC.Forum.Forum.Unread(tag, searchString, page: p)));
            this.ViewBag.TopicsPagination = topicsPagination;
            this.ViewBag.Subsection = "Unread";

            this.ViewBag.PageData["tag"] = tag;
            this.ViewBag.PageData["searchString"] = searchString;

            this.ViewBag.TopicsCount = topicsCount;

            return this.View(topics);
        }

        /// <inheritdoc />
        public virtual ActionResult Topics(string forumAlias, string tag = null, string searchString = null, int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var topicsQuery = new GetForumTopicsQuery
                {
                    Tag = tag,
                    SearchString = searchString,
                    OnlyUnread = false,
                    ForumAlias = forumAlias
                };

            var paginationSettings = new PaginationSettings(page - 1, 20);

            var topics = this.QueryProcessor.Process(topicsQuery, paginationSettings);
            var topicsCount = this.QueryProcessor.GetItemsCount(topicsQuery);

            var topicsPagination = new Dictionary<int, PaginationViewModel>();
            foreach (var topic in topics)
            {
                int topicId = topic.TopicId;
                string topicForumAlias = topic.Forum.Alias;

                topicsPagination[topicId] =
                    new PaginationViewModel(
                        new PaginationSettings(PaginationSettings.GetItemPage(topic.PostsCount, 20), 20),
                        topic.PostsCount,
                        p => this.GetTopicPageUrl(p, topicId, topicForumAlias))
                        {
                            ShowPrevNextNavigation = false,
                            HightlightCurrentPage = false
                        };
            }

            this.ViewBag.RssUrl = this.Url.ActionCached(MVC.Forum.Forum.Rss());
            this.ViewBag.Pagination = new PaginationViewModel(
                 paginationSettings, topicsCount, p => this.Url.ActionCached(MVC.Forum.Forum.Topics(forumAlias, tag, searchString, page: p)));
            this.ViewBag.TopicsPagination = topicsPagination;
            this.ViewBag.SearchString = searchString;
            this.ViewBag.Subsection = "Topics";
            
            this.ViewBag.PageData["tag"] = tag;
            this.ViewBag.PageData["searchString"] = searchString;
            this.ViewBag.PageData["forumAlias"] = forumAlias;

            if (!forumAlias.IsNullOrEmpty())
            {
                var forum = this.QueryProcessor.Process(new GetForumsQuery()).FirstOrDefault(f => f.Alias.EqualsIgnoreCase(forumAlias));
                this.ViewBag.Forum = forum;
            }

            this.ViewBag.TopicsCount = topicsCount;

            return this.View(topics);
        }

        /// <inheritdoc />
        public virtual ActionResult Rss(string tag = null, string searchString = null)
        {
            var topicsQuery = new GetForumPostsQuery();

            var posts = this.QueryProcessor.Process(topicsQuery, new PaginationSettings(0, 20));
            var feed = new SyndicationFeed(
                "Новые сообщения на форуме MirGames.ru", "Новые сообщения", this.GetAbsoluteUri(this.Url.ActionCached(MVC.Forum.Forum.Topics())))
            {
                Items = posts.Select(this.CreateTopicSyndicationItem).ToList()
            };

            return new RssActionResult(feed);
        }

        /// <inheritdoc />
        [Authorize(Roles = "User")]
        public virtual ActionResult New(string forumAlias)
        {
            var forums = this.QueryProcessor.Process(new GetForumsQuery()).ToList();
            var forum = forums.FirstOrDefault(f => f.Alias.EqualsIgnoreCase(forumAlias));

            if (forum == null)
            {
                return this.HttpNotFound();
            }

            this.PageData["forumAlias"] = forum.Alias;
            this.ViewBag.Forum = forum;
            this.ViewBag.Subsection = "New";
            return this.View();
        }

        /// <inheritdoc />
        public virtual ActionResult Topic(string forumAlias, int topicId, int page = 1, int? postId = null)
        {
            var topic = this.QueryProcessor.Process(new GetForumTopicQuery { TopicId = topicId });

            if (topic == null)
            {
                return this.HttpNotFound();
            }

            if (forumAlias == null)
            {
                return this.RedirectPermanent(Url.ActionCached(MVC.Forum.Forum.Topic(topic.Forum.Alias, topicId, page)));
            }

            var postsQuery = new GetForumTopicPostsQuery { TopicId = topicId };
            var postsCount = this.QueryProcessor.GetItemsCount(postsQuery);

            var pageSize = 20;

            int pageIdx = page - 1;
            if (page < 1)
            {
                pageIdx = PaginationSettings.GetItemPage(postsCount, pageSize);
            }

            if (postId.HasValue)
            {
                var forumPost = this.QueryProcessor.Process(new GetForumPostQuery { PostId = postId.Value });
                pageIdx = PaginationSettings.GetItemPage(forumPost.Index, pageSize);
            }

            var pagination = new PaginationSettings(pageIdx, pageSize);
            var posts = this.QueryProcessor.Process(postsQuery, pagination);

            if (this.User.IsInRole("User") && !topic.IsRead)
            {
                this.CommandProcessor.Execute(new MarkTopicAsVisitedCommand { TopicId = topic.TopicId });
            }

            this.ViewBag.Pagination = new PaginationViewModel(pagination, postsCount, p => this.GetTopicPageUrl(p, topicId, forumAlias));
            this.ViewBag.Posts = posts;
            this.ViewBag.PageData["topicId"] = topicId;
            this.ViewBag.PageData["pagesCount"] = this.ViewBag.Pagination.PagesCount;
            this.ViewBag.PageData["page"] = pageIdx + 1;
            this.ViewBag.PageData["pageSize"] = pageSize;

            return this.View(topic);
        }

        /// <inheritdoc />
        public virtual ActionResult EditPostDialog()
        {
            return this.PartialView("_EditPostDialog");
        }

        /// <inheritdoc />
        public virtual ActionResult DeletePostDialog()
        {
            return this.PartialView("_DeletePostDialog");
        }

        /// <inheritdoc />
        public virtual ActionResult DeleteTopicDialog()
        {
            return this.PartialView("_DeleteTopicDialog");
        }

        /// <inheritdoc />
        [Authorize(Roles = "User")]
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        public virtual JsonResult MarkAllTopicsAsRead()
        {
            this.CommandProcessor.Execute(new MarkAllTopicsAsReadCommand());
            return this.Json(new { result = true });
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
            var topicUrl = this.Url.ActionCached(MVC.Forum.Forum.Topic(post.ForumAlias, post.TopicId));

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
                this.Url.ActionCached(MVC.Users.Profile(author.Id.GetValueOrDefault())));
        }

        /// <summary>
        /// Gets the topic page URL.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="topicId">The topic unique identifier.</param>
        /// <param name="forumAlias">The forum alias.</param>
        /// <returns>
        /// The topic page URL.
        /// </returns>
        private string GetTopicPageUrl(int page, int topicId, string forumAlias)
        {
            return this.Url.ActionCached(MVC.Forum.Forum.Topic(forumAlias, topicId, page)) + "#posts";
        }
    }
}
