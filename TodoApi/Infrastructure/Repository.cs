using System;
using TodoApi.Domain;

namespace TodoApi.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;

        public Repository(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public T GetById(Guid id)
        {
            var obj = new T();
            var e = _eventStore.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }

        public void Save(T aggregate, int expectedVersion)
        {
            _eventStore.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }
    }
}