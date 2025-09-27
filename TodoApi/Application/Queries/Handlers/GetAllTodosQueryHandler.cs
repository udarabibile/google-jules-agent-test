using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Infrastructure;

namespace TodoApi.Application.Queries.Handlers
{
    public class GetAllTodosQueryHandler : IQueryHandler<GetAllTodosQuery, IEnumerable<TodoReadModel>>
    {
        private readonly TodoProjection _projection;

        public GetAllTodosQueryHandler(TodoProjection projection)
        {
            _projection = projection;
        }

        public Task<IEnumerable<TodoReadModel>> Handle(GetAllTodosQuery query)
        {
            return Task.FromResult(_projection.GetAll());
        }
    }
}