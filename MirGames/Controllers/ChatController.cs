namespace MirGames.Controllers
{
    using System.Web.Mvc;

    using MirGames.Domain.Chat.Commands;
    using MirGames.Filters;
    using MirGames.Infrastructure;

    /// <summary>
    /// The chat controller.
    /// </summary>
    public class ChatController : AppController
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
        public ActionResult Index()
        {
            return View();
        }

        /// <inheritdoc />
        [HttpPost]
        [AjaxOnly]
        [AntiForgery]
        [Authorize(Roles = "ChatMember")]
        [ValidateInput(false)]
        public ActionResult Post(PostChatMessageCommand command)
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
