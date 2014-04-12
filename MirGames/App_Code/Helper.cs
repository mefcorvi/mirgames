// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="Helper.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace System.Web.Mvc.Html
{
    using System.Web.WebPages;

    /// <summary>
    /// Base helper class.
    /// </summary>
    public class Helper : HelperPage
    {
        /// <summary>
        /// Gets the HTML helper.
        /// </summary>
        public static new WebViewPage Page
        {
            get { return (WebViewPage)WebPageContext.Current.Page; }
        }

        /// <summary>
        /// Gets the HTML helper.
        /// </summary>
        public static new HtmlHelper Html
        {
            get { return ((WebViewPage)WebPageContext.Current.Page).Html; }
        }

        /// <summary>
        /// Gets the HTML helper.
        /// </summary>
        public static UrlHelper Url
        {
            get { return ((WebViewPage)WebPageContext.Current.Page).Url; }
        }
    }
}