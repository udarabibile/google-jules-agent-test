using System;
using TodoApi.Application;

namespace TodoApi.Application.Commands
{
    public class MarkTodoAsIncompleteCommand : ICommand
    {
        public Guid Id { get; }

        public MarkTodoAsIncompleteCommand(Guid id)
        {
            Id = id;
        }
    }
}