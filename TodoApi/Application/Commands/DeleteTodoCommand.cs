using System;
using TodoApi.Application;

namespace TodoApi.Application.Commands
{
    public class DeleteTodoCommand : ICommand
    {
        public Guid Id { get; }

        public DeleteTodoCommand(Guid id)
        {
            Id = id;
        }
    }
}