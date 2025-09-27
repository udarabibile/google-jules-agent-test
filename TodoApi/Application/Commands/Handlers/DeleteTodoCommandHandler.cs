using System.Threading.Tasks;
using TodoApi.Domain;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Commands.Handlers
{
    public class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand>
    {
        private readonly IRepository<TodoItem> _repository;

        public DeleteTodoCommandHandler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public Task Handle(DeleteTodoCommand command)
        {
            var todo = _repository.GetById(command.Id);
            todo.Delete();
            _repository.Save(todo, todo.Version);
            return Task.CompletedTask;
        }
    }
}