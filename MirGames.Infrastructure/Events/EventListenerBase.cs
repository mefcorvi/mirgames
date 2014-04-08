namespace MirGames.Infrastructure.Events
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// The game listener.
    /// </summary>
    /// <typeparam name="T">Type of the event.</typeparam>
    public abstract class EventListenerBase<T> : IEventListener where T : Event, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventListenerBase{T}"/> class.
        /// </summary>
        protected EventListenerBase()
        {
            var dummyEvent = new T();
            this.SupportedEventType = dummyEvent.EventType;
        }

        /// <inheritdoc />
        IEnumerable<string> IEventListener.SupportedEventTypes
        {
            get { return new[] { this.SupportedEventType }; }
        }

        /// <summary>
        /// Gets or sets the type of the supported event.
        /// </summary>
        protected string SupportedEventType { get; set; }

        /// <inheritdoc />
        void IEventListener.Process(Event @event)
        {
            if (@event is T)
            {
                this.Process((T)@event);
            }
            else
            {
                T convertedEvent;

                try
                {
                    var serializedObject = JsonConvert.SerializeObject(@event);
                    convertedEvent = JsonConvert.DeserializeObject<T>(serializedObject);
                }
                catch (Exception e)
                {
                    throw new UnsupportedEventTypeException(@event.GetType(), typeof(T), e);
                }

                this.Process(convertedEvent);
            }
        }

        /// <inheritdoc />
        bool IEventListener.CanProcess(Event @event)
        {
            return string.Equals(@event.EventType, this.SupportedEventType, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Processes the specified game event.
        /// </summary>
        /// <param name="event">The game event.</param>
        public abstract void Process(T @event);
    }
}