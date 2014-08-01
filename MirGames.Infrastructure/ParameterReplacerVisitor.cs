// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="ParameterReplacerVisitor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MirGames.Infrastructure
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Substitutes mediate parameter by input parameter in the specified function.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TMediate">The type of the mediate.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public sealed class ParameterReplacerVisitor<TInput, TMediate, TOutput> : ExpressionVisitor
    {
        private readonly ParameterExpression sourceParameter;
        private readonly Expression<Func<TInput, TMediate>> substitutionExpression;

        public ParameterReplacerVisitor(ParameterExpression sourceParameter, Expression<Func<TInput, TMediate>> substitutionExpression)
        {
            this.sourceParameter = sourceParameter;
            this.substitutionExpression = substitutionExpression;
        }

        /// <summary>
        /// Visits the expression and convert.
        /// </summary>
        /// <param name="root">The root.</param>
        /// <returns>The result expression.</returns>
        public Expression<Func<TInput, TOutput>> VisitAndConvert(Expression<Func<TMediate, TOutput>> root)
        {
            return (Expression<Func<TInput, TOutput>>)this.VisitLambda(root);
        }

        /// <inheritdoc />
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            var parameters = node.Parameters.Where(p => p != this.sourceParameter).ToList();
            parameters.Add(this.substitutionExpression.Parameters.Single());

            return Expression.Lambda<Func<TInput, TOutput>>(this.Visit(node.Body), parameters);
        }

        /// <inheritdoc />
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == this.sourceParameter ? this.substitutionExpression.Body : base.VisitParameter(node);
        }
    }
}