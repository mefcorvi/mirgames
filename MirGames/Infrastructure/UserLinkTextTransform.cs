// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserLinkTextTransform.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames
{
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    using MirGames.Domain.TextTransform;

    /// <summary>
    /// Converts links.
    /// </summary>
    public sealed class UserLinkTextTransform : ITextTransform
    {
        /// <summary>
        /// The regular expression.
        /// </summary>
        private readonly Regex regularExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLinkTextTransform"/> class.
        /// </summary>
        public UserLinkTextTransform()
        {
            const string Regex = @"<user id=""([0-9]+?)"">([^<]+?)</user>";
            this.regularExpression = new Regex(Regex, RegexOptions.IgnoreCase);
        }

        /// <inheritdoc />
        public string Transform(string text)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null)
            {
                var request = new HttpRequest("/", "http://mirgames.ru", string.Empty);
                var response = new HttpResponse(new StringWriter());
                httpContext = new HttpContext(request, response);
            }

            var httpContextBase = new HttpContextWrapper(httpContext);
            var routeData = new RouteData();
            var requestContext = new RequestContext(httpContextBase, routeData);

            return this.regularExpression.Replace(
                text,
                m =>
                    {
                        if (m.Groups[1].Length > 0 && m.Value.EndsWith(")"))
                        {
                            return m.Value;
                        }

                        var url = new UrlHelper(requestContext);
                        var link = url.Action(MVC.Users.Profile(int.Parse(m.Groups[1].Value)));

                        return string.Format("[{0}]({1})", m.Groups[2].Value, link);
                    });
        }
    }
}