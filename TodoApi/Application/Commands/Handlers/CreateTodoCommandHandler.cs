using System.Threading.Tasks;
using TodoApi.Domain;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Commands.Handlers
{
    public class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand>
    {
        private readonly IRepository<TodoItem> _repository;

        public CreateTodoCommandHandler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public Task Handle(CreateTodoCommand command)
        {
            var todo = new TodoItem(command.Id, command.Name);
            // The initial version is -1 because it's a new aggregate.
            _repository.Save(todo, -1);
            return Task.CompletedTask;
        }
    }
}