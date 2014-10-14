// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RecaptchaSettings.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.Recaptcha
{
    using System.Diagnostics.Contracts;

    using MirGames.Infrastructure;

    /// <summary>
    /// Settings of the Recaptcha.
    /// </summary>
    internal sealed class RecaptchaSettings : IRecaptchaSettings
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaSettings"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public RecaptchaSettings(ISettings settings)
        {
            Contract.Requires(settings != null);
            this.settings = settings;
        }

        /// <inheritdoc />
        public string GetVerificationUrl()
        {
            return this.settings.GetValue<string>("Recaptcha.VerificationUrl");
        }

        /// <inheritdoc />
        public string GetPublicKey()
        {
            return this.settings.GetValue<string>("Recaptcha.PublicKey");
        }

        /// <inheritdoc />
        public string GetPrivateKey()
        {
            return this.settings.GetValue<string>("Recaptcha.PrivateKey");
        }
    }
}