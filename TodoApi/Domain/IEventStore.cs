using System;
using System.Collections.Generic;

namespace TodoApi.Domain
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion);
        List<IEvent> GetEventsForAggregate(Guid aggregateId);
    }
}