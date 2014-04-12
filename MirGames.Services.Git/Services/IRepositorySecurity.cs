// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IRepositorySecurity.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Services.Git.Services
{
    public interface IRepositorySecurity
    {
        /// <summary>
        /// Determines whether this instance can write the specified repository name.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>True whther the specified repository could be writen.</returns>
        bool CanWrite(string repositoryName);

        /// <summary>
        /// Determines whether this instance can read the specified repository name.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>True whether the specified repository could be read.</returns>
        bool CanRead(string repositoryName);

        /// <summary>
        /// Determines whether this instance can delete the specified repository name.
        /// </summary>
        /// <param name="repositoryName">Name of the repository.</param>
        /// <returns>True whether the specified repository could be deleted.</returns>
        bool CanDelete(string repositoryName);
    }
}