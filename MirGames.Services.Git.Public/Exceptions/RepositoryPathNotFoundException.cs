// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="RepositoryPathNotFoundException.cs">
// Copyright 2014 Bulat Aykaev
// 
// This file is part of MirGames.
// 
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// 
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.Public.Exceptions
{
    using System;

    public class RepositoryPathNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryPathNotFoundException"/> class.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        public RepositoryPathNotFoundException(string relativePath)
        {
            this.RelativePath = relativePath;
        }

        /// <summary>
        /// Gets the relative path.
        /// </summary>
        public string RelativePath { get; private set; }
    }
}