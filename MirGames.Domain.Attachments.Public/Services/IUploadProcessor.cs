// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IUploadProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Attachments.Services
{
    /// <summary>
    /// Processes the uploaded file.
    /// </summary>
    public interface IUploadProcessor
    {
        /// <summary>
        /// Determines whether this instance can process the specified entity type.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>True whether this instance can process the specified entity type.</returns>
        bool CanProcess(string entityType);

        /// <summary>
        /// Determines whether the specified file is valid.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>True whether the specified file is valid.</returns>
        bool IsValid(string filePath);

        /// <summary>
        /// Processes the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void Process(string filePath);
    }
}
