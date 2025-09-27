using System.Threading.Tasks;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Queries.Handlers
{
    public class GetTodoByIdQueryHandler : IQueryHandler<GetTodoByIdQuery, TodoReadModel>
    {
        private readonly TodoProjection _projection;

        public GetTodoByIdQueryHandler(TodoProjection projection)
        {
            _projection = projection;
        }

        public Task<TodoReadModel> Handle(GetTodoByIdQuery query)
        {
            return Task.FromResult(_projection.GetById(query.Id));
        }
    }
}