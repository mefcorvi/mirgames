namespace MirGames.Infrastructure.Events
{
    using System;

    /// <summary>
    /// The generic game events listener.
    /// </summary>
    /// <typeparam name="T">Type of the event.</typeparam>
    public sealed class GenericListener<T> : EventListenerBase<T> where T : Event, new()
    {
        /// <summary>
        /// The callback.
        /// </summary>
        private readonly Action<T> callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericListener{T}" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public GenericListener(Action<T> callback)
        {
            this.callback = callback;
        }

        /// <inheritdoc />
        public override void Process(T @event)
        {
            this.callback(@event);
        }
    }
}