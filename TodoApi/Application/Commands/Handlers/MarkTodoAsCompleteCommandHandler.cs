using System.Threading.Tasks;
using TodoApi.Domain;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Commands.Handlers
{
    public class MarkTodoAsCompleteCommandHandler : ICommandHandler<MarkTodoAsCompleteCommand>
    {
        private readonly IRepository<TodoItem> _repository;

        public MarkTodoAsCompleteCommandHandler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public Task Handle(MarkTodoAsCompleteCommand command)
        {
            var todo = _repository.GetById(command.Id);
            todo.MarkComplete();
            _repository.Save(todo, todo.Version);
            return Task.CompletedTask;
        }
    }
}