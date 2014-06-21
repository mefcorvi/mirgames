// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ChatController.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Controllers
{
    using System.Web.Mvc;

    using MirGames.Domain.Chat.Commands;
    using MirGames.Filters;
    using MirGames.Infrastructure;

    /// <summary>
    /// The chat controller.
    /// </summary>
    public partial class ChatController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ChatController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public virtual ActionResult Index()
        {
            return View();
        }

        /// <inheritdoc />
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        [Authorize(Roles = "ChatMember")]
        [ValidateInput(false)]
        public virtual ActionResult Post(PostChatMessageCommand command)
        {
            this.CommandProcessor.Execute(command);
            return this.Json(new { result = true });
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "Chat";
            base.OnActionExecuting(filterContext);
        }
    }
}
