// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ITemplateProcessor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Notifications
{
    /// <summary>
    /// Processes the template.
    /// </summary>
    public interface ITemplateProcessor
    {
        /// <summary>
        /// Processes the specified template.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="template">The template.</param>
        /// <param name="model">The model.</param>
        /// <param name="cacheName">Name of the cache.</param>
        /// <returns>The result of processing.</returns>
        string Process<TModel>(string template, TModel model, string cacheName);
    }
}
