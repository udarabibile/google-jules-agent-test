using System.Threading.Tasks;
using TodoApi.Domain;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Commands.Handlers
{
    public class MarkTodoAsIncompleteCommandHandler : ICommandHandler<MarkTodoAsIncompleteCommand>
    {
        private readonly IRepository<TodoItem> _repository;

        public MarkTodoAsIncompleteCommandHandler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public Task Handle(MarkTodoAsIncompleteCommand command)
        {
            var todo = _repository.GetById(command.Id);
            todo.MarkIncomplete();
            _repository.Save(todo, todo.Version);
            return Task.CompletedTask;
        }
    }
}