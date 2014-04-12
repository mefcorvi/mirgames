// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="OAuthToken.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Entities
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// The OAuth token.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
    internal sealed class OAuthToken
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the provider unique identifier.
        /// </summary>
        public int ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the provider user unique identifier.
        /// </summary>
        public string ProviderUserId { get; set; }

        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        public int UserId { get; set; }
    }
}