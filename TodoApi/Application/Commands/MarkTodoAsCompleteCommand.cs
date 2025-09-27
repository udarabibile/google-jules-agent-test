using System;
using TodoApi.Application;

namespace TodoApi.Application.Commands
{
    public class MarkTodoAsCompleteCommand : ICommand
    {
        public Guid Id { get; }

        public MarkTodoAsCompleteCommand(Guid id)
        {
            Id = id;
        }
    }
}