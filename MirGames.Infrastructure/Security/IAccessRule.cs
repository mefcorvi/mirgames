// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="IAccessRule.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Security
{
    using System;
    using System.Security.Claims;

    /// <summary>
    /// Rule that determines whether user have an access to the specified resource.
    /// </summary>
    /// <typeparam name="T">Type of resource.</typeparam>
    public interface IAccessRule<in T> : IAccessRule
    {
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>
        /// True whether access is granted; otherwise false.
        /// </returns>
        bool CheckAccess(ClaimsPrincipal principal, T resource);
    }

    /// <summary>
    /// Rule that determines whether user have an access to the specified resource.
    /// </summary>
    public interface IAccessRule
    {
        /// <summary>
        /// Gets the actions.
        /// </summary>
        string Action { get; }

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>
        /// True whether access is granted; otherwise false.
        /// </returns>
        bool CheckAccess(ClaimsPrincipal principal, object resource);
    }

    /// <summary>
    /// Rule that determines whether user have an access to the specified resource.
    /// </summary>
    /// <typeparam name="T">Type of resource.</typeparam>
    public abstract class AccessRule<T> : IAccessRule<T>
    {
        /// <inheritdoc />
        string IAccessRule.Action
        {
            get { return this.Action; }
        }

        /// <inheritdoc />
        Type IAccessRule.ResourceType
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        protected abstract string Action { get; }

        /// <inheritdoc />
        bool IAccessRule<T>.CheckAccess(ClaimsPrincipal principal, T resource)
        {
            return this.CheckAccess(principal, resource);
        }

        /// <inheritdoc />
        bool IAccessRule.CheckAccess(ClaimsPrincipal principal, object resource)
        {
            return this.CheckAccess(principal, (T)resource);
        }

        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="resource">The resource.</param>
        /// <returns>True whether access is granted; otherwise false.</returns>
        protected abstract bool CheckAccess(ClaimsPrincipal principal, T resource);
    }
}