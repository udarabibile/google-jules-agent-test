using System;

namespace TodoApi.Application.Queries
{
    public class TodoReadModel
    {
        public Guid Id { get; set; }
    public string? Name { get; set; }
        public bool IsComplete { get; set; }
    }
}