namespace MirGames
{
    using System.Web.Helpers;
    using System.Web.Mvc;

    /// <summary>
    /// Validates anti forgery token.
    /// </summary>
    public class AntiForgeryAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var request = filterContext.HttpContext.Request;

            var cookie = request.Cookies[AntiForgeryConfig.CookieName];
            var formToken = request.Form["__RequestVerificationToken"];
            var rvt = request.Headers["__RequestVerificationToken"];
            AntiForgery.Validate(cookie != null ? cookie.Value : null, formToken ?? rvt);
        }
    }
}