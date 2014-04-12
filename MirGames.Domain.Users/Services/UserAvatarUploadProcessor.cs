// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="UserAvatarUploadProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Domain.Users.Services
{
    using System;
    using System.IO;

    using ImageResizer;

    using MirGames.Domain.Attachments.Services;
    using MirGames.Infrastructure;

    /// <summary>
    /// The upload processor.
    /// </summary>
    internal sealed class UserAvatarUploadProcessor : IUploadProcessor
    {
        /// <inheritdoc />
        public bool CanProcess(string entityType)
        {
            return entityType.EqualsIgnoreCase("userAvatar");
        }

        /// <inheritdoc />
        public bool IsValid(string filePath)
        {
            try
            {
                ImageBuilder.Current.LoadImageInfo(filePath, null);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public void Process(string filePath)
        {
            string tempFile = Path.GetTempFileName();

            var resizeCropSettings = new ResizeSettings("width=100&height=100&crop=auto");
            ImageBuilder.Current.Build(filePath, tempFile, resizeCropSettings);
            File.Copy(tempFile, filePath, true);
            File.Delete(tempFile);
        }
    }
}
