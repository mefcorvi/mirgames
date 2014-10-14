// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RecaptchaVerificationResult.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure
{
    /// <summary>
    /// Represents the result value of recaptcha verification process.
    /// </summary>
    public enum RecaptchaVerificationResult
    {
        /// <summary>
        /// Verification failed but the exact reason is not known.
        /// </summary>
        UnknownError = 0,
        
        /// <summary>
        /// Verification succeeded.
        /// </summary>
        Success = 1,

        /// <summary>
        /// The user's response to recaptcha challenge is incorrect.
        /// </summary>
        IncorrectCaptchaSolution = 2,

        /// <summary>
        /// The request parameters in the client-side cookie are invalid.
        /// </summary>
        InvalidCookieParameters = 3,
        
        /// <summary>
        /// The private supplied at the time of verification process is invalid.
        /// </summary>
        InvalidPrivateKey = 4,
        
        /// <summary>
        /// The user's response to the recaptcha challenge is null or empty.
        /// </summary>
        NullOrEmptyCaptchaSolution = 5,
        
        /// <summary>
        /// The recaptcha challenge could not be retrieved.
        /// </summary>
        ChallengeNotProvided = 6
    }
}
