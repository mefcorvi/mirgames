// --------------------------------------------------------------------------------------------------------------------
// <copyright company="MirGames" file="EventBus.cs">
// Copyright 2014 Bulat Aykaev
// This file is part of MirGames.
// MirGames is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
// MirGames is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. You should have received a copy of the GNU General Public License along with MirGames. If not, see http://www.gnu.org/licenses/.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace MirGames.Infrastructure.Events
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Threading.Tasks;

    using MirGames.Infrastructure.Logging;

    /// <summary>
    /// The event bus.
    /// </summary>
    internal sealed class EventBus : IEventBus
    {
        /// <summary>
        /// The settings.
        /// </summary>
        private readonly ISettings settings;

        /// <summary>
        /// The event log.
        /// </summary>
        private readonly IEventLog eventLog;

        /// <summary>
        /// The lock object.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The listeners by event type.
        /// </summary>
        private readonly Lazy<IDictionary<string, ICollection<IEventListener>>> listenersByEventTypes; 

        /// <summary>
        /// Initializes a new instance of the <see cref="EventBus" /> class.
        /// </summary>
        /// <param name="listeners">The listeners.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="eventLog">The event log.</param>
        public EventBus(Lazy<IEnumerable<IEventListener>> listeners, ISettings settings, IEventLog eventLog)
        {
            Contract.Requires(listeners != null);
            Contract.Requires(settings != null);
            Contract.Requires(eventLog != null);

            this.settings = settings;
            this.eventLog = eventLog;

            this.listenersByEventTypes = new Lazy<IDictionary<string, ICollection<IEventListener>>>(() =>
                {
                    var dictionary = new Dictionary<string, ICollection<IEventListener>>();

                    foreach (var listener in listeners.Value)
                    {
                        foreach (var eventType in listener.SupportedEventTypes)
                        {
                            if (!dictionary.ContainsKey(eventType))
                            {
                                dictionary[eventType] = new List<IEventListener>();
                            }

                            dictionary[eventType].Add(listener);
                        }
                    }

                    return dictionary;
                });
        }

        /// <summary>
        /// Adds the listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void AddListener(IEventListener listener)
        {
            Contract.Requires(listener != null);

            lock (this.lockObject)
            {
                var listeners = this.listenersByEventTypes.Value;

                foreach (string supportedEventType in listener.SupportedEventTypes)
                {
                    if (!listeners.ContainsKey(supportedEventType))
                    {
                        listeners[supportedEventType] = new List<IEventListener>();
                    }

                    listeners[supportedEventType].Add(listener);
                }
            }
        }

        /// <summary>
        /// Raises the specified event.
        /// </summary>
        /// <param name="event">The event.</param>
        public void Raise(Event @event)
        {
            Contract.Requires(@event != null);

            Task.Factory.StartNew(
                () =>
                    {
                        IEventListener[] currentListeners;

                        lock (this.lockObject)
                        {
                            var listeners = this.listenersByEventTypes.Value;

                            if (!listeners.ContainsKey(@event.EventType))
                            {
                                return;
                            }

                            currentListeners = listeners[@event.EventType].ToArray();
                        }

                        for (int i = 0; i < currentListeners.Length; i++)
                        {
                            if (currentListeners[i].CanProcess(@event))
                            {
                                try
                                {
                                    currentListeners[i].Process(@event);
                                }
                                catch (Exception e)
                                {
                                    this.eventLog.Log(
                                        EventLogType.Error,
                                        "EventBus",
                                        string.Format("Exception occured during processing of event {0}", @event.EventType),
                                        new 
                                        {
                                            Exception = e
                                        });
                                }
                            }
                        }

                        this.TraceEvent(@event, currentListeners);
                    });
        }

        /// <summary>
        /// Traces the event.
        /// </summary>
        /// <param name="event">The event.</param>
        /// <param name="currentListeners">The current listeners.</param>
        private void TraceEvent(Event @event, IEventListener[] currentListeners)
        {
            if (this.settings.GetValue("EventBus.TraceEnabled", false))
            {
                this.eventLog.Log(
                    EventLogType.Verbose,
                    "EventBus.RaiseEvent",
                    string.Format("Event {0} have been handled. Listeners count: {1}", @event.EventType, currentListeners.Length),
                    @event);
            }
        }
    }
}
