// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="AntiForgeryAttribute.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
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