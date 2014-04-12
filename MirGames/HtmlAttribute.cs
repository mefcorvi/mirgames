// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="HtmlAttribute.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace System.Web.Mvc
{
    public class HtmlAttribute : IHtmlString
    {
        /// <summary>
        /// The separator.
        /// </summary>
        private readonly string separator;

        /// <summary>
        /// The internal value.
        /// </summary>
        private string internalValue = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public HtmlAttribute(string name) : this(name, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="separator">The separator.</param>
        public HtmlAttribute(string name, string separator)
        {
            this.Name = name;
            this.separator = separator ?? " ";
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The attribute.</returns>
        public HtmlAttribute Add(string value)
        {
            return this.Add(value, true);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="condition">if set to <c>true</c> [condition].</param>
        /// <returns>The attribute.</returns>
        public HtmlAttribute Add(string value, bool condition)
        {
            if (!string.IsNullOrWhiteSpace(value) && condition)
            {
                this.internalValue += value + this.separator;
            }

            return this;
        }

        /// <inheritdoc />
        public string ToHtmlString()
        {
            string value = string.Empty;

            if (!string.IsNullOrWhiteSpace(this.internalValue))
            {
                value = string.Format(
                    "{0}=\"{1}\"",
                    this.Name,
                    this.internalValue.Substring(0, this.internalValue.Length - this.separator.Length));
            }

            return value;
        }
    }

    public static class Extensions
    {
        public static HtmlAttribute Css(this HtmlHelper html, string value)
        {
            return Css(html, value, true);
        }

        public static HtmlAttribute Css(this HtmlHelper html, string value, bool condition)
        {
            return Css(html, null, value, condition);
        }

        public static HtmlAttribute Css(this HtmlHelper html, string seperator, string value, bool condition)
        {
            return new HtmlAttribute("class", seperator).Add(value, condition);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, string value)
        {
            return Attr(html, name, value, true);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, string value, bool condition)
        {
            return Attr(html, name, null, value, condition);
        }

        public static HtmlAttribute Attr(this HtmlHelper html, string name, string seperator, string value, bool condition)
        {
            return new HtmlAttribute(name, seperator).Add(value, condition);
        }
    }
}