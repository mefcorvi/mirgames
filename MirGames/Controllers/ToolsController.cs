namespace MirGames.Controllers
{
    using System.Web.Mvc;

    using MirGames.Domain.Forum.Commands;
    using MirGames.Domain.Tools.Queries;
    using MirGames.Domain.Topics.Commands;
    using MirGames.Infrastructure;
    using MirGames.Infrastructure.Queries;

    /// <summary>
    /// The tools controller.
    /// </summary>
    public class ToolsController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToolsController" /> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ToolsController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        [Authorize(Roles = "Administrator")]
        public ActionResult ReIndex()
        {
            this.CommandProcessor.Execute(new ReindexTopicsCommand());
            return this.Content("ok");
        }

        /// <inheritdoc />
        [Authorize(Roles = "Administrator")]
        public ActionResult ReIndexForum()
        {
            this.CommandProcessor.Execute(new ReindexForumTopicsCommand());
            return this.Content("ok");
        }

        /// <inheritdoc />
        [Authorize(Roles = "Administrator")]
        public ActionResult ImportFromIpb()
        {
            this.CommandProcessor.Execute(new ImportFromIpbCommand());
            return this.Content("ok");
        }

        /// <inheritdoc />
        [Authorize(Roles = "Administrator")]
        public ActionResult Events()
        {
            var eventLogItems = this.QueryProcessor.Process(new GetEventLogQuery(), new PaginationSettings(0, 100));
            ViewBag.SectionMode = "Events";

            return this.View(eventLogItems);
        }

        /// <inheritdoc />
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.CurrentSection = "Tools";
            base.OnActionExecuting(filterContext);
        }
    }
}
