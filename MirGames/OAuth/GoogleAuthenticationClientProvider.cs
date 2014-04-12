// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="GoogleAuthenticationClientProvider.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.OAuth
{
    using System.Diagnostics.Contracts;

    using DotNetOpenAuth.AspNet;
    using DotNetOpenAuth.GoogleOAuth2;

    using MirGames.Infrastructure;

    /// <summary>
    /// The google authentication client.
    /// </summary>
    internal sealed class GoogleAuthenticationClientProvider : IAuthenticationClientProvider
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoogleAuthenticationClientProvider" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public GoogleAuthenticationClientProvider(ISettings settings)
        {
            Contract.Requires(settings != null);

            this.settings = settings;
        }

        /// <inheritdoc />
        public IAuthenticationClient GetClient()
        {
            return new GoogleOAuth2Client(this.settings.GetValue<string>("Google.AppID"), this.settings.GetValue<string>("Google.AppSecret"));
        }
    }
}