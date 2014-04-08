namespace MirGames.Infrastructure.Transactions
{
    using System;

    /// <summary>
    /// Executes action in transaction.
    /// </summary>
    public interface ITransactionExecutor
    {
        /// <summary>
        /// Executes the action in transaction.
        /// </summary>
        /// <param name="forward">The forward action.</param>
        /// <param name="rollback">The rollback action.</param>
        void Execute(Action forward, Action rollback);
    }
}