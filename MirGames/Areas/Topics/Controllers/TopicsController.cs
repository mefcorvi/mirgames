// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TopicsController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Areas.Topics.Controllers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Web.Mvc;

    using MirGames.Controllers;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Domain.Topics.Queries;
    using MirGames.Domain.Topics.ViewModels;
    using MirGames.Domain.Users.ViewModels;
    using MirGames.Filters;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;
    using MirGames.Models;

    /// <summary>
    /// The topics controller.
    /// </summary>
    public partial class TopicsController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicsController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public TopicsController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <summary>
        /// Returns RSS feed of the topics list.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="searchString">The search string.</param>
        /// <returns>The feed.</returns>
        public virtual RssActionResult Rss(string tag = null, string searchString = null)
        {
            var topicsQuery = new GetTopicsQuery { IsPublished = true, Tag = tag, SearchString = searchString };
            var topics = this.QueryProcessor.Process(topicsQuery, new PaginationSettings(0, 20));

            var feed = new SyndicationFeed(
                "MirGames.ru", "Новые посты на MirGames.ru", this.GetAbsoluteUri(this.Url.Action(MVC.Topics.Topics.Index())))
                {
                    Items = topics.Select(this.CreateTopicSyndicationItem).ToList()
                };

            return new RssActionResult(feed);
        }

        /// <summary>
        /// Returns RSS feed of the topics list.
        /// </summary>
        /// <returns>The feed.</returns>
        public virtual RssActionResult CommentsRss()
        {
            var commentsQuery = new GetCommentsQuery();
            var comments = this.QueryProcessor.Process(commentsQuery, new PaginationSettings(0, 20));

            var feed = new SyndicationFeed(
                "Новые комментарии на MirGames.ru", "Новые комментарии на MirGames.ru", this.GetAbsoluteUri(this.Url.Action(MVC.Topics.Topics.Index())))
            {
                Items = comments.Select(this.CreateCommentSyndicationItem).ToList()
            };

            return new RssActionResult(feed);
        }

        /// <summary>
        /// The index action.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="searchString">The search string.</param>
        /// <param name="page">The page.</param>
        /// <param name="onlyUnread">if set to <c>true</c> only unread topics will be returned.</param>
        /// <returns>The action result.</returns>
        public virtual ActionResult Index(string tag = null, string searchString = null, int page = 1, bool onlyUnread = false)
        {
            if (page < 1)
            {
                page = 1;
            }

            var paginationSettings = new PaginationSettings(page - 1, 20);
            var topicsQuery = new GetTopicsQuery { IsPublished = true, Tag = tag, SearchString = searchString, OnlyUnread = onlyUnread };
            var topics = this.QueryProcessor.Process(topicsQuery, paginationSettings);
            var topicsCount = this.QueryProcessor.GetItemsCount(topicsQuery);
            
            var tags = this.QueryProcessor.Process(new GetMainTagsQuery());
            this.ViewBag.Tags = tags;
            this.ViewBag.Tag = tag;
            this.ViewBag.TopicsCount = topicsCount;
            this.ViewBag.RssUrl = this.Url.Action("Rss", "Topics", new { tag, searchString });
            this.ViewBag.Pagination = new PaginationViewModel(
                paginationSettings, topicsCount, p => this.Url.Action("Index", "Topics", new { tag, searchString, page = p }));

            this.ViewBag.PageData["tag"] = tag;
            this.ViewBag.PageData["searchString"] = searchString;

            return this.View(topics);
        }

        /// <summary>
        /// Shows the topic.
        /// </summary>
        /// <param name="topicId">The topic id.</param>
        /// <returns>The action result.</returns>
        public virtual ActionResult Topic(int topicId)
        {
            var topic = this.QueryProcessor.Process(new GetTopicQuery { TopicId = topicId });

            if (topic == null)
            {
                return this.HttpNotFound();
            }

            this.ViewBag.BackUrl = this.HttpContext.Request.UrlReferrer != null
                                   && this.HttpContext.Request.UrlReferrer.IsRouteMatch("Topics", "Index")
                                       ? this.HttpContext.Request.UrlReferrer.ToString()
                                       : this.Url.Action(MVC.Topics.Topics.Index());

            this.ViewBag.PageData["topicId"] = topicId;
            return this.View(topic);
        }

        /// <summary>
        /// Creates the topic.
        /// </summary>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User")]
        public virtual ActionResult New()
        {
            return this.View();
        }

        /// <summary>
        /// Edits the specified topic with the unique identifier.
        /// </summary>
        /// <param name="topicId">The topic unique identifier.</param>
        /// <returns>The action result.</returns>
        [Authorize(Roles = "User")]
        public virtual ActionResult Edit(int topicId)
        {
            var topic = this.QueryProcessor.Process(new GetTopicForEditQuery { TopicId = topicId });

            this.ViewBag.PageData["text"] = topic.Text;
            this.ViewBag.PageData["tags"] = topic.Tags;
            this.ViewBag.PageData["title"] = topic.Title;
            this.ViewBag.PageData["topicId"] = topic.Id;

            return this.View(topic);
        }

        /// <summary>
        /// Adds the topic.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public virtual ActionResult AddTopic(AddNewTopicCommand command)
        {
            Contract.Requires(command != null);
            
            var topicId = this.CommandProcessor.Execute(command);
            return this.Json(new { topicId });
        }

        /// <summary>
        /// Saves the topic.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        [Authorize(Roles = "User")]
        [ValidateInput(false)]
        public virtual ActionResult SaveTopic(SaveTopicCommand command)
        {
            Contract.Requires(command != null);

            this.CommandProcessor.Execute(command);
            return this.Json(new { result = "ok" });
        }

        /// <summary>
        /// Deletes the topic.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        [Authorize(Roles = "User")]
        public virtual ActionResult DeleteTopic(DeleteTopicCommand command)
        {
            Contract.Requires(command != null);

            this.CommandProcessor.Execute(command);
            return this.Json(new { result = "ok" });
        }

        /// <inheritdoc />
        [AjaxOnly]
        public virtual ActionResult EditCommentDialog()
        {
            return this.PartialView("_EditCommentDialog");
        }

        /// <inheritdoc />
        [AjaxOnly]
        public virtual ActionResult DeleteCommentDialog()
        {
            return this.PartialView("_DeleteCommentDialog");
        }

        /// <inheritdoc />
        [AjaxOnly]
        public virtual ActionResult AddTopicDialog()
        {
            return this.PartialView(MVC.Topics.Topics.Views._AddTopicDialog);
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "Topics";
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Creates the topic syndication item.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <returns>The syndication item.</returns>
        private SyndicationItem CreateTopicSyndicationItem(TopicsListItem topic)
        {
            var topicUrl = this.Url.Action("Topic", "Topics", new { topicId = topic.TopicId });

            var item = new SyndicationItem(
                topic.Title,
                topic.ShortText,
                this.GetAbsoluteUri(topicUrl),
                "Topic" + topic.TopicId,
                topic.CreationDate)
                {
                    PublishDate = topic.CreationDate
                };

            topic.TagsSet.Select(t => new SyndicationCategory(t)).ForEach(t => item.Categories.Add(t));
            item.Authors.Add(this.GetSyndicationPerson(topic.Author));

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
        /// Creates the topic syndication item.
        /// </summary>
        /// <param name="comment">The comment.</param>
        /// <returns>The syndication item.</returns>
        private SyndicationItem CreateCommentSyndicationItem(CommentViewModel comment)
        {
            var topicUrl = this.Url.Action("Topic", "Topics", new { topicId = comment.TopicId }) + "#c" + comment.Id;

            var item = new SyndicationItem(
                string.Format("{0} > {1} (#{2})", comment.TopicTitle, comment.Author.Login, comment.Id),
                comment.Text,
                this.GetAbsoluteUri(topicUrl),
                "Comment" + comment.Id,
                comment.CreationDate)
                {
                    PublishDate = comment.CreationDate
                };

            item.Authors.Add(this.GetSyndicationPerson(comment.Author));

            return item;
        }
    }
}
