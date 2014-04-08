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