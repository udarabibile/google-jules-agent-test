using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Domain;
using TodoApi.Application;

namespace TodoApi.Infrastructure
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<Guid, List<EventDescriptor>> _current = new Dictionary<Guid, List<EventDescriptor>>();
        private readonly IEventPublisher _publisher;

        public InMemoryEventStore(IEventPublisher publisher)
        {
            _publisher = publisher;
        }

        public void SaveEvents(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion)
        {
            List<EventDescriptor> eventDescriptors;
            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                eventDescriptors = new List<EventDescriptor>();
                _current.Add(aggregateId, eventDescriptors);
            }
            else if (eventDescriptors[eventDescriptors.Count - 1].Version != expectedVersion && expectedVersion != -1)
            {
                throw new ConcurrencyException();
            }

            var i = expectedVersion;
            foreach (var @event in events)
            {
                i++;
                @event.GetType().GetProperty("Version").SetValue(@event, i, null);
                eventDescriptors.Add(new EventDescriptor(aggregateId, @event, i));
                _publisher.Publish(@event);
            }
        }

        public List<IEvent> GetEventsForAggregate(Guid aggregateId)
        {
            List<EventDescriptor> eventDescriptors;
            if (!_current.TryGetValue(aggregateId, out eventDescriptors))
            {
                throw new AggregateNotFoundException();
            }
            return eventDescriptors.Select(desc => desc.EventData).ToList();
        }
    }

    public class EventDescriptor
    {
        public IEvent EventData { get; private set; }
        public Guid Id { get; private set; }
        public int Version { get; private set; }

        public EventDescriptor(Guid id, IEvent eventData, int version)
        {
            EventData = eventData;
            Version = version;
            Id = id;
        }
    }

    public class AggregateNotFoundException : Exception { }
    public class ConcurrencyException : Exception { }

    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }

    public class EventPublisher : IEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventPublisher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            // This is a simplified implementation. In a real-world scenario, you might use a message bus.
            // Here, we're just finding all registered handlers for this event type and invoking them.
            var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = (IEnumerable<object>)_serviceProvider.GetService(typeof(IEnumerable<>).MakeGenericType(handlerType));

            if (handlers != null)
            {
                foreach (var handler in handlers)
                {
                    ((dynamic)handler).Handle((dynamic)@event);
                }
            }
        }
    }

    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}