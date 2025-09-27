using System;

namespace TodoApi.Application.Queries
{
    public class GetTodoByIdQuery : IQuery<TodoReadModel>
    {
        public Guid Id { get; }

        public GetTodoByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}