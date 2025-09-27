using System.Collections.Generic;

namespace TodoApi.Application.Queries
{
    public class GetAllTodosQuery : IQuery<IEnumerable<TodoReadModel>>
    {
    }
}