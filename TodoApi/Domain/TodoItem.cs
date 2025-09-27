using System;

namespace TodoApi.Domain
{
    public class TodoItem : AggregateRoot
    {
        public string Name { get; private set; }
        public bool IsComplete { get; private set; }

        // Parameterless constructor for event sourcing hydration
        public TodoItem() { }

        public TodoItem(Guid id, string name)
        {
            ApplyChange(new TodoCreated(id, name));
        }

        public void Apply(TodoCreated e)
        {
            Id = e.Id;
            Name = e.Name;
            IsComplete = false;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(newName));
            }
            if (newName == Name) return; // No change needed

            ApplyChange(new TodoNameUpdated(Id, newName));
        }

        public void Apply(TodoNameUpdated e)
        {
            Name = e.NewName;
        }

        public void MarkComplete()
        {
            if (IsComplete) return; // Already complete

            ApplyChange(new TodoCompleted(Id));
        }

        public void Apply(TodoCompleted e)
        {
            IsComplete = true;
        }

        public void MarkIncomplete()
        {
            if (!IsComplete) return; // Already incomplete

            ApplyChange(new TodoIncompleted(Id));
        }

        public void Apply(TodoIncompleted e)
        {
            IsComplete = false;
        }

        public bool IsDeleted { get; private set; }

        public void Delete()
        {
            if (IsDeleted) return; // Already deleted

            ApplyChange(new TodoDeleted(Id));
        }

        public void Apply(TodoDeleted e)
        {
            IsDeleted = true;
        }
    }
}