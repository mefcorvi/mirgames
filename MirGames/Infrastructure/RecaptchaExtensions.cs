// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RecaptchaExtensions.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure
{
    using System;
    using System.Web;

    using MirGames.Domain.Users.Recaptcha;

    public static class RecaptchaExtensions
    {
        /// <summary>
        /// Verifies the specified recaptcha verification processor.
        /// </summary>
        /// <param name="recaptchaVerificationProcessor">The recaptcha verification processor.</param>
        /// <returns>The result of verification.</returns>
        public static RecaptchaVerificationResult Verify(this IRecaptchaVerificationProcessor recaptchaVerificationProcessor)
        {
            var httpContext = HttpContext.Current;

            if (httpContext == null || httpContext.Request == null)
            {
                throw new InvalidOperationException("Http request context does not exist.");
            }

            string challenge = httpContext.Request.Form["recaptcha_challenge_field"]
                               ?? httpContext.Request.Params["recaptcha_challenge_field"];

            string response = httpContext.Request.Form["recaptcha_response_field"]
                              ?? httpContext.Request.Params["recaptcha_response_field"];

            string userHostAddress = httpContext.Request.UserHostAddress;

            return recaptchaVerificationProcessor.Verify(challenge, response, userHostAddress);
        }
    }
}