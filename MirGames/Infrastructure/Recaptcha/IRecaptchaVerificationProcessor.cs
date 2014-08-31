// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IRecaptchaVerificationProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure
{
    /// <summary>
    /// Represents the functionality for verifying user's response to the recpatcha challenge.
    /// </summary>
    public interface IRecaptchaVerificationProcessor
    {
        /// <summary>
        /// Verifies whether the user's response to the recaptcha request is correct.
        /// </summary>
        /// <returns>Returns the result as a value of the <see cref="RecaptchaVerificationResult"/> enum.</returns>
        RecaptchaVerificationResult Verify();

        /// <summary>
        /// Verifies the Recaptcha.
        /// </summary>
        /// <param name="challenge">The challenge.</param>
        /// <param name="response">The response.</param>
        /// <param name="userHostAddress">The user host address.</param>
        /// <returns>
        /// A result of the verification.
        /// </returns>
        RecaptchaVerificationResult Verify(string challenge, string response, string userHostAddress);
    }
}