using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                ApplyChange(e, false);
            }
            Version = history.Count();
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        // push atomic aggregate changes to local history for further processing (Event Store)
        private void ApplyChange(IEvent @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew)
            {
                _changes.Add(@event);
            }
        }
    }

    public static class AggregateRootExtensions
    {
        public static dynamic AsDynamic(this object o)
        {
            return o;
        }
    }
}