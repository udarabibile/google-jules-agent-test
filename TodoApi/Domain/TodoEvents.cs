using System;

namespace TodoApi.Domain
{
    public class TodoCreated : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }

        public TodoCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class TodoNameUpdated : IEvent
    {
        public Guid Id { get; set; }
        public string NewName { get; set; }
        public int Version { get; set; }

        public TodoNameUpdated(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }

    public class TodoCompleted : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public TodoCompleted(Guid id)
        {
            Id = id;
        }
    }

    public class TodoIncompleted : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public TodoIncompleted(Guid id)
        {
            Id = id;
        }
    }

    public class TodoDeleted : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public TodoDeleted(Guid id)
        {
            Id = id;
        }
    }
}