// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="TransactionExecutor.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Transactions
{
    using System;
    using System.Transactions;

    /// <summary>
    /// Executes actions in transaction.
    /// </summary>
    internal sealed class TransactionExecutor : ITransactionExecutor
    {
        /// <inheritdoc />
        public void Execute(Action forward, Action rollback)
        {
            forward.Invoke();

            if (Transaction.Current != null)
            {
                var enlistNotification = new GenericTransactionEnlistment(rollback);
                Transaction.Current.EnlistVolatile(enlistNotification, EnlistmentOptions.None);
            }
        }

        /// <summary>
        /// The generic transaction enlistment.
        /// </summary>
        private sealed class GenericTransactionEnlistment : IEnlistmentNotification
        {
            /// <summary>
            /// The rollback action.
            /// </summary>
            private readonly Action rollback;

            /// <summary>
            /// Initializes a new instance of the <see cref="GenericTransactionEnlistment" /> class.
            /// </summary>
            /// <param name="rollback">The rollback.</param>
            public GenericTransactionEnlistment(Action rollback)
            {
                this.rollback = rollback;
            }

            /// <inheritdoc />
            public void Prepare(PreparingEnlistment preparingEnlistment)
            {
                preparingEnlistment.Prepared();
            }

            /// <inheritdoc />
            public void Commit(Enlistment enlistment)
            {
                enlistment.Done();
            }

            /// <inheritdoc />
            public void Rollback(Enlistment enlistment)
            {
                try
                {
                    this.rollback.Invoke();
                }
                finally
                {
                    enlistment.Done();
                }
            }

            /// <inheritdoc />
            public void InDoubt(Enlistment enlistment)
            {
                enlistment.Done();
            }
        }
    }
}