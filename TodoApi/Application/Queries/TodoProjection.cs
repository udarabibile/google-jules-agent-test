using System;
using System.Collections.Generic;
using System.Linq;
using TodoApi.Domain;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Queries
{
    public class TodoProjection :
        IEventHandler<TodoCreated>,
        IEventHandler<TodoNameUpdated>,
        IEventHandler<TodoCompleted>,
        IEventHandler<TodoIncompleted>,
        IEventHandler<TodoDeleted>
    {
        private readonly Dictionary<Guid, TodoReadModel> _todos = new Dictionary<Guid, TodoReadModel>();

        public void Handle(TodoCreated @event)
        {
            _todos[@event.Id] = new TodoReadModel
            {
                Id = @event.Id,
                Name = @event.Name,
                IsComplete = false
            };
        }

        public void Handle(TodoNameUpdated @event)
        {
            if (_todos.TryGetValue(@event.Id, out var todo))
            {
                todo.Name = @event.NewName;
            }
        }

        public void Handle(TodoCompleted @event)
        {
            if (_todos.TryGetValue(@event.Id, out var todo))
            {
                todo.IsComplete = true;
            }
        }

        public void Handle(TodoIncompleted @event)
        {
            if (_todos.TryGetValue(@event.Id, out var todo))
            {
                todo.IsComplete = false;
            }
        }

        public TodoReadModel GetById(Guid id)
        {
            _todos.TryGetValue(id, out var todo);
            return todo;
        }

        public IEnumerable<TodoReadModel> GetAll()
        {
            return _todos.Values.ToList();
        }

        public void Handle(TodoDeleted @event)
        {
            _todos.Remove(@event.Id);
        }
    }
}