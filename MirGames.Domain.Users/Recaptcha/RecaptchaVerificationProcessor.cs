// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RecaptchaVerificationProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Domain.Users.Recaptcha
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Net;

    using MirGames.Infrastructure;

    /// <summary>
    /// Represents the functionality for verifying user's response to the recpatcha challenge.
    /// </summary>
    internal sealed class RecaptchaVerificationProcessor : IRecaptchaVerificationProcessor
    {
        private readonly IRecaptchaSettings settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaVerificationProcessor" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="InvalidOperationException">Private key cannot be null or empty.
        /// or
        /// Http request context does not exist.</exception>
        public RecaptchaVerificationProcessor(IRecaptchaSettings settings)
        {
            Contract.Requires(settings != null);
            this.settings = settings;
        }

        /// <inheritdoc />
        public RecaptchaVerificationResult Verify(string challenge, string response, string userHostAddress)
        {
            if (string.IsNullOrEmpty(challenge))
            {
                return RecaptchaVerificationResult.ChallengeNotProvided;
            }

            if (string.IsNullOrEmpty(response))
            {
                return RecaptchaVerificationResult.NullOrEmptyCaptchaSolution;
            }

            string privateKey = this.settings.GetPrivateKey();

            string postData = string.Format(
                "privatekey={0}&remoteip={1}&challenge={2}&response={3}",
                privateKey,
                userHostAddress,
                challenge,
                response);

            byte[] postDataBuffer = System.Text.Encoding.ASCII.GetBytes(postData);

            var verifyUri = new Uri(this.settings.GetVerificationUrl(), UriKind.Absolute);

            var webRequest = (HttpWebRequest)WebRequest.Create(verifyUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = postDataBuffer.Length;
            webRequest.Method = "POST";

            IWebProxy proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials;

            webRequest.Proxy = proxy;

            Stream requestStream = webRequest.GetRequestStream();
            requestStream.Write(postDataBuffer, 0, postDataBuffer.Length);

            var webResponse = (HttpWebResponse)webRequest.GetResponse();

            string[] responseTokens;
            using (var sr = new StreamReader(webResponse.GetResponseStream()))
            {
                responseTokens = sr.ReadToEnd().Split('\n');
            }

            if (responseTokens.Length == 2)
            {
                bool success = responseTokens[0].Equals("true", StringComparison.CurrentCulture);

                if (success)
                {
                    return RecaptchaVerificationResult.Success;
                }

                if (responseTokens[1].Equals("incorrect-captcha-sol", StringComparison.CurrentCulture))
                {
                    return RecaptchaVerificationResult.IncorrectCaptchaSolution;
                }

                if (responseTokens[1].Equals("invalid-site-private-key", StringComparison.CurrentCulture))
                {
                    return RecaptchaVerificationResult.InvalidPrivateKey;
                }

                if (responseTokens[1].Equals("invalid-request-cookie", StringComparison.CurrentCulture))
                {
                    return RecaptchaVerificationResult.InvalidCookieParameters;
                }
            }

            return RecaptchaVerificationResult.UnknownError;
        }
    }
}