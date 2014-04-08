namespace MirGames.Filters
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Attribute that could be used to mark that the specified action is valid only for ajax request.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        /// <inheritdoc />
        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return controllerContext.RequestContext.HttpContext.Request.IsAjaxRequest();
        }
    }
}