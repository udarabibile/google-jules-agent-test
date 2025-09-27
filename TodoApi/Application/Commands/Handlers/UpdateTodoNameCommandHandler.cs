using System.Threading.Tasks;
using TodoApi.Domain;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Commands.Handlers
{
    public class UpdateTodoNameCommandHandler : ICommandHandler<UpdateTodoNameCommand>
    {
        private readonly IRepository<TodoItem> _repository;

        public UpdateTodoNameCommandHandler(IRepository<TodoItem> repository)
        {
            _repository = repository;
        }

        public Task Handle(UpdateTodoNameCommand command)
        {
            var todo = _repository.GetById(command.Id);
            todo.UpdateName(command.NewName);
            _repository.Save(todo, todo.Version);
            return Task.CompletedTask;
        }
    }
}