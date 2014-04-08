namespace MirGames.Controllers
{
    using System.Web.Mvc;

    using MirGames.Infrastructure;

    /// <summary>
    /// The forum controller.
    /// </summary>
    public class ErrorController : AppController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorController"/> class.
        /// </summary>
        /// <param name="queryProcessor">The query processor.</param>
        /// <param name="commandProcessor">The command processor.</param>
        public ErrorController(IQueryProcessor queryProcessor, ICommandProcessor commandProcessor)
            : base(queryProcessor, commandProcessor)
        {
        }

        /// <inheritdoc />
        public ActionResult General()
        {
            Response.StatusCode = 500;
            return View();
        }

        /// <inheritdoc />
        public ActionResult Error404()
        {
            Response.StatusCode = 404;
            return View();
        }

        /// <inheritdoc />
        public ActionResult Error500()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
